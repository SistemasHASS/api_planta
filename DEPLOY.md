# Guia de Despliegue a Produccion (IIS Windows Server)

## URLs de Produccion

- **API:** `https://apiplanta.agroapps.net:7615`
- **Angular:** `https://planta.agroapps.net:7616`

---

## 1. Requisitos en el Servidor Windows

1. Instalar **ASP.NET Core Hosting Bundle** (.NET Runtime + IIS Module):
   - Descargar desde: `https://dotnet.microsoft.com/download/dotnet`
   - Instalar la version que corresponda a tu proyecto (ej. .NET 8 o .NET 9)
2. Reiniciar IIS despues de instalar:
   ```cmd
   iisreset
   ```
3. Instalar **URL Rewrite Module** (necesario para Angular SPA):
   - Descargar desde Microsoft oficial

---

## 2. API .NET — Publicar y Desplegar

### A. Publish desde este workspace

```bash
dotnet publish Planta.Api/Planta.Api.csproj -c Release -o ./publish/api
```

Esto genera la carpeta `publish/api/` con todo lo necesario.

### B. Configurar `appsettings.Production.json`

El archivo `Planta.Api/appsettings.Production.json` ya fue creado. **Debes editarlo en el servidor** y reemplazar estos valores reales:

- `ConnectionStrings:DefaultConnection` → tu cadena de conexion SQL Server real
- `Jwt:Key` → tu secret key de al menos 32 caracteres (debe coincidir con el que usa tu app Angular para generar tokens)

### C. En IIS (Servidor Windows)

1. **Application Pool** → Add:
   - **Name:** `PlantaApiPool`
   - **.NET CLR version:** `No Managed Code`
   - **Managed pipeline mode:** `Integrated`

2. **Sites** → Add Website:
   - **Site name:** `apiplanta`
   - **Application pool:** `PlantaApiPool`
   - **Physical path:** `C:\inetpub\apiplanta` (copia aqui el contenido de `publish/api`)
   - **Binding:**
     - Type: `https`
     - IP: `*` (o la IP del servidor)
     - Port: `7615`
     - Host name: `apiplanta.agroapps.net`
     - SSL certificate: seleccionar tu certificado

3. **Permisos de escritura:**
   - La carpeta `C:\inetpub\apiplanta\Logs` necesita permisos de escritura para el usuario del Application Pool (`IIS AppPool\PlantaApiPool` o la identity que uses).

---

## 3. Angular — Build y Desplegar

### A. Configurar entorno de produccion

En tu proyecto Angular, asegurate que `src/environments/environment.prod.ts` apunte a la API:

```typescript
export const environment = {
  production: true,
  apiUrl: 'https://apiplanta.agroapps.net:7615/api'
};
```

### B. Build de produccion

```bash
ng build --configuration production
```

### C. En IIS (Servidor Windows)

1. **Application Pool** → Add:
   - **Name:** `PlantaAngularPool`
   - **.NET CLR version:** `No Managed Code`

2. **Sites** → Add Website:
   - **Site name:** `planta`
   - **Application pool:** `PlantaAngularPool`
   - **Physical path:** `C:\inetpub\planta` (copia aqui los archivos de `dist/...`)
   - **Binding:**
     - Type: `https`
     - Port: `7616`
     - Host name: `planta.agroapps.net`
     - SSL certificate: seleccionar tu certificado

3. **URL Rewrite para SPA routing:**

   Crear un archivo `web.config` dentro de `C:\inetpub\planta\`:

   ```xml
   <?xml version="1.0" encoding="utf-8"?>
   <configuration>
     <system.webServer>
       <rewrite>
         <rules>
           <rule name="Angular Routes" stopProcessing="true">
             <match url=".*" />
             <conditions logicalGrouping="MatchAll">
               <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
               <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
             </conditions>
             <action type="Rewrite" url="/index.html" />
           </rule>
         </rules>
       </rewrite>
       <staticContent>
         <mimeMap fileExtension=".json" mimeType="application/json" />
       </staticContent>
     </system.webServer>
   </configuration>
   ```

---

## 4. CORS

La API ya lee los orígenes permitidos desde `appsettings.Production.json` bajo la clave `Cors:AllowedOrigins`.

Asegurate que contenga la URL de tu Angular:

```json
{
  "Cors": {
    "AllowedOrigins": [
      "https://planta.agroapps.net:7616"
    ]
  }
}
```

---

## 5. Cookies / JWT Cross-Origin

Tu API y Angular estan en **subdominios diferentes** (`apiplanta.agroapps.net` vs `planta.agroapps.net`). Esto significa que las cookies se consideran **cross-origin**. Ya modifique el `AuthController` para que en produccion las cookies tengan:

- `Secure = true` (solo se envian por HTTPS)
- `SameSite = None` (se envian cross-subdomain)
- `HttpOnly = true` (no accesibles desde JavaScript)

### A. En tu Angular (HttpClient)

Tu app Angular **debe** enviar las cookies al hacer requests a la API. Esto se hace con `withCredentials: true`.

Si usas un interceptor global en Angular:

```typescript
import { HttpInterceptorFn } from '@angular/common/http';

export const credentialsInterceptor: HttpInterceptorFn = (req, next) => {
  const cloned = req.clone({ withCredentials: true });
  return next(cloned);
};
```

O en cada request individual:

```typescript
this.http.get('https://apiplanta.agroapps.net:7615/api/...', { withCredentials: true })
```

> **IMPORTANTE:** Si no configuras `withCredentials: true`, el navegador **no enviara** la cookie `access_token` y todos los endpoints protegidos con `[Authorize]` devolveran `401`.

### B. Flujo en produccion

1. Usuario hace login en `https://planta.agroapps.net:7616`
2. Angular llama `POST https://apiplanta.agroapps.net:7615/api/auth/login`
3. El backend valida credenciales y responde con:
   - Body JSON: `{ user: { ... } }`
   - Header `Set-Cookie: access_token=...; HttpOnly; Secure; SameSite=None`
4. El navegador almacena la cookie
5. En cada request posterior, el navegador envia automaticamente `Cookie: access_token=...` (gracias a `withCredentials: true`)
6. El middleware JWT de la API lee la cookie y autentica la peticion

---

## 6. Checklist Final

- [ ] Hosting Bundle instalado y IIS reiniciado
- [ ] `appsettings.Production.json` editado con cadena de conexion real
- [ ] `Jwt:Key` configurado (minimo 32 caracteres, igual al frontend)
- [ ] Carpeta `Logs` con permisos de escritura para IIS
- [ ] Angular `environment.prod.ts` apuntando a `https://apiplanta.agroapps.net:7615/api`
- [ ] Angular `HttpClient` configurado con `withCredentials: true`
- [ ] `web.config` con URL Rewrite en la carpeta del Angular
- [ ] Certificados SSL configurados en ambos bindings IIS (ambos usan HTTPS)
- [ ] Puerto 7615 y 7616 abiertos en el firewall de Windows (si aplica)
