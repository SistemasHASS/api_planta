USE SistemaPalets;
GO

-- =============================================
-- Fix all SPs created in this session with proper SET options
-- =============================================

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
    
    SELECT (
        SELECT 
            p.Id, p.FechaProceso, p.Turno, p.Estado, p.AcopioId, p.UsuarioCreacionId,
            p.FechaCreacion, p.FechaCierre, p.UsuarioCierreId, p.CampaniaId,
            a.Codigo AS AcopioCodigo, a.Nombre AS AcopioNombre,
            u.Nombre AS UsuarioCreacionNombre,
            uc.Nombre AS UsuarioCierreNombre,
            c.Nombre AS CampaniaNombre
        FROM Procesos p WITH (NOLOCK)
        INNER JOIN Acopios a WITH (NOLOCK) ON p.AcopioId = a.Id
        LEFT JOIN Usuarios u WITH (NOLOCK) ON p.UsuarioCreacionId = u.Id
        LEFT JOIN Usuarios uc WITH (NOLOCK) ON p.UsuarioCierreId = uc.Id
        LEFT JOIN Campanias c WITH (NOLOCK) ON p.CampaniaId = c.Id
        WHERE (@estado IS NULL OR @estado = 'TODOS' OR p.Estado = @estado)
          AND (@acopioId IS NULL OR p.AcopioId = @acopioId)
        ORDER BY p.FechaProceso DESC, p.Id DESC
        FOR JSON PATH
    ) AS JsonData;
END
GO

-- =============================================
-- SP_Proceso_ObtenerPorId
-- =============================================
IF OBJECT_ID('SP_Proceso_ObtenerPorId', 'P') IS NOT NULL DROP PROCEDURE SP_Proceso_ObtenerPorId;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_Proceso_ObtenerPorId
    @json NVARCHAR(MAX) = '{}'
AS BEGIN
    SET NOCOUNT ON;
    
    DECLARE @id INT = 0;
    
    IF @json IS NOT NULL AND @json != '{}'
    BEGIN
        SELECT @id = ISNULL(JSON_VALUE(@json, '$.id'), 0);
    END
    
    SELECT (
        SELECT 
            p.Id, p.FechaProceso, p.Turno, p.Estado, p.AcopioId, p.UsuarioCreacionId,
            p.FechaCreacion, p.FechaCierre, p.UsuarioCierreId, p.CampaniaId,
            a.Codigo AS AcopioCodigo, a.Nombre AS AcopioNombre,
            u.Nombre AS UsuarioCreacionNombre,
            uc.Nombre AS UsuarioCierreNombre,
            c.Nombre AS CampaniaNombre
        FROM Procesos p WITH (NOLOCK)
        INNER JOIN Acopios a WITH (NOLOCK) ON p.AcopioId = a.Id
        LEFT JOIN Usuarios u WITH (NOLOCK) ON p.UsuarioCreacionId = u.Id
        LEFT JOIN Usuarios uc WITH (NOLOCK) ON p.UsuarioCierreId = uc.Id
        LEFT JOIN Campanias c WITH (NOLOCK) ON p.CampaniaId = c.Id
        WHERE p.Id = @id
        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
    ) AS JsonData;
END
GO

-- =============================================
-- SP_Proceso_Crear
-- =============================================
IF OBJECT_ID('SP_Proceso_Crear', 'P') IS NOT NULL DROP PROCEDURE SP_Proceso_Crear;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_Proceso_Crear
    @json NVARCHAR(MAX) = '{}'
AS BEGIN
    SET NOCOUNT ON;
    
    DECLARE @fechaProceso DATE, @turno VARCHAR(20), @acopioId INT, @usuarioId INT, @campaniaId INT = NULL;
    
    IF @json IS NOT NULL AND @json != '{}'
    BEGIN
        SELECT 
            @fechaProceso = JSON_VALUE(@json, '$.fechaProceso'),
            @turno = JSON_VALUE(@json, '$.turno'),
            @acopioId = JSON_VALUE(@json, '$.acopioId'),
            @usuarioId = JSON_VALUE(@json, '$.usuarioId'),
            @campaniaId = JSON_VALUE(@json, '$.campaniaId');
    END
    
    -- Validate no duplicate open proceso
    IF EXISTS (SELECT 1 FROM Procesos WHERE AcopioId = @acopioId AND FechaProceso = @fechaProceso AND Turno = @turno AND Estado = 'ABIERTO')
    BEGIN
        SELECT '{"success": false, "message": "Ya existe un proceso abierto para este acopio, fecha y turno"}' AS JsonData;
        RETURN;
    END
    
    INSERT INTO Procesos (FechaProceso, Turno, Estado, AcopioId, UsuarioCreacionId, FechaCreacion, CampaniaId)
    VALUES (@fechaProceso, @turno, 'ABIERTO', @acopioId, @usuarioId, GETDATE(), @campaniaId);
    
    DECLARE @newId INT = SCOPE_IDENTITY();
    
    SELECT (
        SELECT 
            p.Id, p.FechaProceso, p.Turno, p.Estado, p.AcopioId, p.UsuarioCreacionId,
            p.FechaCreacion, a.Codigo AS AcopioCodigo, a.Nombre AS AcopioNombre
        FROM Procesos p
        INNER JOIN Acopios a ON p.AcopioId = a.Id
        WHERE p.Id = @newId
        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
    ) AS JsonData;
END
GO

-- =============================================
-- SP_Proceso_Reabrir
-- =============================================
IF OBJECT_ID('SP_Proceso_Reabrir', 'P') IS NOT NULL DROP PROCEDURE SP_Proceso_Reabrir;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_Proceso_Reabrir
    @json NVARCHAR(MAX) = '{}'
AS BEGIN
    SET NOCOUNT ON;
    
    DECLARE @id INT = 0;
    
    IF @json IS NOT NULL AND @json != '{}'
    BEGIN
        SELECT @id = ISNULL(JSON_VALUE(@json, '$.id'), 0);
    END
    
    -- Validate proceso exists and is closed
    IF NOT EXISTS (SELECT 1 FROM Procesos WHERE Id = @id AND Estado = 'CERRADO')
    BEGIN
        SELECT '{"success": false, "message": "El proceso no existe o no esta cerrado"}' AS JsonData;
        RETURN;
    END
    
    -- Update proceso to ABIERTO
    UPDATE Procesos
    SET Estado = 'ABIERTO', 
        FechaCierre = NULL, 
        UsuarioCierreId = NULL
    WHERE Id = @id;
    
    SELECT '{"success": true, "message": "Proceso reabierto exitosamente"}' AS JsonData;
END
GO

PRINT 'All Proceso SPs recreated with correct SET options';
GO
