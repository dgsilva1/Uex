using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetLoginUsuario(this ClaimsPrincipal usuario)
        {
            var loginUsuario = usuario.FindFirstValue(ClaimTypes.NameIdentifier);

            if (loginUsuario == null)
                throw new Exception("Não foi possível obter o login do usuário.");

            return loginUsuario;
        }
    }
}
