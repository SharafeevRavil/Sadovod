using Microsoft.EntityFrameworkCore;
using SadovodBack.Entities;

namespace SadovodBack.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}