using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using PowerPointToOBSSceneSwitcher.Obs;
using PresentationObsSceneSwitcherConsole;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationObsSceneSwitcher
{
    public class CommandLineApp
    {
        [Option(Description = "Run as console app", ShortName = "cmd")]
        public bool NoGui { get; } = false;

        [Option(Description = "IpAddress", ShortName = "ip")]
        public string IpAddress { get; }

        [Option(Description = "Port", ShortName = "p")]
        public int? Port { get; }

        [Option(Description = "Optional Password", ShortName = "pass")]
        public string Password { get; }

        private readonly Func<ObsWebSocketClientSettings, ConfigurationForm> formFactory;
        private readonly JsonSettingsRepository settingsRepository;
        private readonly ObsWebSocketClient client;
        private readonly IPresentationSubscriber subscriber;
        private readonly ILogger<CommandLineApp> logger;

        public CommandLineApp(Func<ObsWebSocketClientSettings, ConfigurationForm> formFactory,
            JsonSettingsRepository settingsRepository, ObsWebSocketClient client,
            IPresentationSubscriber subscriber, ILogger<CommandLineApp> logger)
        {
            this.formFactory = formFactory;
            this.settingsRepository = settingsRepository;
            this.client = client;
            this.subscriber = subscriber;
            this.logger = logger;
        }

        public async Task OnExecuteAsync()
        {
            ObsWebSocketClientSettings settings = await ReadSettings().ConfigureAwait(false);

            logger.LogInformation("Try to connect using Address: {IpAddress}:{Port}", settings.IpAddress, settings.Port);

            try
            {
                await client.ConnectAsync(settings).ConfigureAwait(false);
                logger.LogInformation("Connected");
            }
            catch (Exception ex)
            {
                logger.LogError("Cannot connect: {Message}", ex.Message);
            }

            /* Suscribe the client. */
            subscriber.Subscribe("OBS", async command =>
            {
                logger.LogTrace("Received command: {Command}", command);

                if (command == "**START")
                    await client.StartRecordingAsync();
                else if (command == "**STOP")
                    await client.StopRecordingAsync();
                else if (string.IsNullOrEmpty(command))
                    await client.ChangeSceneAsync(command);
            });


            if (!NoGui)
            {
                logger.LogInformation("Runnig with GUI");
                ConsoleWindowController.Hide();
                RunGui(settings);

                /* Save if running GUI. */
                await settingsRepository.SaveAsync(settings);
            }
            else
            {
                logger.LogInformation("Runnig without GUI");
                Console.ReadKey();
            }
        }

        private async Task<ObsWebSocketClientSettings> ReadSettings()
        {
            ObsWebSocketClientSettings settings = (await settingsRepository.LoadAsync().ConfigureAwait(false))
                            ?? new ObsWebSocketClientSettings();

            if (!string.IsNullOrEmpty(IpAddress))
                settings.IpAddress = IpAddress;

            if (Port.HasValue)
                settings.Port = Port.Value;

            if (!string.IsNullOrEmpty(Password))
                settings.Password = Password;

            return settings;
        }

        private void RunGui(ObsWebSocketClientSettings settings)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(formFactory(settings));
        }
    }
}
