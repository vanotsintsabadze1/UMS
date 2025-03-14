using Serilog;
using Serilog.Events;

namespace UMS.API.Infrastructure.Extensions;

public static class LoggingExtensions
{
    private static readonly string _loggingDirName = "Logs";
    
    public static ILoggingBuilder ConfigureLogging(this ILoggingBuilder loggingBuilder)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), _loggingDirName, "log-.txt");
        
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information() // Default minimum level is Information
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .WriteTo
            .Console()
            .WriteTo
            .File(filePath, rollingInterval: RollingInterval.Hour)
            .CreateLogger();

        return loggingBuilder.AddSerilog();
    }
}