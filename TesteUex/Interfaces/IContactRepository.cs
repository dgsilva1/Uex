using TesteUex.Entities;
using TesteUex.Models.ContactModels;

namespace TesteUex.Interfaces
{
    public interface IContactRepository
    {
        void Update(Contact contact);

        Task<bool> SaveAllAsync();
        Task<Contact?> GetContactById(int contactId);
        Task<bool> CheckCpfExists(int userId, string cpf);
    }
}
