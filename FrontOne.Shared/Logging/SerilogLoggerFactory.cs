using FrontOne.Shared.Configuration;
using Serilog;
using Serilog.Events;

namespace FrontOne.Shared.Logging;

public static class SerilogLoggerFactory
{
    public static ILogger CreateLogger(GeneralOptions options)
    {
        var logsPath = Path.Combine(AppContext.BaseDirectory, "Logs", $"{options.ApplicationName}-.log");

        return new LoggerConfiguration()
            .MinimumLevel.Is(options.Environment == "Development" ? LogEventLevel.Debug : LogEventLevel.Information)
            .Enrich.WithProperty("Application", options.ApplicationName)
            .WriteTo.File(
                logsPath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();
    }
}
