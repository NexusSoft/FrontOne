using System.Globalization;
using System.Threading.RateLimiting;
using FrontOne.Application.Extensions;
using FrontOne.Infrastructure.SapB1.Extensions;
using FrontOne.Infrastructure.SqlServer.Extensions;
using FrontOne.Shared.Configuration;
using FrontOne.Shared.Extensions;
using FrontOne.Shared.Logging;
using FrontOne.Web.Components;
using FrontOne.Web.Configuration;
using FrontOne.Web.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#if DEBUG
// User Secrets se agrega automáticamente solo cuando ASPNETCORE_ENVIRONMENT=Development; lo
// declaramos explícito aquí para que la carga no dependa de qué perfil de lanzamiento use Visual
// Studio (IIS Express, Kestrel, etc.) — en builds Debug siempre se intenta leer, sin fallar si el
// archivo de secrets no existe todavía.
builder.Configuration.AddUserSecrets<Program>(optional: true);
#endif

// ---------------------------------------------------------------------------------------------
// Configuración + logging (mismo patrón que FrontOne.WinForms/Program.cs).
// ---------------------------------------------------------------------------------------------
var generalOptions = builder.Configuration.GetSection(GeneralOptions.SectionName).Get<GeneralOptions>() ?? new GeneralOptions();
Log.Logger = SerilogLoggerFactory.CreateLogger(generalOptions);
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", generalOptions.ApplicationName)
    .WriteTo.File(
        Path.Combine(AppContext.BaseDirectory, "Logs", $"{generalOptions.ApplicationName}-.log"),
        rollingInterval: Serilog.RollingInterval.Day,
        retainedFileCountLimit: 30,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}"));

// ---------------------------------------------------------------------------------------------
// Guardia de seguridad no negociable: la contraseña SQL nunca viaja en appsettings.json del
// repositorio. Debe llegar por variable de entorno (Sql__Password, típicamente configurada en el
// App Pool de IIS) o por User Secrets en desarrollo (dotnet user-secrets set "Sql:Password" "...").
// Si no está presente, la aplicación se niega a arrancar en vez de conectarse con una cadena vacía.
// ---------------------------------------------------------------------------------------------
var sqlPassword = builder.Configuration[$"{SqlOptions.SectionName}:Password"];
if (string.IsNullOrEmpty(sqlPassword) && !builder.Configuration.GetValue("Sql:IntegratedSecurity", false))
{
    Log.Fatal("La contraseña SQL no está configurada. Defínela con la variable de entorno Sql__Password o con User Secrets (dotnet user-secrets set \"Sql:Password\" \"...\") — nunca en appsettings.json.");
    throw new InvalidOperationException("Falta configurar Sql:Password fuera del repositorio (variable de entorno o User Secrets).");
}

// ---------------------------------------------------------------------------------------------
// Reutilización íntegra de las capas existentes — cero servicios duplicados respecto a WinForms.
// ---------------------------------------------------------------------------------------------
builder.Services.AddShared();
builder.Services.AddApplication();
builder.Services.AddSqlServerInfrastructure(builder.Configuration);
builder.Services.AddSapB1Infrastructure(builder.Configuration);
builder.Services.AddWeb();

// ---------------------------------------------------------------------------------------------
// UI: Blazor Web App con render mode InteractiveServer global + DevExpress Blazor.
// ---------------------------------------------------------------------------------------------
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddDevExpressBlazor();

var culturaFija = new CultureInfo("es-MX");
CultureInfo.DefaultThreadCurrentCulture = culturaFija;
CultureInfo.DefaultThreadCurrentUICulture = culturaFija;

// ---------------------------------------------------------------------------------------------
// Autenticación por cookie — mismas tablas/SPs de Seguridad.Usuario que WinForms.
// ---------------------------------------------------------------------------------------------
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "__Host-FrontOne";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/acceso-denegado";
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.Redirect("/acceso-denegado");
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization(options =>
{
    // Ninguna página queda pública por olvido: sin [AllowAnonymous] explícito, hace falta sesión.
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddAntiforgery();

// ---------------------------------------------------------------------------------------------
// Anti fuerza bruta: 5 intentos por IP cada 5 minutos sobre /login (el bloqueo de cuenta tras 5
// fallos consecutivos vive en Seguridad.Usuario.IntentosFallidos/BloqueadoHasta, ver LoginEndpoints).
// ---------------------------------------------------------------------------------------------
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("login", httpContext => RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "sin-ip",
        factory: _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 5,
            Window = TimeSpan.FromMinutes(5),
            QueueLimit = 0,
        }));
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    // Necesario detrás de IIS para que RemoteIpAddress (rate limiter) y el esquema (https)
    // reflejen al cliente real, no al proxy interno.
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

app.UseForwardedHeaders();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseFrontOneSecurityHeaders();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

FrontOne.Web.Security.LoginEndpoints.Map(app);

try
{
    Log.Information("FrontOne.Web iniciando…");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "FrontOne.Web terminó de forma inesperada");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
