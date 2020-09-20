using McMaster.Extensions.CommandLineUtils;
using PowerPointToOBSSceneSwitcher.Obs;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationObsSceneSwitcher
{
    [Command]
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

        public CommandLineApp(Func<ObsWebSocketClientSettings, ConfigurationForm> formFactory,
            JsonSettingsRepository settingsRepository, ObsWebSocketClient client,
            IPresentationSubscriber subscriber)
        {
            this.formFactory = formFactory;
            this.settingsRepository = settingsRepository;
            this.client = client;
            this.subscriber = subscriber;
        }

        public async Task OnExecuteAsync()
        {
            ObsWebSocketClientSettings settings = await ReadSettings().ConfigureAwait(false);
            await client.ConnectAsync(settings).ConfigureAwait(false);

            /* Suscribe the client. */
            subscriber.Subscribe("OBS", async scene =>
            {
                if (client.IsConnected) await client.ChangeSceneAsync(scene);
            });

            if (!NoGui)
            {
                RunGui(settings);

                /* Save if running GUI. */
                await settingsRepository.SaveAsync(settings);
            }
        }

        private async Task<ObsWebSocketClientSettings> ReadSettings()
        {
            ObsWebSocketClientSettings settings = (await settingsRepository.LoadAsync())
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
