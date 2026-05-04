-- =============================================
-- Script: Create missing JSON-based SPs for palets_api Clean Architecture
-- Date: 2026-03-30
-- Description: JSON wrapper SPs that accept @json NVARCHAR(MAX) parameter
-- =============================================

USE SistemaPalets;
GO

-- =============================================
-- 1. SP_Palet_ObtenerPorProceso (JSON wrapper)
-- =============================================
IF OBJECT_ID('SP_Palet_ObtenerPorProceso', 'P') IS NOT NULL DROP PROCEDURE SP_Palet_ObtenerPorProceso;
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

-- =============================================
-- 2. SP_Palet_ObtenerPorId (JSON wrapper)
-- =============================================
IF OBJECT_ID('SP_Palet_ObtenerPorId', 'P') IS NOT NULL DROP PROCEDURE SP_Palet_ObtenerPorId;
GO
CREATE PROCEDURE SP_Palet_ObtenerPorId
    @json NVARCHAR(MAX) = '{}'
AS BEGIN
    SET NOCOUNT ON;
    
    DECLARE @id INT = 0;
    
    IF @json IS NOT NULL AND @json != '{}'
    BEGIN
        SELECT @id = ISNULL(JSON_VALUE(@json, '$.id'), 0);
    END
    
    -- Palet info
    DECLARE @paletJson NVARCHAR(MAX);
    SELECT @paletJson = (
        SELECT 
            p.Id, p.NumeroPalet, p.Estado, p.CantidadCajas, p.PesoTotal,
            p.PorcentajeAvance, p.FormatoId, p.ProcesoId, p.AcopioId,
            p.FechaCreacion, p.FechaCierre, p.Observaciones, p.MedidaCorrectiva,
            f.Descripcion AS FormatoDescripcion, f.LimiteCajasPorPalet,
            a.Codigo AS AcopioCodigo, a.Nombre AS AcopioNombre,
            pr.Turno,
            (SELECT MIN(cp.FechaCreacion) FROM ComposicionPalets cp WHERE cp.PaletId = p.Id) AS PrimeraComposicionFecha
        FROM Palets p WITH (NOLOCK)
        LEFT JOIN Formatos f WITH (NOLOCK) ON p.FormatoId = f.Id
        INNER JOIN Acopios a WITH (NOLOCK) ON p.AcopioId = a.Id
        INNER JOIN Procesos pr WITH (NOLOCK) ON p.ProcesoId = pr.Id
        WHERE p.Id = @id
        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
    );
    
    -- Composicion
    DECLARE @composicionJson NVARCHAR(MAX);
    SELECT @composicionJson = (
        SELECT 
            c.Id, c.PaletId, c.ConsignatarioId, c.DestinoId, c.FormatoId,
            c.TipoEmpaqueId, c.CalibreId, c.VariedadId, c.LugarProduccionId,
            c.CodigoRanchoId, c.TransporteId, c.CantidadCajas, c.PesoPorCaja,
            c.PesoTotal, c.EsReposicion, c.FechaCreacion,
            cons.RazonSocial AS ConsignatarioNombre,
            d.Nombre AS DestinoNombre,
            f.Descripcion AS FormatoNombre,
            f.PesoPorCaja AS FormatoPesoPorCaja,
            te.Descripcion AS TipoEmpaqueNombre,
            cal.Nombre AS CalibreNombre,
            v.Nombre AS VariedadNombre,
            lp.Codigo AS LDP,
            cr.Codigo AS CodigoRancho,
            t.Nombre AS TransporteNombre,
            teg.Nombre AS TipoEmpaqueGuiaNombre,
            pre.Nombre AS Presentacion,
            tpe.Codigo AS TipoProcesoEmpacadoCodigo
        FROM ComposicionPalets c WITH (NOLOCK)
        INNER JOIN Consignatarios cons WITH (NOLOCK) ON c.ConsignatarioId = cons.Id
        INNER JOIN Destinos d WITH (NOLOCK) ON c.DestinoId = d.Id
        INNER JOIN Formatos f WITH (NOLOCK) ON c.FormatoId = f.Id
        INNER JOIN TiposEmpaque te WITH (NOLOCK) ON c.TipoEmpaqueId = te.Id
        LEFT JOIN Calibres cal WITH (NOLOCK) ON c.CalibreId = cal.Id
        INNER JOIN Variedades v WITH (NOLOCK) ON c.VariedadId = v.Id
        INNER JOIN LugaresProduccion lp WITH (NOLOCK) ON c.LugarProduccionId = lp.Id
        INNER JOIN CodigosRancho cr WITH (NOLOCK) ON c.CodigoRanchoId = cr.Id
        LEFT JOIN Transporte t WITH (NOLOCK) ON c.TransporteId = t.Id
        LEFT JOIN TiposEmpaqueGuia teg WITH (NOLOCK) ON c.TipoEmpaqueGuiaId = teg.Id
        LEFT JOIN Presentacion pre WITH (NOLOCK) ON c.PresentacionId = pre.Id
        LEFT JOIN TipoProcesoEmpacado tpe WITH (NOLOCK) ON c.TipoProcesoEmpacadoId = tpe.Id
        WHERE c.PaletId = @id
        ORDER BY c.Id
        FOR JSON PATH
    );
    
    SELECT '{"success": true, "palet": ' + ISNULL(@paletJson, 'null') + ', "Composicion": ' + ISNULL(@composicionJson, '[]') + '}' AS JsonData;
END
GO

-- =============================================
-- 3. SP_Palet_AgregarCajas (JSON wrapper)
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
            @esEnsayo BIT = 0, @variedadGuiaId INT, @categoriaId INT;
    
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
            @categoriaId = JSON_VALUE(@json, '$.categoriaId');
    END
    
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
    
    -- Insertar composicion
    INSERT INTO ComposicionPalets (
        PaletId, ConsignatarioId, DestinoId, FormatoId, TipoEmpaqueId,
        CalibreId, VariedadId, LugarProduccionId, CodigoRanchoId, TransporteId,
        CantidadCajas, PesoPorCaja, PesoTotal, EsReposicion,
        TipoEmpaqueGuiaId, PresentacionId, TipoCajaId, TipoClamshellId,
        TipoProcesoEmpacadoId, EsEnsayo, VariedadGuiaId, CategoriaId,
        UsuarioCreacionId, FechaCreacion
    )
    VALUES (
        @paletId, @consignatarioId, @destinoId, @formatoId, @tipoEmpaqueId,
        @calibreId, @variedadId, @lugarProduccionId, @codigoRanchoId, @transporteId,
        @cantidadCajas, @pesoPorCaja, @pesoTotal, @esReposicion,
        @tipoEmpaqueGuiaId, @presentacionId, @tipoCajaId, @tipoClamshellId,
        @tipoProcesoEmpacadoId, @esEnsayo, @variedadGuiaId, @categoriaId,
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
-- 4. SP_Palet_EditarCajas (JSON wrapper)
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
            @esEnsayo BIT = 0, @variedadGuiaId INT, @categoriaId INT;
    
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
            @variedadGuiaId = JSON_VALUE(@json, '$.variedadGuiaId'),
            @categoriaId = JSON_VALUE(@json, '$.categoriaId');
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
        EsEnsayo = @esEnsayo, VariedadGuiaId = @variedadGuiaId, CategoriaId = @categoriaId,
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
-- 5. SP_Proceso_Listar (JSON wrapper)
-- =============================================
IF OBJECT_ID('SP_Proceso_Listar', 'P') IS NOT NULL DROP PROCEDURE SP_Proceso_Listar;
GO
CREATE PROCEDURE SP_Proceso_Listar
    @json NVARCHAR(MAX) = '{}'
AS BEGIN
    SET NOCOUNT ON;
    
    DECLARE @acopioId INT = NULL, @estado VARCHAR(20) = NULL;
    
    IF @json IS NOT NULL AND @json != '{}'
    BEGIN
        SET @acopioId = JSON_VALUE(@json, '$.acopioId');
        SET @estado = JSON_VALUE(@json, '$.estado');
        IF @estado = 'TODOS' SET @estado = NULL;
    END
    
    SELECT (
        SELECT 
            p.Id, p.FechaProceso, p.Estado, p.AcopioId, p.Turno,
            p.CampaniaId, p.FechaApertura, p.FechaCierre,
            p.UsuarioAperturaId, p.UsuarioCierreId,
            a.Codigo AS AcopioCodigo, a.Nombre AS AcopioNombre,
            ua.NombreCompleto AS UsuarioApertura,
            uc.NombreCompleto AS UsuarioCierre,
            c.Nombre AS CampaniaNombre
        FROM Procesos p
        INNER JOIN Acopios a ON p.AcopioId = a.Id
        LEFT JOIN Usuarios ua ON p.UsuarioAperturaId = ua.Id
        LEFT JOIN Usuarios uc ON p.UsuarioCierreId = uc.Id
        LEFT JOIN Campanias c ON p.CampaniaId = c.Id
        WHERE (@acopioId IS NULL OR p.AcopioId = @acopioId)
          AND (@estado IS NULL OR p.Estado = @estado)
        ORDER BY p.FechaProceso DESC, a.Codigo, p.Turno
        FOR JSON PATH
    ) AS JsonData;
END
GO

-- =============================================
-- 6. SP_Proceso_ObtenerPorId (JSON wrapper)
-- =============================================
IF OBJECT_ID('SP_Proceso_ObtenerPorId', 'P') IS NOT NULL DROP PROCEDURE SP_Proceso_ObtenerPorId;
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
            p.Id, p.FechaProceso, p.Estado, p.AcopioId, p.Turno,
            p.CampaniaId, p.FechaApertura, p.FechaCierre,
            a.Codigo AS AcopioCodigo, a.Nombre AS AcopioNombre,
            ua.NombreCompleto AS UsuarioApertura,
            uc.NombreCompleto AS UsuarioCierre,
            c.Nombre AS CampaniaNombre
        FROM Procesos p
        INNER JOIN Acopios a ON p.AcopioId = a.Id
        LEFT JOIN Usuarios ua ON p.UsuarioAperturaId = ua.Id
        LEFT JOIN Usuarios uc ON p.UsuarioCierreId = uc.Id
        LEFT JOIN Campanias c ON p.CampaniaId = c.Id
        WHERE p.Id = @id
        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
    ) AS JsonData;
END
GO

-- =============================================
-- 7. SP_Proceso_Crear (JSON wrapper)
-- =============================================
IF OBJECT_ID('SP_Proceso_Crear', 'P') IS NOT NULL DROP PROCEDURE SP_Proceso_Crear;
GO
CREATE PROCEDURE SP_Proceso_Crear
    @json NVARCHAR(MAX) = '{}'
AS BEGIN
    SET NOCOUNT ON;
    
    DECLARE @fechaProceso DATE, @acopioId INT, @turno VARCHAR(10),
            @usuarioId INT, @campaniaId INT = NULL;
    
    IF @json IS NOT NULL AND @json != '{}'
    BEGIN
        SELECT 
            @fechaProceso = JSON_VALUE(@json, '$.fechaProceso'),
            @acopioId = JSON_VALUE(@json, '$.acopioId'),
            @turno = JSON_VALUE(@json, '$.turno'),
            @usuarioId = JSON_VALUE(@json, '$.usuarioId'),
            @campaniaId = JSON_VALUE(@json, '$.campaniaId');
    END
    
    -- Validar duplicado
    IF EXISTS (
        SELECT 1 FROM Procesos
        WHERE AcopioId = @acopioId AND FechaProceso = @fechaProceso AND Turno = @turno AND Estado = 'ABIERTO'
    )
    BEGIN
        SELECT '{"success": false, "message": "Ya existe un proceso abierto para este acopio, fecha y turno"}' AS JsonData;
        RETURN;
    END
    
    INSERT INTO Procesos (FechaProceso, Estado, AcopioId, Turno, UsuarioAperturaId, FechaApertura, CampaniaId)
    VALUES (@fechaProceso, 'ABIERTO', @acopioId, @turno, @usuarioId, GETDATE(), @campaniaId);
    
    DECLARE @newId INT = SCOPE_IDENTITY();
    
    SELECT (
        SELECT '{"success": true, "message": "Proceso creado exitosamente", "data": ' + (
            SELECT p.Id, p.FechaProceso, p.Estado, p.AcopioId, p.Turno,
                   a.Codigo AS AcopioCodigo, a.Nombre AS AcopioNombre
            FROM Procesos p
            INNER JOIN Acopios a ON p.AcopioId = a.Id
            WHERE p.Id = @newId
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        ) + '}'
    ) AS JsonData;
END
GO

-- =============================================
-- 8. SP_Proceso_Cerrar (JSON wrapper)
-- =============================================
IF OBJECT_ID('SP_Proceso_Cerrar', 'P') IS NOT NULL DROP PROCEDURE SP_Proceso_Cerrar;
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
    
    UPDATE Procesos
    SET Estado = 'CERRADO', FechaCierre = GETDATE(), UsuarioCierreId = @usuarioId
    WHERE Id = @id AND Estado = 'ABIERTO';
    
    IF @@ROWCOUNT = 0
    BEGIN
        SELECT '{"success": false, "message": "El proceso no existe o ya esta cerrado"}' AS JsonData;
        RETURN;
    END
    
    SELECT '{"success": true, "message": "Proceso cerrado exitosamente"}' AS JsonData;
END
GO

-- =============================================
-- 9. SP_Proceso_Reabrir (JSON wrapper)
-- =============================================
IF OBJECT_ID('SP_Proceso_Reabrir', 'P') IS NOT NULL DROP PROCEDURE SP_Proceso_Reabrir;
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
    
    UPDATE Procesos
    SET Estado = 'ABIERTO', FechaCierre = NULL, UsuarioCierreId = NULL
    WHERE Id = @id AND Estado = 'CERRADO';
    
    IF @@ROWCOUNT = 0
    BEGIN
        SELECT '{"success": false, "message": "El proceso no existe o no esta cerrado"}' AS JsonData;
        RETURN;
    END
    
    SELECT '{"success": true, "message": "Proceso reabierto exitosamente"}' AS JsonData;
END
GO

-- =============================================
-- 10. PALET_ObtenerCatalogos (combined catalog query)
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
        SELECT Id, Codigo, RazonSocial AS Nombre, Activo FROM Consignatarios WHERE Activo = 1 ORDER BY RazonSocial FOR JSON PATH
    ), '[]') + ',';
    
    -- Destinos
    SET @result = @result + '"destinos": ' + ISNULL((
        SELECT Id, Codigo, Nombre, Activo FROM Destinos WHERE Activo = 1 ORDER BY Nombre FOR JSON PATH
    ), '[]') + ',';
    
    -- Formatos
    SET @result = @result + '"formatos": ' + ISNULL((
        SELECT Id, Codigo, Descripcion, PesoPorCaja, LimiteCajasPorPalet, Activo FROM Formatos WHERE Activo = 1 ORDER BY Descripcion FOR JSON PATH
    ), '[]') + ',';
    
    -- Variedades
    SET @result = @result + '"variedades": ' + ISNULL((
        SELECT Id, Codigo, Nombre, Procedencia, EsEnsayo, Activo FROM Variedades WHERE Activo = 1 ORDER BY Nombre FOR JSON PATH
    ), '[]') + ',';
    
    -- Calibres
    SET @result = @result + '"calibres": ' + ISNULL((
        SELECT Id, Codigo, Nombre, Activo FROM Calibres WHERE Activo = 1 ORDER BY Nombre FOR JSON PATH
    ), '[]') + ',';
    
    -- TiposEmpaque
    SET @result = @result + '"tiposEmpaque": ' + ISNULL((
        SELECT Id, Codigo, Descripcion AS Nombre, Activo FROM TiposEmpaque WHERE Activo = 1 ORDER BY Descripcion FOR JSON PATH
    ), '[]') + ',';
    
    -- TiposEmpaqueGuia
    SET @result = @result + '"tiposEmpaqueGuia": ' + ISNULL((
        SELECT Id, Codigo, Nombre, Activo FROM TiposEmpaqueGuia WHERE Activo = 1 ORDER BY Nombre FOR JSON PATH
    ), '[]') + ',';
    
    -- Presentaciones
    SET @result = @result + '"presentaciones": ' + ISNULL((
        SELECT Id, Codigo, Nombre, Activo FROM Presentacion WHERE Activo = 1 ORDER BY Nombre FOR JSON PATH
    ), '[]') + ',';
    
    -- TiposCaja
    SET @result = @result + '"tiposCaja": ' + ISNULL((
        SELECT Id, Codigo, Nombre, Activo FROM TiposCaja WHERE Activo = 1 ORDER BY Nombre FOR JSON PATH
    ), '[]') + ',';
    
    -- TiposClamshell
    SET @result = @result + '"tiposClamshell": ' + ISNULL((
        SELECT Id, Codigo, Nombre, Activo FROM TiposClamshell WHERE Activo = 1 ORDER BY Nombre FOR JSON PATH
    ), '[]') + ',';
    
    -- LugaresProduccion
    SET @result = @result + '"lugaresProduccion": ' + ISNULL((
        SELECT Id, Codigo, Nombre, Activo FROM LugaresProduccion WHERE Activo = 1 ORDER BY Nombre FOR JSON PATH
    ), '[]') + ',';
    
    -- CodigosRancho
    SET @result = @result + '"codigosRancho": ' + ISNULL((
        SELECT Id, Codigo, Activo FROM CodigosRancho WHERE Activo = 1 ORDER BY Codigo FOR JSON PATH
    ), '[]') + ',';
    
    -- Transportes
    SET @result = @result + '"transportes": ' + ISNULL((
        SELECT Id, Codigo, Nombre, Activo FROM Transporte WHERE Activo = 1 ORDER BY Nombre FOR JSON PATH
    ), '[]') + ',';
    
    -- Categorias
    SET @result = @result + '"categorias": ' + ISNULL((
        SELECT Id, Codigo, Nombre, Activo FROM Categorias WHERE Activo = 1 ORDER BY Nombre FOR JSON PATH
    ), '[]') + ',';
    
    -- TiposProcesoEmpacado
    SET @result = @result + '"tiposProcesoEmpacado": ' + ISNULL((
        SELECT Id, Codigo, Nombre, Activo FROM TipoProcesoEmpacado WHERE Activo = 1 ORDER BY Id FOR JSON PATH
    ), '[]') + ',';
    
    -- Acopios
    SET @result = @result + '"acopios": ' + ISNULL((
        SELECT Id, Codigo, Nombre, Activo FROM Acopios WHERE Activo = 1 ORDER BY Nombre FOR JSON PATH
    ), '[]') + ',';
    
    -- Campanias
    SET @result = @result + '"campanias": ' + ISNULL((
        SELECT Id, Nombre, FechaInicio, FechaFin, Activa FROM Campanias ORDER BY Id DESC FOR JSON PATH
    ), '[]');
    
    SET @result = @result + '}';
    
    SELECT @result AS JsonData;
END
GO

PRINT '✅ All missing JSON-based SPs created successfully';
GO
