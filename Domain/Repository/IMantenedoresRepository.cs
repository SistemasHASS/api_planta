
namespace api_planta.Domain.Repository
{
    public interface IMantenedoresRepository
    {
        Task<bool> EliminarVehiculoAsync(int id);
    }
}
