using TesteUex.Data;
using TesteUex.Entities;
using TesteUex.Interfaces;
using TesteUex.Models.AccountModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TesteUex.Models.UserModels;
using TesteUex.Helpers;
using AutoMapper;
using TesteUex.Models.ContactModels;

namespace TesteUex.Controllers
{
    public class AuthController(DataContext context, ITokenService tokenService, IMapper mapper) : BaseApiController
    {
        [HttpPost("register")] 
        public async Task<ActionResult<ResponseLoginModel>> Register(RegisterModel registerModel)
        {
            if (!Validators.ValidateEmail(registerModel.Email))
                return BadRequest("Email inválido");

            if (await CheckLoginUserExist(registerModel.Login.ToLower()))
                return BadRequest("Login já em uso!");

            if (await CheckEmailUserExist(registerModel.Email.ToLower()))
                return BadRequest("Email já em uso!");


            using var hmac = new HMACSHA512();

            var user = mapper.Map<User>(registerModel);

            user.Name = registerModel.Name.ToUpper();
            user.Login = registerModel.Login.ToLower();
            user.Email = registerModel.Email.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerModel.Password));
            user.PasswordSalt = hmac.Key;

            context.Users.Add(user);

            await context.SaveChangesAsync();

            var firstName = user.Name.Split(' ').First().ToLower();
            firstName = char.ToUpper(firstName[0]) + firstName.Substring(1);

            var mappedContacts = mapper.Map<List<ContactModel>>(user.Contacts?.ToList());

            return new ResponseLoginModel
            {
                Login = user.Login,
                Token = tokenService.CreateToken(user),
                FirstName = firstName,
                Contacts = mappedContacts
            };
 
        }

        [HttpPost("login")] 
        public async Task<ActionResult<ResponseLoginModel>> Login(LoginModel loginModel)
        {
            var user = await context.Users
                .Include(e => e.Contacts)
                .SingleOrDefaultAsync(e => e.Login == loginModel.Login);

            if (user == null)
                return Unauthorized("Login inválido!");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginModel.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return Unauthorized("Senha inválida!");
            }

            var firstName = user.Name.Split(' ').First().ToLower();
            firstName = char.ToUpper(firstName[0]) + firstName.Substring(1);

            var mappedContacts = mapper.Map<List<ContactModel>>(user.Contacts?.ToList());

            return new ResponseLoginModel
            {
                Login = loginModel.Login,
                Token = tokenService.CreateToken(user),
                FirstName = firstName,
                Contacts = mappedContacts
            };
        }

        private async Task<bool> CheckLoginUserExist(string login)
        {
            return await context.Users.AnyAsync(x => x.Login == login);
        }

        private async Task<bool> CheckEmailUserExist(string email)
        {
            return await context.Users.AnyAsync(x => x.Email == email);
        }
    }
}
