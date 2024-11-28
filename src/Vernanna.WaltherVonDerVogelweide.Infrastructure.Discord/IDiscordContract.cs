namespace Vernanna.WaltherVonDerVogelweide.Infrastructure.Discord;

public interface IDiscordContract
{
    Task Initialize();

    public Task JoinChannel(ulong channelId);
}