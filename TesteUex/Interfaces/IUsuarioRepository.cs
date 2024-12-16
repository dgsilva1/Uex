using TesteUex.Entities;
using TesteUex.Models.AccountModels;
using TesteUex.Models.ContactModels;

namespace TesteUex.Interfaces
{
    public interface IUserRepository
    {
        void Update(User User);
        void Remove(User User);

        Task<bool> SaveAllAsync();
        Task<User?> GetUserByLoginAsync(string login);
        Task<UserModel?> GetUserModelByLoginAsync(int id);
    
    }
}
