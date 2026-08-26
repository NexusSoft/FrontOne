# Despliegue de FrontOne.Web en IIS

Pasos para publicar el sitio en un servidor Windows con IIS, expuesto a internet detrás de
NAT/firewall (arquitectura elegida en el plan de infraestructura web).

## 1. Requisitos en el servidor

- **IIS** con el módulo **ASP.NET Core Module v2 (ANCM)** instalado — viene con el
  *.NET 10 Hosting Bundle* (`dotnet-hosting-10.x-win.exe`), instalarlo y reiniciar IIS
  (`iisreset`) después.
- Certificado TLS válido para el dominio público del sitio (no el certificado de desarrollo).
- El servidor debe poder alcanzar `172.16.1.100` (SQL Server) y `https://fronterra.vdv.one:50000`
  (SAP Service Layer) en su red interna.

## 2. Login SQL dedicado (una sola vez)

Nunca usar `sa` para el sitio. Ejecutar `Database/Utilidades/Crear_Login_SQL_FrontOneWeb.sql`
contra `172.16.1.100`/`FrontOne` (reemplazando `<PASSWORD_AQUI>` por una contraseña fuerte antes
de ejecutar, y sin dejarla en el archivo ni en el historial de comandos):

```
sqlcmd -S 172.16.1.100 -d FrontOne -U sa -P "<password de sa>" -i Database/Utilidades/Crear_Login_SQL_FrontOneWeb.sql
```

El login `frontone_web` queda con `EXECUTE` sobre los schemas de negocio únicamente — no
`db_owner`, no acceso directo a tablas.

## 3. Publicar el sitio

```
dotnet publish FrontOne.Web/FrontOne.Web.csproj -c Release -o C:\inetpub\frontone-web
```

Copiar el contenido de `C:\inetpub\frontone-web` a la carpeta del sitio en el servidor IIS.

## 4. Configurar el sitio en IIS

1. **Application Pool** nuevo, .NET CLR version = "No Managed Code" (el runtime lo maneja ANCM),
   identidad = una cuenta de servicio dedicada de bajos privilegios — **nunca** una cuenta con
   permisos de administrador local.
2. **Sitio** apuntando a la carpeta publicada, binding HTTPS con el certificado del dominio.
3. **Variables de entorno del App Pool** (IIS Manager → Configuration Editor →
   `system.applicationHost/applicationPools` → el pool → `environmentVariables`, o vía
   `appcmd`/PowerShell `Set-WebConfiguration`):
   - `Sql__Password` = la contraseña real de `frontone_web` (nunca en `appsettings.json`).
   - `Sql__Server`, `Sql__UserId` si difieren de los valores por defecto en `appsettings.json`.
   - `ASPNETCORE_ENVIRONMENT` = `Production`.
4. El `web.config` lo genera automáticamente `dotnet publish` (apunta a
   `FrontOne.Web.dll` vía ANCM, `hostingModel="InProcess"` por defecto) — no hace falta escribirlo
   a mano. Confirmar que `<aspNetCore ... hostingModel="InProcess" />` esté presente.
5. Reiniciar el App Pool y probar `https://<dominio>/login` — debe redirigir correctamente y no
   arrancar si `Sql__Password` no está configurada (guardia en `Program.cs`).

## 5. Cabeceras/red

- `UseForwardedHeaders` en `Program.cs` ya está configurado para leer `X-Forwarded-For`/
  `X-Forwarded-Proto` del proxy — si IIS está detrás de otro proxy/NAT adicional, confirmar que
  ese proxy también reenvíe esas cabeceras, o el rate limiter de `/login` y la auditoría (IP)
  quedarían con la IP del proxy en vez de la del cliente real.
- HSTS (`app.UseHsts()`) solo se activa fuera de `Development` — confirmar que el certificado y el
  binding HTTPS estén listos antes de que los clientes empiecen a recibir esa cabecera (una vez
  que un navegador la cachea, insiste en HTTPS para ese dominio por el tiempo configurado).

## 6. Verificación post-despliegue

- `https://<dominio>/` sin sesión → redirige a `/login`.
- Login con un usuario sin `AccesoWeb` → rechazado con mensaje claro.
- Revisar cabeceras de respuesta (`Content-Security-Policy`, `X-Frame-Options`, `Strict-Transport-Security`)
  con las herramientas de desarrollador del navegador o `curl -I`.
- Confirmar en `Auditoria.Registro` que los logins quedan con la IP real del cliente, no la del
  proxy/NAT.
