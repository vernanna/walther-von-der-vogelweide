using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;
using FFMpegCore;
using FFMpegCore.Pipes;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace Vernanna.WaltherVonDerVogelweide.Infrastructure.Discord.CommandHandlers;

public class PlayCommandHandler(IDiscordContract discordContract) : ModuleBase<SocketCommandContext>
{
    [Command("play", RunMode = RunMode.Async)]
    public async Task Handle()
    {
        if (discordContract.AudioStream == null)
        {
            await ReplyAsync("Ich gebe gerade kein Konzert");
            return;
        }

        var youtube = new YoutubeClient();
        var video = await youtube.Videos.GetAsync("https://www.youtube.com/watch?v=jvU4xWsN7-A");
        var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
        var audioStreamInfo = streamManifest.GetAudioOnlyStreams().FirstOrDefault(s => s.Container == Container.Mp4);;

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (audioStreamInfo != null)
        {
            await using var inputStream = await youtube.Videos.Streams.GetAsync(audioStreamInfo);
            try
            {
                using (var fileStream = new FileStream("debug_input.mp4", FileMode.Create, FileAccess.Write))
                {
                    await inputStream.CopyToAsync(fileStream);
                }
                using var debugStream = new FileStream("debug_audio.pcm", FileMode.Create, FileAccess.Write);
                using var bufferedStream = new BufferedStream(inputStream);
                await FFMpegArguments
                    .FromPipeInput(new StreamPipeSource(bufferedStream), options => options.WithCustomArgument("-f mp4"))
                    .OutputToPipe(new StreamPipeSink(discordContract.AudioStream), options => options
                        .WithCustomArgument("-ac 2")
                        .WithCustomArgument("-ar 48000")
                        .WithCustomArgument("-f s16le")
                        .WithAudioCodec("pcm_s16le"))
                    .NotifyOnOutput(line => Console.WriteLine($"FFmpeg: {line}"))
                    .ProcessAsynchronously();
            }
            finally
            {
                await discordContract.AudioStream.FlushAsync();
            }
        }
    }
}