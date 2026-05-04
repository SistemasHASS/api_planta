# Sistema de Gestión Logística API

API REST desarrollada en **ASP.NET Core 8** con arquitectura limpia (Clean Architecture) para la gestión de palets, procesos de empaque, guías de transporte y catálogos logísticos.

## Tecnologías

- .NET 8 / ASP.NET Core 8
- Entity Framework Core 9 + SQL Server
- JWT Bearer Authentication (BCrypt password hashing)
- Swagger / OpenAPI
- Serilog (logs a consola y archivo)

## Arquitectura

```
palets_api/
├── Application/
│   └── Usecase/          # Implementaciones de casos de uso
├── Controllers/          # Controladores HTTP (Auth, Catálogos, Guías, Procesos)
├── Domain/
│   ├── Repository/       # Interfaces de repositorio
│   ├── Services/         # Interfaces de servicio
│   └── UseCase/          # Interfaces de casos de uso
├── Infraestructure/
│   ├── Controllers/      # Controlador de Palets
│   ├── Persistence/      # DbContext (EF Core)
│   ├── RepositoryImpl/   # Implementaciones de repositorio (llamadas a SPs)
│   ├── ServiceImpl/      # Implementaciones de servicio
│   └── Shared/Exceptions # Middleware global de excepciones
└── scripts/              # Scripts SQL (stored procedures, fixes, pruebas)
```

## Endpoints principales

| Módulo | Ruta base |
|--------|-----------|
| Autenticación | `POST /api/auth/login` |
| Palets | `POST /api/palets/...` |
| Procesos | `POST /api/procesos/...` |
| Guías | `GET/POST /api/guias/...` |
| Catálogos | `POST /api/catalogos/...` |

La documentación completa está disponible en Swagger: `http://localhost:{puerto}/swagger`

## Configuración

### 1. Base de datos

La API utiliza **SQL Server** y se conecta mediante stored procedures. Edita la cadena de conexión en `palets_api/appsettings.json`:

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=TU_SERVIDOR;Database=SistemaPalets;Trusted_Connection=true;TrustServerCertificate=true;"
}
```

### 2. JWT

Reemplaza la clave secreta con un valor aleatorio seguro (mínimo 32 caracteres):

```json
"Jwt": {
    "Key": "TU_CLAVE_SECRETA_ALEATORIA_MINIMO_32_CARACTERES",
    "Issuer": "https://localhost:5050",
    "Audience": "https://localhost:5050"
}
```

> **Recomendación:** usa [User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) en desarrollo o variables de entorno en producción para no exponer credenciales.

### 3. Logs

Los logs se escriben en `C:\LogPaletsAPI\log-.txt` (configurable en `Program.cs`). Asegúrate de que el directorio exista o ajusta la ruta.

## Ejecución

```bash
cd palets_api
dotnet restore
dotnet run
```

## Scripts SQL

La carpeta `scripts/` contiene los stored procedures y scripts de mantenimiento necesarios para la base de datos `SistemaPalets`.
