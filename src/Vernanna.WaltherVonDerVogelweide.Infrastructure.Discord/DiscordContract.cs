using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vernanna.WaltherVonDerVogelweide.Infrastructure.Discord.Options;
using Vernanna.WaltherVonDerVogelweide.Shared.Extensions;

namespace Vernanna.WaltherVonDerVogelweide.Infrastructure.Discord;

public class DiscordContract(
    IServiceProvider serviceProvider,
    IOptionsMonitor<DiscordOptions> discordOptions,
    ILogger<DiscordContract> logger)
    : CommandService, IDiscordContract, IAsyncDisposable
{
    private readonly DiscordClient client = new(logger);
    private IAudioClient? audioClient;

    public async Task Initialize()
    {
        await AddModulesAsync(GetType().Assembly, serviceProvider);
        await client.LoginAsync(TokenType.Bot, discordOptions.CurrentValue.Token);
        await client.StartAsync();
        client.MessageReceived += OnMessageReceived;
    }

    public async Task JoinChannel(ulong channelId)
    {
        if (audioClient != null)
        {
            await LeaveChannel();
        }

        try
        {
            var channel = (await client.GetChannelAsync(channelId)).As<SocketVoiceChannel>().ThrowIfNull();
            audioClient = await channel.ConnectAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "An error occured while connecting to voice channel");
            Console.WriteLine(exception);
            throw;
        }

        audioClient.Disconnected += exception =>
        {
            logger.LogError(exception, "Audio client disconnected unexpectedly");
            audioClient.Dispose();
            audioClient = null;
            return Task.CompletedTask;
        };
    }

    public async Task LeaveChannel()
    {
        if (audioClient != null)
        {
            await audioClient.StopAsync();
            audioClient?.Dispose();
            audioClient = null;
        }
    }

    private async Task OnMessageReceived(SocketMessage rawMessage)
    {
        if (rawMessage is not SocketUserMessage message)
        {
            return;
        }

        if (message.Author.IsBot || !message.Content.StartsWith('!'))
        {
            return;
        }

        var commandContext = new SocketCommandContext(client, message);
        var result = await ExecuteAsync(commandContext, commandContext.Message.Content[1..], serviceProvider);
        if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
        {
            logger.LogError("An error occured while executing a command: {Error}", result.ErrorReason);
        }
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        await LeaveChannel();
        client.MessageReceived -= OnMessageReceived;
        await client.DisposeAsync();
        base.Dispose(true);
    }
}