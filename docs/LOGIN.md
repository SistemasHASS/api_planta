# Login

## Endpoints (`api/auth`)

| Metodo | Ruta | Acceso | Descripcion |
|---|---|---|---|
| POST | `/login` | anonimo | usuario + password + datos del dispositivo -> tokens |
| POST | `/refresh` | anonimo | refresh token -> nuevo access y refresh (rotacion) |
| POST | `/logout` | anonimo | revoca la sesion del refresh token |
| GET | `/me` | token | datos del usuario segun el JWT |
| GET | `/sessions` | token | sesiones activas del usuario |
| GET | `/sessions/{id}` | token | cierra una sesion |
| POST | `/logout-all` | rol `Roles.Admin` | cierra todas las sesiones de la empresa |

Todo endpoint nuevo exige token por defecto; solo `[AllowAnonymous]` lo abre.

## Flujo
1. `POST /login` -> `AuthUseCaseImpl` valida usuario y clave en **apimaestra** (`/api/Maestros/get-usuarios`, aplicacion `PLANTA` desde `App:Name`).
   Sin usuario -> 401. Se toma el primer usuario devuelto (igual que el login anterior de PLANTA).
2. El usuario de apimaestra se envia como JSON (sin camelCase) al SP `PLANTA_GetUsuarioAcopio`, que devuelve sus datos de acopio
   (`codigoAcopio`, `acopioNombre`, `serieGuia`). Si el SP devuelve error o vacio -> 401.
3. Se crea una sesion en `PLANTA_SesionesUsuario` (guarda el **hash SHA-256** del refresh token) y se devuelve
   access token (JWT, 4 h por defecto) + refresh token (8 h) + tope de sesion (1 dia).
4. El frontend envia `Authorization: Bearer <access>`. Al vencer llama a `/refresh`; el refresh anterior se invalida (rotacion).
5. Pasado el tope absoluto de sesion hay que loguearse de nuevo (claim `SesionExpiraUtc`, validado en cada peticion).

## Claims del JWT
`NameIdentifier`, `Name`, `Role`, `NombreCompleto`, `DocumentoIdentidad`, `IdEmpresa`, `Ruc`, `IdProyecto`, `SesionExpiraUtc`, `Aplicacion`, `CodigoAcopio`, `AcopioNombre`, `SerieGuia`.
Se leen en el codigo con `ICurrentUser`.

## Configuracion (`Jwt`)
`Key`, `Issuer`, `Audience`, `AccessTokenHours|Minutes`, `RefreshTokenHours|Minutes`, `SessionMaxHours|Minutes`.

## Piezas
- `Api/Controller/AuthController` · `Api/Extensions/JwtExtensions` · `Api/Shared/CurrentUser`
- `Domain/UseCase/IAuthUseCase`, `Repository/IAuthRepository`, `Service/ITokenService`, `IMaestrosService`
- `Application/UseCaseImpl/AuthUseCaseImpl`
- `Infrastructure/RepositoryImpl/AuthRepositoryImpl` (SP `PLANTA_SesionesUsuario_*`), `ServiceImpl/JwtTokenServiceImpl`, `MaestrosServiceImpl`, `UsuarioAcopioRepositoryImpl` (SP `PLANTA_GetUsuarioAcopio`)
- `base-datos/tabla/PLANTA_SesionesUsuario.sql` y `base-datos/sp/PLANTA_SesionesUsuario_*.sql` (un archivo por SP)

## Roles (`Domain/Constants/Roles.cs`)
`ADPLA` (admin), `LOPLA`, `MOPLA`, `GMPLA`, `COPLA`, `OPPLA`.
