using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PowerPointToOBSSceneSwitcher.Obs;
using PresentationObsSceneSwitcher;
using PresentationObsSceneSwitcher.PowerPoint;
using System;
using System.Threading.Tasks;

namespace PresentationObsSceneSwitcherConsole
{
    internal static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            ServiceProvider services = new ServiceCollection().AddLogging(builder => builder.AddConsole().AddDebug())
                .AddScoped<IPresentationSubscriber, PowerPointPresentationSubscriber>()
                .AddSingleton<ObsWebSocketClient>()
                .AddScoped<JsonSettingsRepository>()
                .AddScoped<Func<ObsWebSocketClientSettings, ConfigurationForm>>(ctx =>
                    settings => new ConfigurationForm(settings, ctx.GetRequiredService<ObsWebSocketClient>()))
                .BuildServiceProvider();

            CommandLineApplication<CommandLineApp> app = new CommandLineApplication<CommandLineApp>();
            app.Conventions.UseDefaultConventions().UseConstructorInjection(services);

            return await app.ExecuteAsync(args);
        }
    }
}
