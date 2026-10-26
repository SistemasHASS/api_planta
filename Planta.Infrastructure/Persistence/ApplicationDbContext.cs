using Microsoft.EntityFrameworkCore;

namespace Planta.Infrastructure.Persistence;

/// <summary>
/// Se usa para la conexion y el ciclo de vida con SQL Server. Los datos se leen y escriben
/// con stored procedures desde los repositorios (ver BaseRepository), no con DbSet.
/// </summary>
public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
}
