using api_planta.Domain.UseCase;
using api_planta.Domain.Services;
using System.Text.Json;

namespace api_planta.Application.Usecase
{
    public class PaletUseCaseImpl : IPaletUseCase
    {
        private readonly IPaletService _service;

        public PaletUseCaseImpl(IPaletService service)
        {
            _service = service;
        }

        public async Task<List<JsonElement>> CrearPaletAsync(string json)
        {
            return await _service.CrearPaletAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerPaletsPorProcesoAsync(string json)
        {
            return await _service.ObtenerPaletsPorProcesoAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerPaletPorIdAsync(string json)
        {
            return await _service.ObtenerPaletPorIdAsync(json);
        }

        public async Task<List<JsonElement>> AgregarCajasAsync(string json)
        {
            return await _service.AgregarCajasAsync(json);
        }

        public async Task<List<JsonElement>> CerrarPaletSaldoAsync(string json)
        {
            return await _service.CerrarPaletSaldoAsync(json);
        }

        public async Task<List<JsonElement>> ReabrirPaletAsync(string json)
        {
            return await _service.ReabrirPaletAsync(json);
        }

        public async Task<List<JsonElement>> EliminarPaletAsync(string json)
        {
            return await _service.EliminarPaletAsync(json);
        }

        public async Task<List<JsonElement>> EditarCajasAsync(string json)
        {
            return await _service.EditarCajasAsync(json);
        }

        public async Task<List<JsonElement>> EliminarComposicionAsync(string json)
        {
            return await _service.EliminarComposicionAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerDestinosPorConsignatarioAsync(string json)
        {
            return await _service.ObtenerDestinosPorConsignatarioAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerConsignatariosPorDestinoAsync(string json)
        {
            return await _service.ObtenerConsignatariosPorDestinoAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerFormatosAsync(string json)
        {
            return await _service.ObtenerFormatosAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerTiposEmpaqueGuiaAsync(string json)
        {
            return await _service.ObtenerTiposEmpaqueGuiaAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerCalibresAsync(string json)
        {
            return await _service.ObtenerCalibresAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerPresentacionesAsync(string json)
        {
            return await _service.ObtenerPresentacionesAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerTiposDesdeMatrizAsync(string json)
        {
            return await _service.ObtenerTiposDesdeMatrizAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerCodigosRanchoAsync(string json)
        {
            return await _service.ObtenerCodigosRanchoAsync(json);
        }

        public async Task<List<JsonElement>> VerificarDriscollAsync(string json)
        {
            return await _service.VerificarDriscollAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerVariedadesPorConsignatarioAsync(string json)
        {
            return await _service.ObtenerVariedadesPorConsignatarioAsync(json);
        }


        public async Task<List<JsonElement>> ObtenerConfigTipoProcesoAsync(string json)
        {
            return await _service.ObtenerConfigTipoProcesoAsync(json);
        }

        // Procesos
        public async Task<List<JsonElement>> ListarProcesosAsync(string json)
        {
            return await _service.ListarProcesosAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerProcesoAsync(string json)
        {
            return await _service.ObtenerProcesoAsync(json);
        }

        public async Task<List<JsonElement>> CrearProcesoAsync(string json)
        {
            return await _service.CrearProcesoAsync(json);
        }

        public async Task<List<JsonElement>> CerrarProcesoAsync(string json)
        {
            return await _service.CerrarProcesoAsync(json);
        }

        public async Task<List<JsonElement>> ReabrirProcesoAsync(string json)
        {
            return await _service.ReabrirProcesoAsync(json);
        }

        public async Task<List<JsonElement>> ListarProcesosPorAcopioAsync(string json)
        {
            return await _service.ListarProcesosPorAcopioAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerPersonalDisponibleAsync(string json)
        {
            return await _service.ObtenerPersonalDisponibleAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerCampaniaActivaAsync(string json)
        {
            return await _service.ObtenerCampaniaActivaAsync(json);
        }
    }
}
