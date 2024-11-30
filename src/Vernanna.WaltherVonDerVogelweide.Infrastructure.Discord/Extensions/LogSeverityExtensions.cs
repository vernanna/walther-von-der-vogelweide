using Discord;
using Microsoft.Extensions.Logging;

namespace Vernanna.WaltherVonDerVogelweide.Infrastructure.Discord.Extensions;

public static class LogSeverityExtensions
{
    public static LogLevel ToLogLevel(this LogSeverity logSeverity)
    {
        return logSeverity switch
        {
            LogSeverity.Critical => LogLevel.Critical,
            LogSeverity.Error => LogLevel.Error,
            LogSeverity.Warning => LogLevel.Warning,
            LogSeverity.Info => LogLevel.Information,
            LogSeverity.Verbose => LogLevel.Debug,
            LogSeverity.Debug => LogLevel.Trace,
            _ => LogLevel.None
        };
    }
}