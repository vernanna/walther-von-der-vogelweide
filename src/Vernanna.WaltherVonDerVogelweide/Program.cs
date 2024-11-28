using Vernanna.WaltherVonDerVogelweide.Infrastructure.Discord;
using Vernanna.WaltherVonDerVogelweide.Infrastructure.Discord.libs;
using Vernanna.WaltherVonDerVogelweide.Infrastructure.Discord.Options;

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Services.Configure<DiscordOptions>(builder.Configuration.GetSection("Discord"));
builder.Services.AddSingleton<IDiscordContract, DiscordContract>();

LibraryLoader.LoadOpus();

var host = builder.Build();
await host.Services.GetRequiredService<IDiscordContract>().Initialize();

host.Run();