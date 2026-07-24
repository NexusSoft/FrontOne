using FrontOne.Application.Extensions;
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
