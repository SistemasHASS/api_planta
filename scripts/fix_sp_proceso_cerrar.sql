USE SistemaPalets;
GO

-- Drop and recreate SP_Proceso_Cerrar with correct SET options
IF OBJECT_ID('SP_Proceso_Cerrar', 'P') IS NOT NULL DROP PROCEDURE SP_Proceso_Cerrar;
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE SP_Proceso_Cerrar
    @json NVARCHAR(MAX) = '{}'
AS BEGIN
    SET NOCOUNT ON;
    
    DECLARE @id INT = 0, @usuarioId INT = 0;
    
    IF @json IS NOT NULL AND @json != '{}'
    BEGIN
        SELECT 
            @id = ISNULL(JSON_VALUE(@json, '$.id'), 0),
            @usuarioId = ISNULL(JSON_VALUE(@json, '$.usuarioId'), 0);
    END
    
    -- Validate proceso exists and is open
    IF NOT EXISTS (SELECT 1 FROM Procesos WHERE Id = @id AND Estado = 'ABIERTO')
    BEGIN
        SELECT '{"success": false, "message": "El proceso no existe o ya esta cerrado"}' AS JsonData;
        RETURN;
    END
    
    -- Update proceso to CERRADO
    UPDATE Procesos
    SET Estado = 'CERRADO', 
        FechaCierre = GETDATE(), 
        UsuarioCierreId = @usuarioId
    WHERE Id = @id;
    
    SELECT '{"success": true, "message": "Proceso cerrado exitosamente"}' AS JsonData;
END
GO

PRINT 'SP_Proceso_Cerrar recreated with correct SET options';
GO
