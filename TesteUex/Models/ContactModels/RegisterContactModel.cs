using TesteUex.Helpers;

namespace TesteUex.Models.ContactModels
{
    public class RegisterContactModel
    {
        public required string Name { get; set; }
        public required string Cpf { get; set; }
        public required string Phone { get; set; }
        public int? UserId { get; set; }
        public required string Street { get; set; }
        public required string State { get; set; }
        public required string City { get; set; }
        public required string District { get; set; }
        public required string Cep { get; set; }
        public required int Number { get; set; }
        public string? Complement { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

