USE SistemaPalets;
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
    
    -- Formatos (Descripcion not Nombre)
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
    
    -- TiposEmpaque (Descripcion not Nombre)
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
    
    -- LugaresProduccion (Descripcion not Nombre)
    SET @result = @result + '"lugaresProduccion": ' + ISNULL((
        SELECT Id, Codigo, Descripcion AS Nombre FROM LugaresProduccion WHERE Activo = 1 ORDER BY Descripcion FOR JSON PATH
    ), '[]') + ',';
    
    -- CodigosRancho (no Nombre column)
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

PRINT 'PALET_ObtenerCatalogos created successfully';
GO
