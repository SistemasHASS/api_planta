USE SistemaPalets;
GO

-- Find Del Monte Fresh ID
DECLARE @consignatarioId INT;
SELECT @consignatarioId = Id FROM Consignatarios WHERE RazonSocial LIKE '%Del Monte%' AND Activo = 1;
SELECT 'Del Monte Fresh ID' AS Info, @consignatarioId AS Id;
GO

-- Find USA ID
DECLARE @destinoId INT;
SELECT @destinoId = Id FROM Destinos WHERE Nombre = 'USA' AND Activo = 1;
SELECT 'USA ID' AS Info, @destinoId AS Id;
GO

-- Test SP_Palet_ObtenerDestinosPorConsignatario
DECLARE @consignatarioId INT;
SELECT @consignatarioId = Id FROM Consignatarios WHERE RazonSocial LIKE '%Del Monte%' AND Activo = 1;

DECLARE @json NVARCHAR(MAX) = '{"consignatarioId": ' + CAST(@consignatarioId AS NVARCHAR(10)) + '}';
EXEC SP_Palet_ObtenerDestinosPorConsignatario @json;
GO

-- Test SP_Palet_ObtenerFormatos
DECLARE @consignatarioId INT, @destinoId INT;
SELECT @consignatarioId = Id FROM Consignatarios WHERE RazonSocial LIKE '%Del Monte%' AND Activo = 1;
SELECT @destinoId = Id FROM Destinos WHERE Nombre = 'USA' AND Activo = 1;

DECLARE @json NVARCHAR(MAX) = '{"consignatarioId": ' + CAST(@consignatarioId AS NVARCHAR(10)) + ', "destinoId": ' + CAST(@destinoId AS NVARCHAR(10)) + '}';
EXEC SP_Palet_ObtenerFormatos @json;
GO

-- Check if there are matriz entries for Del Monte + USA
DECLARE @consignatarioId INT, @destinoId INT;
SELECT @consignatarioId = Id FROM Consignatarios WHERE RazonSocial LIKE '%Del Monte%' AND Activo = 1;
SELECT @destinoId = Id FROM Destinos WHERE Nombre = 'USA' AND Activo = 1;

SELECT 'MatrizCompatibilidad entries for Del Monte + USA' AS Info;
SELECT COUNT(*) AS Total
FROM MatrizCompatibilidad mc
WHERE mc.ConsignatarioId = @consignatarioId
    AND mc.DestinoId = @destinoId
    AND mc.Activo = 1;

SELECT TOP 5
    mc.Id,
    c.RazonSocial AS Consignatario,
    d.Nombre AS Destino,
    f.Descripcion AS Formato,
    teg.Nombre AS TipoEmpaqueGuia
FROM MatrizCompatibilidad mc
INNER JOIN Consignatarios c ON mc.ConsignatarioId = c.Id
INNER JOIN Destinos d ON mc.DestinoId = d.Id
INNER JOIN Formatos f ON mc.FormatoId = f.Id
LEFT JOIN TiposEmpaqueGuia teg ON mc.TipoEmpaqueGuiaId = teg.Id
WHERE mc.ConsignatarioId = @consignatarioId
    AND mc.DestinoId = @destinoId
    AND mc.Activo = 1;
GO
