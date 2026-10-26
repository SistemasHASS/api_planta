namespace Planta.Domain.Dtos;

/// <summary>Archivo que genera la API (por ejemplo un PDF) listo para devolverse al navegador.</summary>
public sealed record ArchivoGenerado(byte[] Contenido, string TipoContenido, string Nombre);
