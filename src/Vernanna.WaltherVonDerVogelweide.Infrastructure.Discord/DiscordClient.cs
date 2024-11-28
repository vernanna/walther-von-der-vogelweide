using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Vernanna.WaltherVonDerVogelweide.Infrastructure.Discord.Extensions;

namespace Vernanna.WaltherVonDerVogelweide.Infrastructure.Discord;

public class DiscordClient : DiscordSocketClient
{
    private static readonly DiscordSocketConfig SocketConfiguration = new()
    {
        LogLevel = LogSeverity.Info,
        GatewayIntents = GatewayIntents.Guilds |
                         GatewayIntents.GuildMessages |
                         GatewayIntents.MessageContent |
                         GatewayIntents.GuildVoiceStates
    };

    public DiscordClient(ILogger logger) : base(SocketConfiguration)
    {
        Log += log =>
        {
            logger.Log(log.Severity.ToLogLevel(), log.Exception, log.Message);
            return Task.CompletedTask;
        };
    }
}