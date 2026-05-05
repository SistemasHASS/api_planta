using api_planta.Domain.Services;
using api_planta.Domain.Repository;
using System.Text.Json;

namespace api_planta.Infrastructure.ServiceImpl
{
    public class PaletServiceImpl : IPaletService
    {
        private readonly IPaletRepository _repository;

        public PaletServiceImpl(IPaletRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<JsonElement>> CrearPaletAsync(string json)
        {
            return await _repository.CrearPaletAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerPaletsPorProcesoAsync(string json)
        {
            return await _repository.ObtenerPaletsPorProcesoAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerPaletPorIdAsync(string json)
        {
            return await _repository.ObtenerPaletPorIdAsync(json);
        }

        public async Task<List<JsonElement>> AgregarCajasAsync(string json)
        {
            return await _repository.AgregarCajasAsync(json);
        }

        public async Task<List<JsonElement>> CerrarPaletSaldoAsync(string json)
        {
            return await _repository.CerrarPaletSaldoAsync(json);
        }

        public async Task<List<JsonElement>> ReabrirPaletAsync(string json)
        {
            return await _repository.ReabrirPaletAsync(json);
        }

        public async Task<List<JsonElement>> EliminarPaletAsync(string json)
        {
            return await _repository.EliminarPaletAsync(json);
        }

        public async Task<List<JsonElement>> EditarCajasAsync(string json)
        {
            return await _repository.EditarCajasAsync(json);
        }

        public async Task<List<JsonElement>> EliminarComposicionAsync(string json)
        {
            return await _repository.EliminarComposicionAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerDestinosPorConsignatarioAsync(string json)
        {
            return await _repository.ObtenerDestinosPorConsignatarioAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerConsignatariosPorDestinoAsync(string json)
        {
            return await _repository.ObtenerConsignatariosPorDestinoAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerFormatosAsync(string json)
        {
            return await _repository.ObtenerFormatosAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerTiposEmpaqueGuiaAsync(string json)
        {
            return await _repository.ObtenerTiposEmpaqueGuiaAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerCalibresAsync(string json)
        {
            return await _repository.ObtenerCalibresAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerPresentacionesAsync(string json)
        {
            return await _repository.ObtenerPresentacionesAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerTiposDesdeMatrizAsync(string json)
        {
            return await _repository.ObtenerTiposDesdeMatrizAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerCodigosRanchoAsync(string json)
        {
            return await _repository.ObtenerCodigosRanchoAsync(json);
        }

        public async Task<List<JsonElement>> VerificarDriscollAsync(string json)
        {
            return await _repository.VerificarDriscollAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerVariedadesPorConsignatarioAsync(string json)
        {
            return await _repository.ObtenerVariedadesPorConsignatarioAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerConfigTipoProcesoAsync(string json)
        {
            return await _repository.ObtenerConfigTipoProcesoAsync(json);
        }

        // Procesos
        public async Task<List<JsonElement>> ListarProcesosAsync(string json)
        {
            return await _repository.ListarProcesosAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerProcesoAsync(string json)
        {
            return await _repository.ObtenerProcesoAsync(json);
        }

        public async Task<List<JsonElement>> CrearProcesoAsync(string json)
        {
            return await _repository.CrearProcesoAsync(json);
        }

        public async Task<List<JsonElement>> CerrarProcesoAsync(string json)
        {
            return await _repository.CerrarProcesoAsync(json);
        }

        public async Task<List<JsonElement>> ReabrirProcesoAsync(string json)
        {
            return await _repository.ReabrirProcesoAsync(json);
        }

        public async Task<List<JsonElement>> ListarProcesosPorAcopioAsync(string json)
        {
            return await _repository.ListarProcesosPorAcopioAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerPersonalDisponibleAsync(string json)
        {
            return await _repository.ObtenerPersonalDisponibleAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerCampaniaActivaAsync(string json)
        {
            return await _repository.ObtenerCampaniaActivaAsync(json);
        }
    }
}
