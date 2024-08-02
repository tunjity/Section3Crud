using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Section3Crud.Model
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext()
        {

        }
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        public virtual DbSet<Product> Products { get; set; }

    }
}
