namespace api_planta.Domain.UseCase
{
    public interface IMantenedoresUseCase
    {
        Task<bool> EliminarVehiculoAsync(int id);
    }
}
