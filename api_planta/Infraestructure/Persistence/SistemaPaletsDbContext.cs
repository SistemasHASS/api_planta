using Microsoft.EntityFrameworkCore;

namespace api_planta.Infraestructure.Persistence
{
    public class SistemaPaletsDbContext : DbContext
    {
        public SistemaPaletsDbContext(DbContextOptions<SistemaPaletsDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=SistemaPalets;Trusted_Connection=true;TrustServerCertificate=true;");
            }
        }
    }
}
