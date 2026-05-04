USE SistemaPalets;
GO

-- =============================================
-- SP_Palet_ObtenerPorId - Simplified version
-- =============================================
IF OBJECT_ID('SP_Palet_ObtenerPorId', 'P') IS NOT NULL DROP PROCEDURE SP_Palet_ObtenerPorId;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_Palet_ObtenerPorId
    @json NVARCHAR(MAX) = '{}'
AS BEGIN
    SET NOCOUNT ON;
    
    DECLARE @paletId INT = 0;
    
    IF @json IS NOT NULL AND @json != '{}'
    BEGIN
        SELECT @paletId = ISNULL(JSON_VALUE(@json, '$.id'), 0);
    END
    
    -- Get palet details
    DECLARE @paletData NVARCHAR(MAX);
    
    SELECT @paletData = (
        SELECT 
            p.Id, p.NumeroPalet, p.Estado, p.CantidadCajas, p.PesoTotal,
            p.PorcentajeAvance, p.FormatoId, p.FechaCreacion, p.FechaCierre,
            p.Observaciones, p.MedidaCorrectiva, p.ProcesoId, p.AcopioId,
            f.Descripcion AS FormatoDescripcion, f.LimiteCajasPorPalet,
            a.Codigo AS AcopioCodigo, a.Nombre AS AcopioNombre,
            pr.Turno,
            (SELECT MIN(cp.FechaCreacion) FROM ComposicionPalets cp WHERE cp.PaletId = p.Id) AS PrimeraComposicionFecha
        FROM Palets p WITH (NOLOCK)
        LEFT JOIN Formatos f WITH (NOLOCK) ON p.FormatoId = f.Id
        INNER JOIN Acopios a WITH (NOLOCK) ON p.AcopioId = a.Id
        INNER JOIN Procesos pr WITH (NOLOCK) ON p.ProcesoId = pr.Id
        WHERE p.Id = @paletId
        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
    );
    
    -- Get composiciones (simplified - just IDs and basic data)
    DECLARE @composiciones NVARCHAR(MAX);
    
    SELECT @composiciones = (
        SELECT 
            cp.Id, cp.PaletId, cp.CantidadCajas, cp.PesoPorCaja, cp.PesoTotal, cp.FechaCreacion,
            cp.TipoEmpaqueId, cp.VariedadId, cp.CalibreId, cp.TipoCajaId,
            cp.TipoClamshellId, cp.TipoEmpaqueGuiaId, cp.PresentacionId, cp.LugarProduccionId,
            cp.CodigoRanchoId, cp.TransporteId, cp.EsEnsayo, cp.VariedadGuiaId, cp.EsReposicion,
            cp.ConsignatarioId, cp.DestinoId, cp.FormatoId, cp.ClienteId
        FROM ComposicionPalets cp WITH (NOLOCK)
        WHERE cp.PaletId = @paletId
        ORDER BY cp.FechaCreacion DESC
        FOR JSON PATH
    );
    
    -- Combine palet data with composiciones
    IF @paletData IS NOT NULL
    BEGIN
        -- Parse palet data and add composiciones array
        SELECT JSON_MODIFY(@paletData, '$.Composiciones', JSON_QUERY(ISNULL(@composiciones, '[]'))) AS JsonData;
    END
    ELSE
    BEGIN
        -- Palet not found
        SELECT '{"success": false, "message": "El palet no existe"}' AS JsonData;
    END
END
GO

PRINT 'SP_Palet_ObtenerPorId created successfully';
GO
