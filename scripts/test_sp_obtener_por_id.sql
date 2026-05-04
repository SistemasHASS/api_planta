USE SistemaPalets;
GO

-- Test SP_Palet_ObtenerPorId with an existing palet (ID 206 from the earlier query)
DECLARE @json NVARCHAR(MAX) = '{"id": 206}';
EXEC SP_Palet_ObtenerPorId @json;
GO
