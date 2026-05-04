USE SistemaPalets;
GO

-- Fix all SPs to return empty array instead of NULL when no results

-- =============================================
-- SP_Proceso_Listar
-- =============================================
IF OBJECT_ID('SP_Proceso_Listar', 'P') IS NOT NULL DROP PROCEDURE SP_Proceso_Listar;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_Proceso_Listar
    @json NVARCHAR(MAX) = '{}'
AS BEGIN
    SET NOCOUNT ON;
    
    DECLARE @estado VARCHAR(20) = NULL, @acopioId INT = NULL;
    
    IF @json IS NOT NULL AND @json != '{}'
    BEGIN
        SELECT 
            @estado = JSON_VALUE(@json, '$.estado'),
            @acopioId = JSON_VALUE(@json, '$.acopioId');
    END
    
    DECLARE @result NVARCHAR(MAX);
    
    SELECT @result = (
        SELECT 
            p.Id, p.FechaProceso, p.Turno, p.Estado, p.AcopioId, p.UsuarioAperturaId,
            p.FechaApertura, p.FechaCierre, p.UsuarioCierreId, p.CampaniaId,
            a.Codigo AS AcopioCodigo, a.Nombre AS AcopioNombre,
            u.NombreCompleto AS UsuarioAperturaNombre,
            uc.NombreCompleto AS UsuarioCierreNombre,
            c.Nombre AS CampaniaNombre
        FROM Procesos p WITH (NOLOCK)
        INNER JOIN Acopios a WITH (NOLOCK) ON p.AcopioId = a.Id
        LEFT JOIN Usuarios u WITH (NOLOCK) ON p.UsuarioAperturaId = u.Id
        LEFT JOIN Usuarios uc WITH (NOLOCK) ON p.UsuarioCierreId = uc.Id
        LEFT JOIN Campanias c WITH (NOLOCK) ON p.CampaniaId = c.Id
        WHERE (@estado IS NULL OR @estado = 'TODOS' OR p.Estado = @estado)
          AND (@acopioId IS NULL OR p.AcopioId = @acopioId)
        ORDER BY p.FechaProceso DESC, p.Id DESC
        FOR JSON PATH
    );
    
    SELECT ISNULL(@result, '[]') AS JsonData;
END
GO

PRINT 'All SPs fixed to return empty array instead of NULL';
GO
