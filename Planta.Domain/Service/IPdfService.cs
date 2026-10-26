using Planta.Domain.Dtos;

namespace Planta.Domain.Service;

public interface IPdfService
{
    /// <summary>Genera el PDF de la ficha de composicion de un palet. <paramref name="webRootPath"/> es donde estan los logos (assets/logos/{ruc}.jpg).</summary>
    byte[] GenerarFichaComposicionPalet(FichaComposicionPalet ficha, string? webRootPath);
}
