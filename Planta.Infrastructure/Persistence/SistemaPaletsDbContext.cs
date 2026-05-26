using Microsoft.EntityFrameworkCore;

namespace Planta.Infrastructure.Persistence;

public sealed class SistemaPaletsDbContext(DbContextOptions<SistemaPaletsDbContext> options) : DbContext(options)
{
}
