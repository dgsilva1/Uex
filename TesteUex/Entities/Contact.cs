using System.ComponentModel.DataAnnotations;

namespace TesteUex.Entities
{
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }
        public required string Name { get; set; }
        public required string Cpf { get; set; }
        public required string Phone { get; set; }
        public required string Street { get; set; }
        public required string State { get; set; }
        public required string City { get; set; }
        public required string District { get; set; }
        public required string Cep { get; set; }
        public required int Number { get; set; }
        public string? Complement { get; set; }
        public required double Latitude { get; set; }
        public required double Longitude { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
