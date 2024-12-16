using TesteUex.Entities;
using Microsoft.EntityFrameworkCore;

namespace TesteUex.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
    }
}
