USE SistemaPalets;
GO

-- Test SP_Palet_ObtenerPorProceso with procesoId = 27
DECLARE @json NVARCHAR(MAX) = '{"procesoId": 27}';
EXEC SP_Palet_ObtenerPorProceso @json;
GO
