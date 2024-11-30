using Discord.Commands;
using Discord.WebSocket;

namespace Vernanna.WaltherVonDerVogelweide.Infrastructure.Discord.CommandHandlers;

public class PlayCommandHandler(IDiscordContract discordContract) : ModuleBase<SocketCommandContext>
{
    [Command("play", RunMode = RunMode.Async)]
    public async Task Handle()
    {
        if (Context.User is SocketGuildUser user)
        {
            if (user.VoiceChannel == null)
            {
                await ReplyAsync("Ich weiß nicht, wo ich auftreten soll...");
            }
            else
            {
                await discordContract.JoinChannel(user.VoiceChannel.Id);
            }
        }
    }
}