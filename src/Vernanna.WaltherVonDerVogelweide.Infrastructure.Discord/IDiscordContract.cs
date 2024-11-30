namespace Vernanna.WaltherVonDerVogelweide.Infrastructure.Discord;

public interface IDiscordContract
{
    public Stream? AudioStream { get; }

    Task Initialize();

    public Task JoinChannel(ulong channelId);
}