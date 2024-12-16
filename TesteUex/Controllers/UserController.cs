using System.Security.Cryptography;
using System.Text;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TesteUex.Interfaces;

namespace TesteUex.Controllers
{
    [Authorize]
    public class UserController(IUserRepository userRepository) : BaseApiController
    {
        [HttpDelete("delete-account/{password}")]
        public async Task<ActionResult<object>> DeletAccount(string password)
        {
            var user = await userRepository.GetUserByLoginAsync(User.GetLoginUsuario());

            if (user == null)
                return BadRequest("Não foi possível obter o login pela sessão, tente relogar.");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return Unauthorized("Senha inválida!");
            }

            userRepository.Remove(user);

            if (await userRepository.SaveAllAsync())
                return Ok();

            return BadRequest("Não foi possível excluir o usuário.");
        }

    }
}
