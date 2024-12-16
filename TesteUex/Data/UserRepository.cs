using TesteUex.Entities;
using TesteUex.Interfaces;
using TesteUex.Models.AccountModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TesteUex.Models.ContactModels;

namespace TesteUex.Data
{
    public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
    {
        public async Task<User?> GetUserByLoginAsync(string login)
        {
            return await context.Users
                .Include(e => e.Contacts)
                .SingleOrDefaultAsync(e => e.Login == login);
        }

        public async Task<UserModel?> GetUserModelByLoginAsync(int userId)
        {
            return await context.Users
              .Where(e => e.UserId == userId)
              .ProjectTo<UserModel>(mapper.ConfigurationProvider)
              .SingleOrDefaultAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void Update(User User)
        {
            context.Entry(User).State = EntityState.Modified;
        }

        public void Remove(User User)
        {
            context.Users.Remove(User);
        }
    }
}
