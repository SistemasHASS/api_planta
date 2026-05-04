USE SistemaPalets;
GO

-- Test cascading filters to see what data exists

-- 1. Check Consignatarios
SELECT 'Consignatarios' AS Tabla, COUNT(*) AS Total FROM Consignatarios WHERE Activo = 1;
SELECT Id, Codigo, RazonSocial FROM Consignatarios WHERE Activo = 1 ORDER BY RazonSocial;
GO

-- 2. Check MatrizCompatibilidad entries
SELECT 'MatrizCompatibilidad' AS Tabla, COUNT(*) AS Total FROM MatrizCompatibilidad WHERE Activo = 1;
GO

-- 3. Test: Get Destinos for "Del Monte Fresh" (assuming Id = 1 or find it)
DECLARE @consignatarioId INT;
SELECT TOP 1 @consignatarioId = Id FROM Consignatarios WHERE RazonSocial LIKE '%Del Monte%' AND Activo = 1;

SELECT 'Testing Destinos for ConsignatarioId' AS Test, @consignatarioId AS ConsignatarioId;

SELECT DISTINCT d.Id, d.Codigo, d.Nombre, d.Pais
FROM MatrizCompatibilidad mc
INNER JOIN Destinos d ON mc.DestinoId = d.Id
WHERE mc.ConsignatarioId = @consignatarioId
    AND mc.Activo = 1
    AND d.Activo = 1
ORDER BY d.Nombre;
GO

-- 4. Test: Get Formatos for Del Monte Fresh + USA
DECLARE @consignatarioId INT, @destinoId INT;
SELECT TOP 1 @consignatarioId = Id FROM Consignatarios WHERE RazonSocial LIKE '%Del Monte%' AND Activo = 1;
SELECT TOP 1 @destinoId = Id FROM Destinos WHERE Nombre = 'USA' AND Activo = 1;

SELECT 'Testing Formatos' AS Test, @consignatarioId AS ConsignatarioId, @destinoId AS DestinoId;

SELECT DISTINCT f.Id, f.Codigo, f.Descripcion AS Nombre, f.PesoPorCaja, f.LimiteCajasPorPalet
FROM MatrizCompatibilidad mc
INNER JOIN Formatos f ON mc.FormatoId = f.Id
WHERE mc.ConsignatarioId = @consignatarioId
    AND mc.DestinoId = @destinoId
    AND mc.Activo = 1
    AND f.Activo = 1
ORDER BY f.Descripcion;
GO

-- 5. Check if there are any matriz entries at all
SELECT TOP 10 
    mc.Id,
    c.RazonSocial AS Consignatario,
    d.Nombre AS Destino,
    f.Descripcion AS Formato,
    teg.Nombre AS TipoEmpaqueGuia,
    p.Nombre AS Presentacion,
    tc.Nombre AS TipoCaja,
    tcl.Nombre AS TipoClamshell
FROM MatrizCompatibilidad mc
LEFT JOIN Consignatarios c ON mc.ConsignatarioId = c.Id
LEFT JOIN Destinos d ON mc.DestinoId = d.Id
LEFT JOIN Formatos f ON mc.FormatoId = f.Id
LEFT JOIN TiposEmpaqueGuia teg ON mc.TipoEmpaqueGuiaId = teg.Id
LEFT JOIN Presentacion p ON mc.PresentacionId = p.Id
LEFT JOIN TiposCaja tc ON mc.TipoCajaId = tc.Id
LEFT JOIN TiposClamshell tcl ON mc.TipoClamshellId = tcl.Id
WHERE mc.Activo = 1;
GO
