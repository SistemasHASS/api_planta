using System.Threading.Tasks;

namespace Planta.Application.Proceso.Abstractions
{
    public interface IPdfService
    {
        Task<byte[]> GenerarFichaComposicionPaletAsync(object data, string? webRootPath = null);
    }
}
