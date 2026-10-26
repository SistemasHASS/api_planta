# API PLANTA (.NET 10)

API de PLANTA sobre el estandar de la plantilla base. Por ahora solo trae el **login con JWT + refresh token**
(apimaestra -> `PLANTA_GetUsuarioAcopio` -> sesion). Puerto de desarrollo: **5100** (la API Go usa 5200).

> Estado: el codigo se escribio sin poder compilar en el entorno de generacion (no hay SDK de .NET).
> Primer paso: `dotnet build` y corregir lo que aparezca.

## Estructura

```
Planta.slnx
Planta.Domain/           CONTRATOS y datos puros. No depende de nadie.
  Entities/                   entidades del negocio
  Dtos/                       requests / responses
  Repository/                 interfaces I...Repository
  UseCase/                    interfaces I...UseCase
  Service/                    interfaces I...Service (JWT, apis externas, usuario actual)
  Constants/  Exceptions/
Planta.Application/      LOGICA de negocio.   Depende de Domain.
  UseCaseImpl/                ...UseCaseImpl
  DependencyInjection.cs      AddApplication()
Planta.Infrastructure/   DETALLES tecnicos.   Depende de Domain.
  Persistence/                DbContext
  RepositoryImpl/             ...RepositoryImpl (stored procedures)
  ServiceImpl/                JWT, cliente de apimaestra
  DependencyInjection.cs      AddInfrastructure()
Planta.Api/              ENTRADA HTTP.        Depende de Domain (+ Application/Infrastructure solo en Program.cs).
  Controller/  Shared/  Extensions/  Startup/  Program.cs
(los scripts SQL estan en ../base-datos: tabla/ y sp/, un archivo por objeto)
docs/                       documentacion (LOGIN.md)
```

### Regla de carpetas
**La interfaz vive en Domain; la implementacion en Application o Infrastructure.**

| Que es | Interfaz (Domain) | Implementacion |
|---|---|---|
| Caso de uso | `UseCase/IXxxUseCase` | `Application/UseCaseImpl/XxxUseCaseImpl` |
| Acceso a BD | `Repository/IXxxRepository` | `Infrastructure/RepositoryImpl/XxxRepositoryImpl` |
| Servicio tecnico / API externa | `Service/IXxxService` | `Infrastructure/ServiceImpl/XxxServiceImpl` |

Flujo: `Controller -> IUseCase -> IRepository / IService`. El controller no tiene logica ni try/catch.

## Como correr

1. Ejecutar en SQL Server: `../base-datos/tabla/PLANTA_SesionesUsuario.sql`, luego los `../base-datos/sp/PLANTA_SesionesUsuario_*.sql` y `../base-datos/sp/PLANTA_GetUsuarioAcopio.sql` (las tablas `PLANTA_usuarioAcopio` y `PLANTA_Acopio_Config` ya existen).
2. Crear la configuracion local. Los `appsettings*.json` **no estan en Git** (llevan credenciales); copiar las plantas y completar:
   ```
   cd Planta.Api
   cp appsettings.example.json appsettings.json
   cp appsettings.Development.example.json appsettings.Development.json
   ```
   Configurar (en esos archivos, con `dotnet user-secrets` o variables de entorno, **nunca subir secretos**):
   - `ConnectionStrings:DefaultConnection`
   - `App:Name` = `PLANTA` (aplicacion registrada en apimaestra)
   - `Jwt:Key` (>= 32 bytes; en no-Development no puede empezar con `CAMBIAR-`)
3. `dotnet run --project Planta.Api` -> Swagger en `http://localhost:5100/swagger`, salud en `/health`.

## Credenciales (nunca en Git)
En Git solo viven las plantas `appsettings*.example.json` (valores `CAMBIAR-...`). Los valores reales se guardan fuera del repositorio:
```
cd Planta.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=...;Database=...;User Id=...;Password=...;TrustServerCertificate=True;"
dotnet user-secrets set "Jwt:Key" "una-clave-larga-de-32-bytes-o-mas"
```
En el servidor se usan variables de entorno: `ConnectionStrings__DefaultConnection`, `Jwt__Key`.

## Crear un modulo nuevo (ej. Producto)

1. `Domain/Entities/Producto.cs` y `Domain/Dtos/...` (request/response).
2. `Domain/Repository/IProductoRepository.cs` y `Domain/UseCase/IProductoUseCase.cs`.
3. `Infrastructure/RepositoryImpl/ProductoRepositoryImpl.cs` (hereda de `BaseRepository`, llama al SP).
4. `Application/UseCaseImpl/ProductoUseCaseImpl.cs` (reglas; lanza `BadRequestException` / `NotFoundException`).
5. `Api/Controller/ProductoController.cs` (solo llama al use case).
6. Registrar: una linea en `Infrastructure/DependencyInjection.cs` y otra en `Application/DependencyInjection.cs`.
7. El SQL en `BD/sp/` (prefijo de la tabla/SP segun el proyecto).

Empresa, RUC y usuario se toman siempre de `ICurrentUser` (JWT), nunca del frontend.

## Maestros (apimaestra)
`POST api/maestros/{fundos|cultivos|campanias|acopios|variedades|clientes|paises|calibres|transporte|tipos-clamshell}` (token; sin cuerpo). Empresa y RUC salen del token; el endpoint y el cuerpo que se envia a apimaestra de cada uno estan en `MaestrosServiceImpl.ConsultarAsync` (mismos que usaba la API anterior) y la respuesta se devuelve sin el envoltorio `{ data }`.
Un maestro nuevo = 1 valor en `MaestroExterno`, 1 caso en `ConsultarAsync` y 1 endpoint en `MaestrosController`.

## Catalogos propios y procesos (base de datos PLANTA)
Cada consulta es un endpoint con su metodo en cada capa (Controller -> UseCase -> Service -> Repository) y devuelve el sobre del SP `{ error, data, mensaje }`. Empresa, RUC, usuario, rol y acopio salen del token.
- Catalogos: `POST api/catalogos/clientes`, `proyecto/{catalogo}` (cuerpo `{"idProyecto"}`), `cultivo/{catalogo}` (cuerpo `{"codigoCultivo"}`), `destinatarios`, `consignatarios`, `variedades`, `acopios` (cuerpo `{"idProyecto"}`), `motivos-traslado`, `establecimiento-emisor`, `establecimientos`, `departamentos`, `provincias` (cuerpo `{"codigoDepartamento"}`) y `distritos` (cuerpo `{"codigoDepartamento","codigoProvincia"}`), `unidades-medida`, `tipos-locacion`, `puertos` y `aeropuertos` (alimentan las guias manuales) (alimentan las guias de remision). Los catalogos por proyecto y por cultivo son los enums de `Planta.Domain/Constants/CatalogoLocal.cs`.
- Procesos: `POST api/procesos/listar` (el administrador ve todos los acopios), `buscar`, `supervisores-disponibles`, `personal-logistico-disponible`, `tipo-proceso-empacado-acopio`, `sincronizar` (crea, cierra, reabre o edita segun `modo`).
- Palets: `POST api/palets/por-proceso`, `sincronizar`, `sincronizar-dpalets`, `dpalets-por-acopio`, `dpalets-por-palet`, `codigos-rancho-por-lugar`, `ficha-composicion` (devuelve el PDF; usa QuestPDF y los logos de `Planta.Api/wwwroot/assets/logos/{ruc}.jpg`) y la matriz de compatibilidad `matriz/destinos`, `matriz/formatos`, `matriz/tipos-empaque-guia`, `matriz/presentaciones`, `matriz/tipos-caja`, `matriz/tipos-clamshell`.
- Guias de remision: `POST api/guias/sincronizar`, `listar`, `exportar-excel` (Excel con EPPlus; solo ADPLA y MOPLA), `obtener` (completa nombres desde apimaestra/catalogos propios), `eliminar`, `editar`, `emitir` y `consultar-estado-sunat` (llaman a ApiDocumentosElectronicos, guardan el estado SUNAT y anulan solas si queda en `RC_05` o `ERROR`), `anular`, `codigos-caja`, `procesos`. Configuracion: `ExternalApis:DocumentosElectronicos:BaseUrl` e `IgnorarErroresSsl` (por defecto `true`, igual que el sistema anterior; ponerlo en `false` cuando esa API tenga certificado valido).
- Guias de remision manuales (sin palets, todo el detalle escrito a mano): `POST api/guias-manuales/sincronizar`, `listar`, `exportar-excel` (solo ADPLA y MOPLA), `obtener`, `editar`, `eliminar`, `emitir` (cierra y envia a SUNAT), `reenviar` (guia CERRADA aun no enviada), `consultar-estado-sunat` y `anular`. Si SUNAT rechaza (`RC_05`) se anula sola. Usa la misma configuracion de ApiDocumentosElectronicos.
- Administracion (listados que el dispositivo descarga): `POST api/administracion/matrices-compatibilidad`, `reglas-sobrepeso` (cuerpo `{"idProyecto","codigoCultivo"}`, ambos opcionales), `correlativos-documentos` y `codigos-caja` (reporte de combinaciones de `PLANTA_CodigosCaja` con nombres de cada código y la descripción completa; SP `PLANTA_ListarCodigosCajaDetalle` + maestros) y `codigos-caja/exportar-excel` (solo administrador; cuerpo `{"ids":[...]}` con los id que se ven en la tabla).
- Reportes (solo administrador ADPLA y monitor MOPLA, pantallas solo online, nada se guarda en Dexie): `POST api/reportes/reporte-diario` (cuerpo `{"fecha","idProyecto","acopios"}`, `acopios` vacio = todos; SP `PLANTA_ReporteDiario`). `POST api/reportes/reporte-semanal/filtros` (cuerpo `{"idProyecto"}`) y `reporte-semanal` (cuerpo `{"idProyecto","semanas","variedades","formatos","destinos","clientes","consignatarios"}`, lista vacia = todos; SP `PLANTA_ReporteSemanal_Filtros` y `PLANTA_ReporteSemanal_Datos`). `POST api/reportes/reporte-campania` (mismo cuerpo y filtros que el semanal; SP `PLANTA_ReporteCampania_Datos`). KG ingresados a acopios: `POST api/reportes/reporte-kg-acopio/empresa-externa` (devuelve `{rucExterno}` segun el parametro `REPORTE_KG_INGRESADOS_EXTERNO`), `reporte-kg-acopio/campanias` (cuerpo `{ruc}`, solo el RUC propio o el externo autorizado) y `reporte-kg-acopio` (cuerpo `{ruc,fecha,idProyecto,codFundo,acopios}`; SP `PLANTA_ReporteKgIngresadosAcopio`). Reporte BH / CAO: `POST api/reportes/reporte-bh/habilitado` (devuelve `{habilitado}` segun el parametro `REPORTE_BH`) y `reporte-diario-propio-externo` (cuerpo `{fecha,idProyecto,acopios,origenes}`, `origenes` = `BH`, `CAO` o vacio para ambos; SP `PLANTA_ReporteDiario_PropioExterno`).
- Los SP `PLANTA_*` ya existian y los usa la aplicacion anterior, por eso NO se modifican: el repositorio solo los llama con sus parametros. Un catalogo nuevo = un valor en el enum + una linea en el `switch` del repositorio.

## Errores
Lanzar excepciones tipadas; `ExceptionMiddleware` las convierte: `UnauthorizedException` 401, `BadRequestException` 400, `NotFoundException` 404, otras 500. Respuesta: `{ "message": "..." }`.

## Pendientes conocidos
- No compilado aun (ver arriba).
- Swagger define el esquema Bearer pero no lo exige por endpoint (igual que Sanidad): el boton Authorize puede no enviar el token. Pendiente `AddSecurityRequirement`.
- Frontend (Angular) y tests: aun no incluidos.

Detalle del login: [docs/LOGIN.md](docs/LOGIN.md).
