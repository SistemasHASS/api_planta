ALTER PROC [dbo].[PLANTA_sincronizarlineasproduccion] 
	@json VARCHAR(MAX)
AS BEGIN
	SET NOCOUNT ON;

	DECLARE @idlinea INT;
	CREATE TABLE #jP_slp (
		ruc VARCHAR(20),
		idfundo VARCHAR(20),
		idacopio VARCHAR(20),
		idlinea INT,
		linea VARCHAR(250),
		espacios INT,
		color VARCHAR(20),
		estado INT,
		importado INT
	);

	INSERT INTO #jP_slp (ruc, idfundo, idacopio, idlinea, linea, espacios, color, estado, importado)
	SELECT ruc, idfundo, idacopio, idlinea, linea, espacios, color, estado, 0 AS importado
	FROM OPENJSON(@json) WITH (
		ruc VARCHAR(20),
		idfundo VARCHAR(20),
		idacopio VARCHAR(20),
		idlinea INT,
		linea VARCHAR(250),
		espacios INT,
		color VARCHAR(20),
		estado INT
	);
	------------------------------------------------------------------------------------------------------------------------------------------

	-- Update existing lines
	UPDATE p SET 
		linea = j.linea,
		espacios = j.espacios,
		color = j.color,
		estado = j.estado,
		idfundo = j.idfundo,
		idacopio = j.idacopio
	FROM #jP_slp j 
	LEFT JOIN PLANTA_lineaproduccion p ON j.ruc = p.ruc AND j.idlinea = p.id
	WHERE p.linea != j.linea 
		OR p.espacios != j.espacios 
		OR p.color != j.color 
		OR p.estado != j.estado
		OR ISNULL(p.idfundo, '') != ISNULL(j.idfundo, '')
		OR ISNULL(p.idacopio, '') != ISNULL(j.idacopio, '');

	-- Insert new lines
	INSERT INTO PLANTA_lineaproduccion (ruc, idfundo, idacopio, linea, espacios, color, estado)
	SELECT j.ruc, j.idfundo, j.idacopio, j.linea, j.espacios, j.color, 1 
	FROM #jP_slp j 
	LEFT JOIN PLANTA_lineaproduccion p ON j.ruc = p.ruc AND j.linea = p.linea
	WHERE j.idlinea = 0 AND p.linea IS NULL;

	------------------------------------------------------------------------------------------------------------------------------------------
	DECLARE @consulta VARCHAR(MAX) = '';
	DECLARE @error INT = 0;

	-- Mark imported records
	UPDATE j SET importado = 1 
	FROM #jP_slp j 
	INNER JOIN PLANTA_lineaproduccion p ON j.ruc = p.ruc AND j.linea = p.linea;

	SET @error = ISNULL((SELECT TOP 1 1 FROM #jP_slp WHERE importado = 0), 0);

	-- FIX: Add alias to the subquery to avoid JSON formatting error
	SET @consulta = (
		SELECT 
			@error AS errorgeneral,
			(SELECT ruc + linea AS error FROM #jP_slp WHERE importado = 0 FOR JSON PATH) AS detalle 
		FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
	);

	SELECT ISNULL(@consulta, '[{"errorgeneral":-1}]') AS id;

	IF @error > 0
	BEGIN
		INSERT INTO PLANTA_logerror (sp, error) 
		VALUES ('exec PLANTA_sincronizarlineasproduccion ''' + @json + ''';', @consulta)
	END
	ELSE 
	BEGIN
		INSERT INTO PLANTA_logexito (sp, error) 
		VALUES ('exec PLANTA_sincronizarlineasproduccion ''' + @json + ''';', 'Exito')
	END

	DROP TABLE #jP_slp;
END
