using Microsoft.EntityFrameworkCore;

namespace SadovodBack
{
    public class DataContext : DbContext
    {
        /*public DataContext(DbContextOptions<DataContext> options) : base(options)
		{
			//Database.EnsureCreated();
		}*/
        public DataContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=tcp:sadovodhelperexampledbserver.database.windows.net,1433;Initial Catalog=Users;Persist Security Info=False;User ID=Hikirangi;Password=Satana666;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        public DbSet<User> Users { get; set; }
    }
}