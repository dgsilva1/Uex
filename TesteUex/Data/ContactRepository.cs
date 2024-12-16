using TesteUex.Entities;
using TesteUex.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TesteUex.Data
{
    public class ContactRepository(DataContext context) : IContactRepository
    {
        public async Task<Contact?> GetContactById(int contactId)
        {
            return await context.Contacts
                .SingleOrDefaultAsync(e => e.ContactId == contactId);
        }

        public async Task<bool> CheckCpfExists(int userId, string cpf)
        {
            return await context.Contacts.AnyAsync(x => x.UserId == userId && x.Cpf == cpf);
     
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void Update(Contact Contact)
        {
            context.Entry(Contact).State = EntityState.Modified;
        }
    }
}
