using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;
using FFMpegCore;
using FFMpegCore.Pipes;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace Vernanna.WaltherVonDerVogelweide.Infrastructure.Discord.CommandHandlers;

public class JoinCommandHandler(IDiscordContract discordContract) : ModuleBase<SocketCommandContext>
{
    [Command("join", RunMode = RunMode.Async)]
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