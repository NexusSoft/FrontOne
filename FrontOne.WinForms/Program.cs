using DevExpress.XtraSplashScreen;
using FrontOne.Application.Extensions;
using FrontOne.Application.Services;
using FrontOne.Infrastructure.SapB1.Extensions;
using FrontOne.Infrastructure.SqlServer.Extensions;
using FrontOne.Shared.Configuration;
using FrontOne.Shared.Extensions;
using FrontOne.Shared.Logging;
using FrontOne.Shared.Security;
using FrontOne.WinForms.Configuration;
using FrontOne.WinForms.Forms;
using FrontOne.WinForms.Forms.Sistema;
using FrontOne.WinForms.Session;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Threading;

namespace FrontOne
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            var generalOptions = configuration.GetSection(GeneralOptions.SectionName).Get<GeneralOptions>() ?? new GeneralOptions();
            Log.Logger = SerilogLoggerFactory.CreateLogger(generalOptions);

            try
            {
                var services = new ServiceCollection();

                services.AddSingleton<IConfiguration>(configuration);
                services.AddLogging(builder => builder.AddSerilog(Log.Logger, dispose: false));

                services.AddShared();
                services.AddApplication();
                services.AddSqlServerInfrastructure(configuration);
                services.AddSapB1Infrastructure(configuration);

                services.AddSingleton<IConnectionCredentialStore, RegistryConnectionStore>();
                services.AddSingleton<SessionContext>();
                services.AddSingleton<ICurrentUserProvider>(sp => sp.GetRequiredService<SessionContext>());
                services.AddTransient<LoginForm>();
                services.AddTransient<MainForm>();

                using var serviceProvider = services.BuildServiceProvider();

                ApplicationConfiguration.Initialize();

                // Textos en español para todos los grids DevExpress (panel Buscar, etc.)
                DevExpress.XtraGrid.Localization.GridLocalizer.Active = new GridLocalizerEspanol();

                // Patrón oficial de DevExpress (docs.devexpress.com/WindowsForms/10823): el splash
                // corre en su propio hilo desde ShowForm — bloquear Main() con el trabajo real
                // entre Show/Close es lo esperado, no hace falta Application.DoEvents ni Task.Run.
                // useFadeOut:false — el fade de salida nativo de DevExpress (y su overload de
                // espera) se cuelga en este entorno; el desvanecimiento de salida se hace a mano
                // (SplashCommand.FadeOut, mismo Timer/Opacity que ya anima el fade-in) más abajo.
                SplashScreenManager.ShowForm(typeof(SplashForm), true, false);
                SplashScreenManager.Default.SendCommand(SplashCommand.SetDescripcion, "Iniciando servicios...");
                Thread.Sleep(400);

                SplashScreenManager.Default.SendCommand(SplashCommand.SetDescripcion, "Verificando licencia...");
                var empresaService = serviceProvider.GetRequiredService<EmpresaConfiguracionService>();
                var empresa = empresaService.ObtenerAsync().GetAwaiter().GetResult();
                if (empresa.Logo is { Length: > 0 })
                {
                    SplashScreenManager.Default.SendCommand(SplashCommand.SetLogo, empresa.Logo);
                }

                SplashScreenManager.Default.SendCommand(SplashCommand.SetDescripcion, "Cargando módulos...");
                Thread.Sleep(700); // deja ver el bounce+shimmer del logo (~900ms) y el texto final

                SplashScreenManager.Default.SendCommand(SplashCommand.FadeOut, null!);
                Thread.Sleep(350); // mismo tiempo que SplashForm anima el fade-out (ver DuracionFadeOutMs)
                SplashScreenManager.CloseForm();

                using var loginForm = serviceProvider.GetRequiredService<LoginForm>();
                if (loginForm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                System.Windows.Forms.Application.Run(serviceProvider.GetRequiredService<MainForm>());
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "La aplicación terminó de forma inesperada");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
