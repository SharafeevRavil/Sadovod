using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SadovodBack
{
    public class SteadContext:DbContext
    {
        public SteadContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=tcp:sadovodhelperexampledbserver.database.windows.net,1433;Initial Catalog=Steads;Persist Security Info=False;User ID=Hikirangi;Password=Satana666;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        public DbSet<SteadData> Steads { get; set; }
    }
}
