USE SistemaPalets;
GO

-- Drop and recreate SP_Palet_ObtenerPorProceso with correct SET options
IF OBJECT_ID('SP_Palet_ObtenerPorProceso', 'P') IS NOT NULL DROP PROCEDURE SP_Palet_ObtenerPorProceso;
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE SP_Palet_ObtenerPorProceso
    @json NVARCHAR(MAX) = '{}'
AS BEGIN
    SET NOCOUNT ON;
    
    DECLARE @procesoId INT = 0;
    
    IF @json IS NOT NULL AND @json != '{}'
    BEGIN
        SELECT @procesoId = ISNULL(JSON_VALUE(@json, '$.procesoId'), 0);
    END
    
    SELECT (
        SELECT 
            p.Id, p.NumeroPalet, p.Estado, p.CantidadCajas, p.PesoTotal,
            p.PorcentajeAvance, p.FormatoId, p.FechaCreacion, p.FechaCierre,
            p.Observaciones, p.MedidaCorrectiva,
            f.Descripcion AS FormatoDescripcion, f.LimiteCajasPorPalet,
            a.Codigo AS AcopioCodigo, a.Nombre AS AcopioNombre,
            pr.Turno,
            (SELECT MIN(cp.FechaCreacion) FROM ComposicionPalets cp WHERE cp.PaletId = p.Id) AS PrimeraComposicionFecha
        FROM Palets p WITH (NOLOCK)
        LEFT JOIN Formatos f WITH (NOLOCK) ON p.FormatoId = f.Id
        INNER JOIN Acopios a WITH (NOLOCK) ON p.AcopioId = a.Id
        INNER JOIN Procesos pr WITH (NOLOCK) ON p.ProcesoId = pr.Id
        WHERE p.ProcesoId = @procesoId
        ORDER BY
            CASE p.Estado
                WHEN 'ABIERTO' THEN 1
                WHEN 'CERRADO_COMPLETO' THEN 2
                WHEN 'CERRADO_SALDO' THEN 2
                WHEN 'DESPACHADO' THEN 3
                ELSE 4
            END,
            p.Id DESC
        FOR JSON PATH
    ) AS JsonData;
END
GO

PRINT 'SP_Palet_ObtenerPorProceso recreated with correct SET options';
GO
