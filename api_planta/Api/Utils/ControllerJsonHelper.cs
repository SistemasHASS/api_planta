using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace api_planta.Api.Utils
{
    public static class ControllerJsonHelper
    {
        /// <summary>
        /// Helper: Extrae el JSON string del body del request
        /// </summary>
        public static string ExtractJson(JsonElement? body)
        {
            return body.HasValue && body.Value.ValueKind != JsonValueKind.Null
                ? body.Value.ToString()
                : "{}";
        }

        /// <summary>
        /// Helper: los SPs retornan 1 fila con columna JsonData (string JSON).
        /// BaseRepository la parsea a JsonElement. Esta función desempaqueta:
        ///   - Si es un array JSON → lo retorna tal cual
        ///   - Si es un string JSON (ej. "[{...}]") → lo parsea
        ///   - Si tiene propiedad JsonData → extrae y parsea
        ///   - Si es un objeto → lo retorna como objeto único
        /// </summary>
        public static IActionResult UnwrapSpResult(
            ControllerBase controller,
            List<JsonElement> resultado,
            ILogger logger,
            string endpoint)
        {
            if (resultado == null || !resultado.Any())
                return controller.Ok(new object[0]);

            var first = resultado.First();

            // Caso 1: El SP retornó un array JSON directamente
            if (first.ValueKind == JsonValueKind.Array)
                return controller.Ok(first);

            // Caso 2: El SP retornó un string (ej. el JSON como texto plano)
            if (first.ValueKind == JsonValueKind.String)
            {
                var jsonStr = first.GetString();
                if (string.IsNullOrEmpty(jsonStr) || jsonStr == "null")
                    return controller.Ok(new object[0]);

                try
                {
                    var parsed = JsonSerializer.Deserialize<JsonElement>(jsonStr);
                    return controller.Ok(parsed);
                }
                catch
                {
                    logger.LogWarning("[{Endpoint}] Could not parse string result as JSON", endpoint);
                    return controller.Ok(new object[0]);
                }
            }

            // Caso 3: Objeto con propiedad JsonData
            if (first.ValueKind == JsonValueKind.Object && first.TryGetProperty("JsonData", out var jsonData))
            {
                if (jsonData.ValueKind == JsonValueKind.Null)
                    return controller.Ok(new object[0]);

                if (jsonData.ValueKind == JsonValueKind.String)
                {
                    var str = jsonData.GetString();
                    if (string.IsNullOrEmpty(str) || str == "null")
                        return controller.Ok(new object[0]);

                    try
                    {
                        var parsed = JsonSerializer.Deserialize<JsonElement>(str);
                        return controller.Ok(parsed);
                    }
                    catch
                    {
                        logger.LogWarning("[{Endpoint}] Could not parse JsonData string", endpoint);
                        return controller.Ok(new object[0]);
                    }
                }

                return controller.Ok(jsonData);
            }

            // Caso 4: Objeto sin JsonData → retornar tal cual (ej. resultado de verificarDriscoll)
            return controller.Ok(first);
        }
    }
}
