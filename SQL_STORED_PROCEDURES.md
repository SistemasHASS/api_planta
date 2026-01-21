# Stored Procedures Required for Production Lines Module

## Database: SQL Server

### 1. PLANTA_listado_lineasproduccion
**Purpose:** List all production lines for a specific company (RUC)

**Input Parameters:**
```json
[
  {
    "ruc": "string"
  }
]
```

**Expected Output:** JSON array with production lines
```json
[
  {
    "id": "int",
    "ruc": "varchar(20)",
    "linea": "varchar(50)",
    "espacios": "int",
    "color": "varchar(50)",
    "estado": "int"
  }
]
```

**SQL Example:**
```sql
CREATE PROCEDURE PLANTA_listado_lineasproduccion
    @json NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @ruc VARCHAR(20);
    
    SELECT @ruc = JSON_VALUE(@json, '$[0].ruc');
    
    SELECT 
        id,
        ruc,
        linea,
        espacios,
        color,
        estado
    FROM dbo.PLANTA_lineaproduccion
    WHERE ruc = @ruc 
      AND estado = 1
    ORDER BY linea
    FOR JSON PATH;
END
```

---

### 2. PLANTA_crud_lineaproduccion
**Purpose:** Create, Update, or Delete (soft delete) production lines

**Input Parameters:**
```json
[
  {
    "ruc": "string",
    "nrodocumento": "string",
    "idlineaprodempa": "int (0 for new, >0 for update)",
    "linea": "string",
    "espacios": "int",
    "color": "string",
    "estado": "int (1=active, 0=deleted)"
  }
]
```

**Expected Output:** JSON with success/error message
```json
[
  {
    "success": true,
    "message": "Operación exitosa",
    "id": "int"
  }
]
```

**SQL Example:**
```sql
CREATE PROCEDURE PLANTA_crud_lineaproduccion
    @json NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @ruc VARCHAR(20);
    DECLARE @nrodocumento VARCHAR(20);
    DECLARE @idlineaprodempa INT;
    DECLARE @linea VARCHAR(50);
    DECLARE @espacios INT;
    DECLARE @color VARCHAR(50);
    DECLARE @estado INT;
    
    SELECT 
        @ruc = JSON_VALUE(@json, '$[0].ruc'),
        @nrodocumento = JSON_VALUE(@json, '$[0].nrodocumento'),
        @idlineaprodempa = JSON_VALUE(@json, '$[0].idlineaprodempa'),
        @linea = JSON_VALUE(@json, '$[0].linea'),
        @espacios = JSON_VALUE(@json, '$[0].espacios'),
        @color = JSON_VALUE(@json, '$[0].color'),
        @estado = JSON_VALUE(@json, '$[0].estado');
    
    BEGIN TRY
        IF @idlineaprodempa = 0 OR @idlineaprodempa IS NULL
        BEGIN
            -- INSERT new record
            INSERT INTO dbo.PLANTA_lineaproduccion (ruc, linea, espacios, color, estado, fecharegistrobd)
            VALUES (@ruc, @linea, @espacios, @color, @estado, GETDATE());
            
            SET @idlineaprodempa = SCOPE_IDENTITY();
            
            SELECT 
                1 AS success,
                'Línea de producción creada exitosamente' AS message,
                @idlineaprodempa AS id
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER;
        END
        ELSE
        BEGIN
            -- UPDATE existing record
            UPDATE dbo.PLANTA_lineaproduccion
            SET 
                linea = @linea,
                espacios = @espacios,
                color = @color,
                estado = @estado,
                fecharegistrobd = GETDATE()
            WHERE id = @idlineaprodempa
              AND ruc = @ruc;
            
            SELECT 
                1 AS success,
                CASE 
                    WHEN @estado = 0 THEN 'Línea de producción eliminada exitosamente'
                    ELSE 'Línea de producción actualizada exitosamente'
                END AS message,
                @idlineaprodempa AS id
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER;
        END
    END TRY
    BEGIN CATCH
        SELECT 
            0 AS success,
            ERROR_MESSAGE() AS message,
            0 AS id
        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER;
    END CATCH
END
```

---

## Table Schema Reference

Based on the database image provided, the table structure is:

**Table: dbo.PLANTA_lineaproduccion**
- `id` (PK, int, not null) - Auto-increment primary key
- `ruc` (varchar(20), null) - Company tax ID
- `linea` (varchar(50), null) - Production line name
- `espacios` (int, null) - Number of spaces/positions
- `color` (varchar(50), null) - Color identifier for the line
- `estado` (int, null) - Status (1=active, 0=inactive/deleted)
- `fecharegistrobd` (datetime2(0), null) - Registration timestamp

## Notes
- The module uses soft delete (estado = 0) instead of physical deletion
- All operations are tracked with timestamps
- The RUC field is used for multi-tenant data isolation
