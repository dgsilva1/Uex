using TesteUex.Entities;
using TesteUex.Models.ContactModels;

namespace TesteUex.Models.UserModels
{
    public class ResponseLoginModel
    {
        public string? Login { get; set; }
        public string? Token { get; set; }
        public string? FirstName { get; set; }
        public List<ContactModel>? Contacts { get; set; }
    }
}
