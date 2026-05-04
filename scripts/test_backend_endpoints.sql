USE SistemaPalets;
GO

-- Test the exact JSON that frontend is sending
DECLARE @json NVARCHAR(MAX) = '{"consignatarioId":3,"destinoId":9}';

SELECT 'Testing SP_Palet_ObtenerFormatos with frontend JSON' AS Test;
EXEC SP_Palet_ObtenerFormatos @json;
GO

-- Test with different JSON format
DECLARE @json2 NVARCHAR(MAX) = '{"consignatarioId": 3, "destinoId": 9}';

SELECT 'Testing SP_Palet_ObtenerFormatos with spaced JSON' AS Test;
EXEC SP_Palet_ObtenerFormatos @json2;
GO

-- Test destinos endpoint
DECLARE @json3 NVARCHAR(MAX) = '{"consignatarioId":3}';

SELECT 'Testing SP_Palet_ObtenerDestinosPorConsignatario' AS Test;
EXEC SP_Palet_ObtenerDestinosPorConsignatario @json3;
GO
