using System.Text.Json;

namespace api_planta.Domain.UseCase
{
    public interface IPaletUseCase
    {
        Task<List<JsonElement>> CrearPaletAsync(string json);
        Task<List<JsonElement>> ObtenerPaletsPorProcesoAsync(string json);
        Task<List<JsonElement>> ObtenerPaletPorIdAsync(string json);
        Task<List<JsonElement>> AgregarCajasAsync(string json);
        Task<List<JsonElement>> CerrarPaletSaldoAsync(string json);
        Task<List<JsonElement>> ReabrirPaletAsync(string json);
        Task<List<JsonElement>> EliminarPaletAsync(string json);
        Task<List<JsonElement>> EditarCajasAsync(string json);
        Task<List<JsonElement>> EliminarComposicionAsync(string json);
        Task<List<JsonElement>> ObtenerDestinosPorConsignatarioAsync(string json);
        Task<List<JsonElement>> ObtenerConsignatariosPorDestinoAsync(string json);
        Task<List<JsonElement>> ObtenerFormatosAsync(string json);
        Task<List<JsonElement>> ObtenerTiposEmpaqueGuiaAsync(string json);
        Task<List<JsonElement>> ObtenerCalibresAsync(string json);
        Task<List<JsonElement>> ObtenerPresentacionesAsync(string json);
        Task<List<JsonElement>> ObtenerTiposDesdeMatrizAsync(string json);
        Task<List<JsonElement>> ObtenerCodigosRanchoAsync(string json);
        Task<List<JsonElement>> VerificarDriscollAsync(string json);
        Task<List<JsonElement>> ObtenerVariedadesPorConsignatarioAsync(string json);
        Task<List<JsonElement>> ObtenerConfigTipoProcesoAsync(string json);

        // Procesos
        Task<List<JsonElement>> ListarProcesosAsync(string json);
        Task<List<JsonElement>> ObtenerProcesoAsync(string json);
        Task<List<JsonElement>> CrearProcesoAsync(string json);
        Task<List<JsonElement>> CerrarProcesoAsync(string json);
        Task<List<JsonElement>> ReabrirProcesoAsync(string json);
        Task<List<JsonElement>> ListarProcesosPorAcopioAsync(string json);
        Task<List<JsonElement>> ObtenerPersonalDisponibleAsync(string json);
        Task<List<JsonElement>> ObtenerCampaniaActivaAsync(string json);
    }
}
