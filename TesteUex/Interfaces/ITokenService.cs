using TesteUex.Entities;

namespace TesteUex.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
