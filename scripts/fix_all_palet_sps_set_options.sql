USE SistemaPalets;
GO

-- =============================================
-- Fix all Palet SPs created in this session with proper SET options
-- =============================================

-- =============================================
-- SP_Palet_ObtenerPorId
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
    
    -- Get palet details + composiciones
    SELECT (
        SELECT 
            p.Id, p.NumeroPalet, p.Estado, p.CantidadCajas, p.PesoTotal,
            p.PorcentajeAvance, p.FormatoId, p.FechaCreacion, p.FechaCierre,
            p.Observaciones, p.MedidaCorrectiva,
            f.Descripcion AS FormatoDescripcion, f.LimiteCajasPorPalet,
            a.Codigo AS AcopioCodigo, a.Nombre AS AcopioNombre,
            pr.Turno,
            (SELECT MIN(cp.FechaCreacion) FROM ComposicionPalets cp WHERE cp.PaletId = p.Id) AS PrimeraComposicionFecha,
            (
                SELECT 
                    cp.Id, cp.PaletId, cp.CantidadCajas, cp.PesoNeto, cp.FechaCreacion,
                    cp.TipoEmpaqueId, cp.VariedadId, cp.CalibreId, cp.CategoriaId, cp.TipoCajaId,
                    cp.TipoClamshellId, cp.TipoEmpaqueGuiaId, cp.PresentacionId, cp.LugarProduccionId,
                    cp.CodigoRanchoId, cp.TransporteId, cp.EsEnsayo, cp.VariedadGuia, cp.EsReposicion,
                    te.Nombre AS TipoEmpaqueNombre, v.Nombre AS VariedadNombre, c.Nombre AS CalibreNombre,
                    cat.Nombre AS CategoriaNombre, tc.Nombre AS TipoCajaNombre, tcl.Nombre AS TipoClamshellNombre,
                    teg.Nombre AS TipoEmpaqueGuiaNombre, pres.Nombre AS PresentacionNombre,
                    lp.Nombre AS LugarProduccionNombre, cr.Nombre AS CodigoRanchoNombre, tr.Nombre AS TransporteNombre
                FROM ComposicionPalets cp WITH (NOLOCK)
                LEFT JOIN TiposEmpaque te WITH (NOLOCK) ON cp.TipoEmpaqueId = te.Id
                LEFT JOIN Variedades v WITH (NOLOCK) ON cp.VariedadId = v.Id
                LEFT JOIN Calibres c WITH (NOLOCK) ON cp.CalibreId = c.Id
                LEFT JOIN Categorias cat WITH (NOLOCK) ON cp.CategoriaId = cat.Id
                LEFT JOIN TiposCaja tc WITH (NOLOCK) ON cp.TipoCajaId = tc.Id
                LEFT JOIN TiposClamshell tcl WITH (NOLOCK) ON cp.TipoClamshellId = tcl.Id
                LEFT JOIN TiposEmpaqueGuia teg WITH (NOLOCK) ON cp.TipoEmpaqueGuiaId = teg.Id
                LEFT JOIN Presentacion pres WITH (NOLOCK) ON cp.PresentacionId = pres.Id
                LEFT JOIN LugaresProduccion lp WITH (NOLOCK) ON cp.LugarProduccionId = lp.Id
                LEFT JOIN CodigosRancho cr WITH (NOLOCK) ON cp.CodigoRanchoId = cr.Id
                LEFT JOIN Transporte tr WITH (NOLOCK) ON cp.TransporteId = tr.Id
                WHERE cp.PaletId = @paletId
                ORDER BY cp.FechaCreacion DESC
                FOR JSON PATH
            ) AS Composiciones
        FROM Palets p WITH (NOLOCK)
        LEFT JOIN Formatos f WITH (NOLOCK) ON p.FormatoId = f.Id
        INNER JOIN Acopios a WITH (NOLOCK) ON p.AcopioId = a.Id
        INNER JOIN Procesos pr WITH (NOLOCK) ON p.ProcesoId = pr.Id
        WHERE p.Id = @paletId
        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
    ) AS JsonData;
END
GO

-- =============================================
-- SP_Palet_AgregarCajas
-- =============================================
IF OBJECT_ID('SP_Palet_AgregarCajas', 'P') IS NOT NULL DROP PROCEDURE SP_Palet_AgregarCajas;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_Palet_AgregarCajas
    @json NVARCHAR(MAX) = '{}'
AS BEGIN
    SET NOCOUNT ON;
    
    DECLARE @paletId INT = 0, @cantidadCajas INT = 0, @pesoNeto DECIMAL(10,2) = 0, @tipoEmpaqueId INT = 0,
            @variedadId INT = 0, @calibreId INT = 0, @categoriaId INT = 0, @tipoCajaId INT = 0,
            @tipoClamshellId INT = 0, @tipoEmpaqueGuiaId INT = 0, @presentacionId INT = 0,
            @lugarProduccionId INT = 0, @codigoRanchoId INT = 0, @transporteId INT = 0,
            @esEnsayo BIT = 0, @variedadGuia VARCHAR(50) = '', @esReposicion BIT = 0, @usuarioId INT = 0;
    
    IF @json IS NOT NULL AND @json != '{}'
    BEGIN
        SELECT 
            @paletId = ISNULL(JSON_VALUE(@json, '$.paletId'), 0),
            @cantidadCajas = ISNULL(JSON_VALUE(@json, '$.cantidadCajas'), 0),
            @pesoNeto = ISNULL(JSON_VALUE(@json, '$.pesoNeto'), 0),
            @tipoEmpaqueId = ISNULL(JSON_VALUE(@json, '$.tipoEmpaqueId'), 0),
            @variedadId = ISNULL(JSON_VALUE(@json, '$.variedadId'), 0),
            @calibreId = ISNULL(JSON_VALUE(@json, '$.calibreId'), 0),
            @categoriaId = ISNULL(JSON_VALUE(@json, '$.categoriaId'), 0),
            @tipoCajaId = ISNULL(JSON_VALUE(@json, '$.tipoCajaId'), 0),
            @tipoClamshellId = ISNULL(JSON_VALUE(@json, '$.tipoClamshellId'), 0),
            @tipoEmpaqueGuiaId = ISNULL(JSON_VALUE(@json, '$.tipoEmpaqueGuiaId'), 0),
            @presentacionId = ISNULL(JSON_VALUE(@json, '$.presentacionId'), 0),
            @lugarProduccionId = ISNULL(JSON_VALUE(@json, '$.lugarProduccionId'), 0),
            @codigoRanchoId = ISNULL(JSON_VALUE(@json, '$.codigoRanchoId'), 0),
            @transporteId = ISNULL(JSON_VALUE(@json, '$.transporteId'), 0),
            @esEnsayo = ISNULL(JSON_VALUE(@json, '$.esEnsayo'), 0),
            @variedadGuia = ISNULL(JSON_VALUE(@json, '$.variedadGuia'), ''),
            @esReposicion = ISNULL(JSON_VALUE(@json, '$.esReposicion'), 0),
            @usuarioId = ISNULL(JSON_VALUE(@json, '$.usuarioId'), 0);
    END
    
    -- Validate palet exists and is open
    IF NOT EXISTS (SELECT 1 FROM Palets WHERE Id = @paletId AND Estado = 'ABIERTO')
    BEGIN
        SELECT '{"success": false, "message": "El palet no existe o no esta abierto"}' AS JsonData;
        RETURN;
    END
    
    -- Insert composicion
    INSERT INTO ComposicionPalets (
        PaletId, CantidadCajas, PesoNeto, FechaCreacion, TipoEmpaqueId, VariedadId, CalibreId,
        CategoriaId, TipoCajaId, TipoClamshellId, TipoEmpaqueGuiaId, PresentacionId,
        LugarProduccionId, CodigoRanchoId, TransporteId, EsEnsayo, VariedadGuia, EsReposicion
    )
    VALUES (
        @paletId, @cantidadCajas, @pesoNeto, GETDATE(), @tipoEmpaqueId, @variedadId, @calibreId,
        @categoriaId, @tipoCajaId, @tipoClamshellId, @tipoEmpaqueGuiaId, @presentacionId,
        @lugarProduccionId, @codigoRanchoId, @transporteId, @esEnsayo, @variedadGuia, @esReposicion
    );
    
    -- Update palet totals
    UPDATE Palets
    SET CantidadCajas = (SELECT SUM(CantidadCajas) FROM ComposicionPalets WHERE PaletId = @paletId),
        PesoTotal = (SELECT SUM(PesoNeto) FROM ComposicionPalets WHERE PaletId = @paletId)
    WHERE Id = @paletId;
    
    SELECT '{"success": true, "message": "Cajas agregadas exitosamente"}' AS JsonData;
END
GO

-- =============================================
-- SP_Palet_EditarCajas
-- =============================================
IF OBJECT_ID('SP_Palet_EditarCajas', 'P') IS NOT NULL DROP PROCEDURE SP_Palet_EditarCajas;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SP_Palet_EditarCajas
    @json NVARCHAR(MAX) = '{}'
AS BEGIN
    SET NOCOUNT ON;
    
    DECLARE @composicionId INT = 0, @cantidadCajas INT = 0, @pesoNeto DECIMAL(10,2) = 0, @usuarioId INT = 0;
    
    IF @json IS NOT NULL AND @json != '{}'
    BEGIN
        SELECT 
            @composicionId = ISNULL(JSON_VALUE(@json, '$.composicionId'), 0),
            @cantidadCajas = ISNULL(JSON_VALUE(@json, '$.cantidadCajas'), 0),
            @pesoNeto = ISNULL(JSON_VALUE(@json, '$.pesoNeto'), 0),
            @usuarioId = ISNULL(JSON_VALUE(@json, '$.usuarioId'), 0);
    END
    
    -- Get paletId from composicion
    DECLARE @paletId INT;
    SELECT @paletId = PaletId FROM ComposicionPalets WHERE Id = @composicionId;
    
    IF @paletId IS NULL
    BEGIN
        SELECT '{"success": false, "message": "La composicion no existe"}' AS JsonData;
        RETURN;
    END
    
    -- Validate palet is open
    IF NOT EXISTS (SELECT 1 FROM Palets WHERE Id = @paletId AND Estado = 'ABIERTO')
    BEGIN
        SELECT '{"success": false, "message": "El palet no esta abierto"}' AS JsonData;
        RETURN;
    END
    
    -- Update composicion
    UPDATE ComposicionPalets
    SET CantidadCajas = @cantidadCajas, PesoNeto = @pesoNeto
    WHERE Id = @composicionId;
    
    -- Update palet totals
    UPDATE Palets
    SET CantidadCajas = (SELECT SUM(CantidadCajas) FROM ComposicionPalets WHERE PaletId = @paletId),
        PesoTotal = (SELECT SUM(PesoNeto) FROM ComposicionPalets WHERE PaletId = @paletId)
    WHERE Id = @paletId;
    
    SELECT '{"success": true, "message": "Cajas editadas exitosamente"}' AS JsonData;
END
GO

PRINT 'All Palet SPs recreated with correct SET options';
GO
