using Microsoft.EntityFrameworkCore;

namespace api_planta.Infrastructure.Persistence
{
    public class SistemaPaletsDbContext : DbContext
    {
        public SistemaPaletsDbContext(DbContextOptions<SistemaPaletsDbContext> options) : base(options)
        {
        }
    }
}
