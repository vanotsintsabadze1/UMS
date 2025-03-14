using Serilog;

namespace UMS.API.Infrastructure.Extensions;

public static class LoggingExtensions
{
    private static readonly string _loggingDirName = "Logs";
    
    public static ILoggingBuilder ConfigureLogging(this ILoggingBuilder loggingBuilder)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), _loggingDirName, "log-.txt");
        
        Log.Logger = new LoggerConfiguration()
            .WriteTo
            .Console()
            .WriteTo
            .File(filePath, rollingInterval: RollingInterval.Hour)
            .CreateLogger();

        return loggingBuilder.AddSerilog();
    }
}