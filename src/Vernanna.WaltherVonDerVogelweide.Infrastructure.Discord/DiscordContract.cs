using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Vernanna.WaltherVonDerVogelweide.Infrastructure.Discord;

public class DiscordContract(ILogger<DiscordContract> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}