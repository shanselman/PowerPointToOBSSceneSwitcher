using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PowerPointToOBSSceneSwitcher.Obs;
using PresentationObsSceneSwitcher.PowerPoint;
using System;
using System.Threading.Tasks;

namespace PresentationObsSceneSwitcher
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        //[STAThread]
        static async Task<int> Main(string[] args)
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

            Console.WriteLine("Hi");

            return await app.ExecuteAsync(args);
        }
    }
}
