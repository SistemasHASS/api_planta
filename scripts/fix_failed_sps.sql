USE SistemaPalets;
GO

-- =============================================
-- FIX 1: SP_Palet_AgregarCajas - remove CategoriaId (column doesn't exist in ComposicionPalets)
-- =============================================
IF OBJECT_ID('SP_Palet_AgregarCajas', 'P') IS NOT NULL DROP PROCEDURE SP_Palet_AgregarCajas;
GO
CREATE PROCEDURE SP_Palet_AgregarCajas
    @json NVARCHAR(MAX) = '{}'
AS BEGIN
    SET NOCOUNT ON;
    
    DECLARE @paletId INT, @consignatarioId INT, @destinoId INT, @formatoId INT,
            @tipoEmpaqueId INT, @calibreId INT, @variedadId INT,
            @lugarProduccionId INT, @codigoRanchoId INT, @transporteId INT,
            @cantidadCajas INT, @esReposicion BIT = 0, @usuarioId INT,
            @tipoEmpaqueGuiaId INT, @presentacionId INT, @tipoCajaId INT,
            @tipoClamshellId INT, @tipoProcesoEmpacadoId INT,
            @esEnsayo BIT = 0, @variedadGuiaId INT, @clienteId INT;
    
    IF @json IS NOT NULL AND @json != '{}'
    BEGIN
        SELECT 
            @paletId = JSON_VALUE(@json, '$.paletId'),
            @consignatarioId = JSON_VALUE(@json, '$.consignatarioId'),
            @destinoId = JSON_VALUE(@json, '$.destinoId'),
            @formatoId = JSON_VALUE(@json, '$.formatoId'),
            @tipoEmpaqueId = JSON_VALUE(@json, '$.tipoEmpaqueId'),
            @calibreId = JSON_VALUE(@json, '$.calibreId'),
            @variedadId = JSON_VALUE(@json, '$.variedadId'),
            @lugarProduccionId = JSON_VALUE(@json, '$.lugarProduccionId'),
            @codigoRanchoId = JSON_VALUE(@json, '$.codigoRanchoId'),
            @transporteId = JSON_VALUE(@json, '$.transporteId'),
            @cantidadCajas = JSON_VALUE(@json, '$.cantidadCajas'),
            @esReposicion = ISNULL(JSON_VALUE(@json, '$.esReposicion'), 0),
            @usuarioId = JSON_VALUE(@json, '$.usuarioId'),
            @tipoEmpaqueGuiaId = JSON_VALUE(@json, '$.tipoEmpaqueGuiaId'),
            @presentacionId = JSON_VALUE(@json, '$.presentacionId'),
            @tipoCajaId = JSON_VALUE(@json, '$.tipoCajaId'),
            @tipoClamshellId = JSON_VALUE(@json, '$.tipoClamshellId'),
            @tipoProcesoEmpacadoId = JSON_VALUE(@json, '$.tipoProcesoEmpacadoId'),
            @esEnsayo = ISNULL(JSON_VALUE(@json, '$.esEnsayo'), 0),
            @variedadGuiaId = JSON_VALUE(@json, '$.variedadGuiaId'),
            @clienteId = JSON_VALUE(@json, '$.clienteId');
    END
    
    -- Default clienteId = consignatarioId if not provided
    IF @clienteId IS NULL SET @clienteId = @consignatarioId;
    
    -- Validar palet existe y esta abierto
    IF NOT EXISTS (SELECT 1 FROM Palets WHERE Id = @paletId AND Estado = 'ABIERTO')
    BEGIN
        SELECT '{"success": false, "message": "El palet no existe o no esta abierto"}' AS JsonData;
        RETURN;
    END
    
    -- Obtener peso por caja del formato
    DECLARE @pesoPorCaja DECIMAL(10,3) = 0;
    SELECT @pesoPorCaja = ISNULL(PesoPorCaja, 0) FROM Formatos WHERE Id = @formatoId;
    DECLARE @pesoTotal DECIMAL(10,2) = @cantidadCajas * @pesoPorCaja;
    
    -- Establecer formato del palet si es la primera composicion
    UPDATE Palets SET FormatoId = @formatoId WHERE Id = @paletId AND FormatoId IS NULL;
    
    -- Insertar composicion (columns that actually exist in ComposicionPalets)
    INSERT INTO ComposicionPalets (
        PaletId, ClienteId, ConsignatarioId, DestinoId, FormatoId, TipoEmpaqueId,
        CalibreId, VariedadId, LugarProduccionId, CodigoRanchoId, TransporteId,
        CantidadCajas, PesoPorCaja, PesoTotal, EsReposicion,
        TipoEmpaqueGuiaId, PresentacionId, TipoCajaId, TipoClamshellId,
        TipoProcesoEmpacadoId, EsEnsayo, VariedadGuiaId,
        UsuarioCreacionId, FechaCreacion
    )
    VALUES (
        @paletId, @clienteId, @consignatarioId, @destinoId, @formatoId, @tipoEmpaqueId,
        @calibreId, @variedadId, @lugarProduccionId, @codigoRanchoId, @transporteId,
        @cantidadCajas, @pesoPorCaja, @pesoTotal, @esReposicion,
        @tipoEmpaqueGuiaId, @presentacionId, @tipoCajaId, @tipoClamshellId,
        @tipoProcesoEmpacadoId, @esEnsayo, @variedadGuiaId,
        @usuarioId, GETDATE()
    );
    
    -- Actualizar totales
    EXEC SP_Palets_ActualizarTotales @PaletId = @paletId;
    
    -- Verificar si alcanzo limite para auto-cerrar
    DECLARE @limite INT = 0, @totalCajas INT = 0;
    SELECT @limite = ISNULL(f.LimiteCajasPorPalet, 0)
    FROM Palets p INNER JOIN Formatos f ON p.FormatoId = f.Id WHERE p.Id = @paletId;
    SELECT @totalCajas = CantidadCajas FROM Palets WHERE Id = @paletId;
    
    DECLARE @pendienteObservacion BIT = 0;
    IF @limite > 0 AND @totalCajas >= @limite
        SET @pendienteObservacion = 1;
    
    SELECT '{"success": true, "message": "Cajas agregadas exitosamente", "pendienteObservacion": ' + 
        CASE WHEN @pendienteObservacion = 1 THEN 'true' ELSE 'false' END + '}' AS JsonData;
END
GO

-- =============================================
-- FIX 2: SP_Palet_EditarCajas - remove CategoriaId
-- =============================================
IF OBJECT_ID('SP_Palet_EditarCajas', 'P') IS NOT NULL DROP PROCEDURE SP_Palet_EditarCajas;
GO
CREATE PROCEDURE SP_Palet_EditarCajas
    @json NVARCHAR(MAX) = '{}'
AS BEGIN
    SET NOCOUNT ON;
    
    DECLARE @composicionId INT, @paletId INT, @consignatarioId INT, @destinoId INT,
            @formatoId INT, @tipoEmpaqueId INT, @calibreId INT, @variedadId INT,
            @lugarProduccionId INT, @codigoRanchoId INT, @transporteId INT,
            @cantidadCajas INT, @esReposicion BIT = 0, @usuarioId INT,
            @tipoEmpaqueGuiaId INT, @presentacionId INT, @tipoCajaId INT,
            @tipoClamshellId INT, @tipoProcesoEmpacadoId INT,
            @esEnsayo BIT = 0, @variedadGuiaId INT;
    
    IF @json IS NOT NULL AND @json != '{}'
    BEGIN
        SELECT 
            @composicionId = JSON_VALUE(@json, '$.composicionId'),
            @paletId = JSON_VALUE(@json, '$.paletId'),
            @consignatarioId = JSON_VALUE(@json, '$.consignatarioId'),
            @destinoId = JSON_VALUE(@json, '$.destinoId'),
            @formatoId = JSON_VALUE(@json, '$.formatoId'),
            @tipoEmpaqueId = JSON_VALUE(@json, '$.tipoEmpaqueId'),
            @calibreId = JSON_VALUE(@json, '$.calibreId'),
            @variedadId = JSON_VALUE(@json, '$.variedadId'),
            @lugarProduccionId = JSON_VALUE(@json, '$.lugarProduccionId'),
            @codigoRanchoId = JSON_VALUE(@json, '$.codigoRanchoId'),
            @transporteId = JSON_VALUE(@json, '$.transporteId'),
            @cantidadCajas = JSON_VALUE(@json, '$.cantidadCajas'),
            @esReposicion = ISNULL(JSON_VALUE(@json, '$.esReposicion'), 0),
            @usuarioId = JSON_VALUE(@json, '$.usuarioId'),
            @tipoEmpaqueGuiaId = JSON_VALUE(@json, '$.tipoEmpaqueGuiaId'),
            @presentacionId = JSON_VALUE(@json, '$.presentacionId'),
            @tipoCajaId = JSON_VALUE(@json, '$.tipoCajaId'),
            @tipoClamshellId = JSON_VALUE(@json, '$.tipoClamshellId'),
            @tipoProcesoEmpacadoId = JSON_VALUE(@json, '$.tipoProcesoEmpacadoId'),
            @esEnsayo = ISNULL(JSON_VALUE(@json, '$.esEnsayo'), 0),
            @variedadGuiaId = JSON_VALUE(@json, '$.variedadGuiaId');
    END
    
    -- Obtener peso por caja del formato
    DECLARE @pesoPorCaja DECIMAL(10,3) = 0;
    SELECT @pesoPorCaja = ISNULL(PesoPorCaja, 0) FROM Formatos WHERE Id = @formatoId;
    DECLARE @pesoTotal DECIMAL(10,2) = @cantidadCajas * @pesoPorCaja;
    
    UPDATE ComposicionPalets
    SET ConsignatarioId = @consignatarioId, DestinoId = @destinoId,
        FormatoId = @formatoId, TipoEmpaqueId = @tipoEmpaqueId,
        CalibreId = @calibreId, VariedadId = @variedadId,
        LugarProduccionId = @lugarProduccionId, CodigoRanchoId = @codigoRanchoId,
        TransporteId = @transporteId, CantidadCajas = @cantidadCajas,
        PesoPorCaja = @pesoPorCaja, PesoTotal = @pesoTotal,
        EsReposicion = @esReposicion, TipoEmpaqueGuiaId = @tipoEmpaqueGuiaId,
        PresentacionId = @presentacionId, TipoCajaId = @tipoCajaId,
        TipoClamshellId = @tipoClamshellId, TipoProcesoEmpacadoId = @tipoProcesoEmpacadoId,
        EsEnsayo = @esEnsayo, VariedadGuiaId = @variedadGuiaId,
        UsuarioModificacionId = @usuarioId, FechaModificacion = GETDATE()
    WHERE Id = @composicionId AND PaletId = @paletId;
    
    IF @@ROWCOUNT = 0
    BEGIN
        SELECT '{"success": false, "message": "Composicion no encontrada"}' AS JsonData;
        RETURN;
    END
    
    EXEC SP_Palets_ActualizarTotales @PaletId = @paletId;
    
    SELECT '{"success": true, "message": "Cajas editadas exitosamente"}' AS JsonData;
END
GO

-- =============================================
-- FIX 3: PALET_ObtenerCatalogos - fix column names per actual tables
-- CodigosRancho: Id, Codigo, LugarProduccionId, Activo (NO Nombre)
-- Presentacion: Id, Nombre, Activo (NO Codigo)
-- Formatos: uses Descripcion not Nombre
-- =============================================
IF OBJECT_ID('PALET_ObtenerCatalogos', 'P') IS NOT NULL DROP PROCEDURE PALET_ObtenerCatalogos;
GO
CREATE PROCEDURE PALET_ObtenerCatalogos
    @json NVARCHAR(MAX) = '{}'
AS BEGIN
    SET NOCOUNT ON;
    
    DECLARE @result NVARCHAR(MAX) = '{';
    
    -- Consignatarios
    SET @result = @result + '"consignatarios": ' + ISNULL((
        SELECT Id, Codigo, RazonSocial AS Nombre FROM Consignatarios WHERE Activo = 1 ORDER BY RazonSocial FOR JSON PATH
    ), '[]') + ',';
    
    -- Destinos
    SET @result = @result + '"destinos": ' + ISNULL((
        SELECT Id, Codigo, Nombre FROM Destinos WHERE Activo = 1 ORDER BY Nombre FOR JSON PATH
    ), '[]') + ',';
    
    -- Formatos (Descripcion, not Nombre)
    SET @result = @result + '"formatos": ' + ISNULL((
        SELECT Id, Codigo, Descripcion AS Nombre, PesoPorCaja, LimiteCajasPorPalet FROM Formatos WHERE Activo = 1 ORDER BY Descripcion FOR JSON PATH
    ), '[]') + ',';
    
    -- Variedades
    SET @result = @result + '"variedades": ' + ISNULL((
        SELECT Id, Codigo, Nombre, Procedencia, EsEnsayo FROM Variedades WHERE Activo = 1 ORDER BY Nombre FOR JSON PATH
    ), '[]') + ',';
    
    -- Calibres
    SET @result = @result + '"calibres": ' + ISNULL((
        SELECT Id, Codigo, Nombre FROM Calibres WHERE Activo = 1 ORDER BY Nombre FOR JSON PATH
    ), '[]') + ',';
    
    -- TiposEmpaque (Descripcion, not Nombre)
    SET @result = @result + '"tiposEmpaque": ' + ISNULL((
        SELECT Id, Codigo, Descripcion AS Nombre FROM TiposEmpaque WHERE Activo = 1 ORDER BY Descripcion FOR JSON PATH
    ), '[]') + ',';
    
    -- TiposEmpaqueGuia
    SET @result = @result + '"tiposEmpaqueGuia": ' + ISNULL((
        SELECT Id, Codigo, Nombre FROM TiposEmpaqueGuia WHERE Activo = 1 ORDER BY Nombre FOR JSON PATH
    ), '[]') + ',';
    
    -- Presentaciones (no Codigo column)
    SET @result = @result + '"presentaciones": ' + ISNULL((
        SELECT Id, Nombre FROM Presentacion WHERE Activo = 1 ORDER BY Nombre FOR JSON PATH
    ), '[]') + ',';
    
    -- TiposCaja
    SET @result = @result + '"tiposCaja": ' + ISNULL((
        SELECT Id, Codigo, Nombre FROM TiposCaja WHERE Activo = 1 ORDER BY Nombre FOR JSON PATH
    ), '[]') + ',';
    
    -- TiposClamshell
    SET @result = @result + '"tiposClamshell": ' + ISNULL((
        SELECT Id, Codigo, Nombre FROM TiposClamshell WHERE Activo = 1 ORDER BY Nombre FOR JSON PATH
    ), '[]') + ',';
    
    -- LugaresProduccion
    SET @result = @result + '"lugaresProduccion": ' + ISNULL((
        SELECT Id, Codigo, Nombre FROM LugaresProduccion WHERE Activo = 1 ORDER BY Nombre FOR JSON PATH
    ), '[]') + ',';
    
    -- CodigosRancho (no Nombre column - uses Codigo only)
    SET @result = @result + '"codigosRancho": ' + ISNULL((
        SELECT Id, Codigo, LugarProduccionId FROM CodigosRancho WHERE Activo = 1 ORDER BY Codigo FOR JSON PATH
    ), '[]') + ',';
    
    -- Transportes
    SET @result = @result + '"transportes": ' + ISNULL((
        SELECT Id, Codigo, Nombre FROM Transporte WHERE Activo = 1 ORDER BY Nombre FOR JSON PATH
    ), '[]') + ',';
    
    -- Categorias
    SET @result = @result + '"categorias": ' + ISNULL((
        SELECT Id, Codigo, Nombre FROM Categorias WHERE Activo = 1 ORDER BY Nombre FOR JSON PATH
    ), '[]') + ',';
    
    -- TiposProcesoEmpacado
    SET @result = @result + '"tiposProcesoEmpacado": ' + ISNULL((
        SELECT Id, Codigo, Nombre FROM TipoProcesoEmpacado WHERE Activo = 1 ORDER BY Id FOR JSON PATH
    ), '[]') + ',';
    
    -- Acopios
    SET @result = @result + '"acopios": ' + ISNULL((
        SELECT Id, Codigo, Nombre FROM Acopios WHERE Activo = 1 ORDER BY Nombre FOR JSON PATH
    ), '[]') + ',';
    
    -- Campanias
    SET @result = @result + '"campanias": ' + ISNULL((
        SELECT Id, Nombre, FechaInicio, FechaFin, Activa FROM Campanias ORDER BY Id DESC FOR JSON PATH
    ), '[]');
    
    SET @result = @result + '}';
    
    SELECT @result AS JsonData;
END
GO

PRINT 'All 3 failed SPs fixed successfully';
GO
