using System.Text.Json;

namespace Planta.Infrastructure.ServiceImpl;

/// <summary>Lee la respuesta de PLANTA_GetParametro para el parametro Flujo_Empaque (la misma regla que usa Palets).</summary>
internal static class FlujoEmpaqueParametro
{
    /// <summary>Si no se pudo leer el parametro devuelve su sobre de error (no se guarda ni se muestra nada); si no, si esta activo (valor SI).</summary>
    public static (bool Activo, JsonElement? Error) Leer(JsonElement parametro)
    {
        if (parametro.ValueKind == JsonValueKind.Object && parametro.TryGetProperty("error", out var error) && error.ValueKind == JsonValueKind.True)
        {
            return (false, parametro);
        }

        if (parametro.ValueKind != JsonValueKind.Object || !parametro.TryGetProperty("data", out var data) || data.ValueKind != JsonValueKind.Array)
        {
            return (false, null);
        }

        foreach (var fila in data.EnumerateArray())
        {
            var valor = fila.TryGetProperty("valor", out var v) && v.ValueKind == JsonValueKind.String ? v.GetString()?.Trim() : null;
            var activo = !fila.TryGetProperty("activo", out var a) || a.ValueKind is JsonValueKind.True or JsonValueKind.Null
                         || (a.ValueKind == JsonValueKind.Number && a.GetInt32() != 0);
            if (activo && string.Equals(valor, "SI", StringComparison.OrdinalIgnoreCase))
            {
                return (true, null);
            }
        }

        return (false, null);
    }
}
