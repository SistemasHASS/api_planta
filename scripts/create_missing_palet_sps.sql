USE SistemaPalets;
GO

-- =============================================
-- Create missing Palet SPs with correct SET options and empty array handling
-- =============================================

-- =============================================
-- SP_Palet_ObtenerPorId - Get palet details with composiciones
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
    
    -- Get composiciones
    DECLARE @composiciones NVARCHAR(MAX);
    
    SELECT @composiciones = (
        SELECT 
            cp.Id, cp.PaletId, cp.CantidadCajas, cp.PesoPorCaja, cp.PesoTotal, cp.FechaCreacion,
            cp.TipoEmpaqueId, cp.VariedadId, cp.CalibreId, cp.TipoCajaId,
            cp.TipoClamshellId, cp.TipoEmpaqueGuiaId, cp.PresentacionId, cp.LugarProduccionId,
            cp.CodigoRanchoId, cp.TransporteId, cp.EsEnsayo, cp.VariedadGuiaId, cp.EsReposicion,
            cp.ConsignatarioId, cp.DestinoId, cp.FormatoId, cp.ClienteId,
            te.Nombre AS TipoEmpaqueNombre, 
            v.Nombre AS VariedadNombre, 
            c.Nombre AS CalibreNombre,
            tc.Nombre AS TipoCajaNombre, 
            tcl.Nombre AS TipoClamshellNombre,
            teg.Nombre AS TipoEmpaqueGuiaNombre, 
            pres.Nombre AS PresentacionNombre,
            lp.Nombre AS LugarProduccionNombre, 
            cr.Codigo AS CodigoRanchoCodigo,
            tr.Nombre AS TransporteNombre,
            vg.Nombre AS VariedadGuiaNombre,
            cons.Nombre AS ConsignatarioNombre,
            d.Nombre AS DestinoNombre,
            f.Descripcion AS FormatoDescripcion,
            cl.Nombre AS ClienteNombre
        FROM ComposicionPalets cp WITH (NOLOCK)
        LEFT JOIN TiposEmpaque te WITH (NOLOCK) ON cp.TipoEmpaqueId = te.Id
        LEFT JOIN Variedades v WITH (NOLOCK) ON cp.VariedadId = v.Id
        LEFT JOIN Calibres c WITH (NOLOCK) ON cp.CalibreId = c.Id
        LEFT JOIN TiposCaja tc WITH (NOLOCK) ON cp.TipoCajaId = tc.Id
        LEFT JOIN TiposClamshell tcl WITH (NOLOCK) ON cp.TipoClamshellId = tcl.Id
        LEFT JOIN TiposEmpaqueGuia teg WITH (NOLOCK) ON cp.TipoEmpaqueGuiaId = teg.Id
        LEFT JOIN Presentacion pres WITH (NOLOCK) ON cp.PresentacionId = pres.Id
        LEFT JOIN LugaresProduccion lp WITH (NOLOCK) ON cp.LugarProduccionId = lp.Id
        LEFT JOIN CodigosRancho cr WITH (NOLOCK) ON cp.CodigoRanchoId = cr.Id
        LEFT JOIN Transporte tr WITH (NOLOCK) ON cp.TransporteId = tr.Id
        LEFT JOIN Variedades vg WITH (NOLOCK) ON cp.VariedadGuiaId = vg.Id
        LEFT JOIN Consignatarios cons WITH (NOLOCK) ON cp.ConsignatarioId = cons.Id
        LEFT JOIN Destinos d WITH (NOLOCK) ON cp.DestinoId = d.Id
        LEFT JOIN Formatos f WITH (NOLOCK) ON cp.FormatoId = f.Id
        LEFT JOIN Clientes cl WITH (NOLOCK) ON cp.ClienteId = cl.Id
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

-- =============================================
-- SP_Palet_Crear - Create new palet
-- =============================================
IF OBJECT_ID('SP_Palet_Crear', 'P') IS NOT NULL DROP PROCEDURE SP_Palet_Crear;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_Palet_Crear
    @json NVARCHAR(MAX) = '{}'
AS BEGIN
    SET NOCOUNT ON;
    
    DECLARE @procesoId INT = 0, @acopioId INT = 0, @usuarioId INT = 0;
    
    IF @json IS NOT NULL AND @json != '{}'
    BEGIN
        SELECT 
            @procesoId = ISNULL(JSON_VALUE(@json, '$.procesoId'), 0),
            @acopioId = ISNULL(JSON_VALUE(@json, '$.acopioId'), 0),
            @usuarioId = ISNULL(JSON_VALUE(@json, '$.usuarioId'), 0);
    END
    
    -- Validate proceso exists and is open
    IF NOT EXISTS (SELECT 1 FROM Procesos WHERE Id = @procesoId AND Estado = 'ABIERTO')
    BEGIN
        SELECT '{"success": false, "message": "El proceso no existe o no esta abierto"}' AS JsonData;
        RETURN;
    END
    
    -- Get next palet number for this proceso
    DECLARE @numeroPalet INT;
    SELECT @numeroPalet = ISNULL(MAX(NumeroPalet), 0) + 1 FROM Palets WHERE ProcesoId = @procesoId;
    
    -- Insert new palet
    INSERT INTO Palets (
        NumeroPalet, ProcesoId, AcopioId, Estado, CantidadCajas, PesoTotal, 
        PorcentajeAvance, FechaCreacion, UsuarioCreacionId
    )
    VALUES (
        @numeroPalet, @procesoId, @acopioId, 'ABIERTO', 0, 0, 
        0, GETDATE(), @usuarioId
    );
    
    DECLARE @newId INT = SCOPE_IDENTITY();
    
    -- Return created palet
    SELECT (
        SELECT 
            p.Id, p.NumeroPalet, p.Estado, p.CantidadCajas, p.PesoTotal,
            p.PorcentajeAvance, p.FormatoId, p.FechaCreacion,
            a.Codigo AS AcopioCodigo, a.Nombre AS AcopioNombre,
            pr.Turno
        FROM Palets p
        INNER JOIN Acopios a ON p.AcopioId = a.Id
        INNER JOIN Procesos pr ON p.ProcesoId = pr.Id
        WHERE p.Id = @newId
        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
    ) AS JsonData;
END
GO

PRINT 'Missing Palet SPs created successfully';
GO
