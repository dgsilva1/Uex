using TesteUex.Extensions;
using System.ComponentModel.DataAnnotations;

namespace TesteUex.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public required string Name { get; set; }
        public required string Login { get; set; }
        public byte[] PasswordHash { get; set; } = [];
        public byte[] PasswordSalt { get; set; } = [];
        public required string Email { get; set; }
        public List<Contact>? Contacts { get; set; }
    }
}
