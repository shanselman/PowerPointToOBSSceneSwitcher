using PowerPointToOBSSceneSwitcher.Obs;
using PresentationObsSceneSwitcher.PowerPoint;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationObsSceneSwitcher
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            JsonSettingsRepository settingsRepository = new JsonSettingsRepository();
            ObsWebSocketClient connectedClient = await ConnectWithSavedSettings(settingsRepository).ConfigureAwait(false);

            Application.Run(new ConfigurationForm(settingsRepository, connectedClient));
        }

        private static async Task<ObsWebSocketClient> ConnectWithSavedSettings(JsonSettingsRepository settingsRepository)
        {
            ObsWebSocketClientSettings savedSettings = await settingsRepository.LoadAsync();

            if (!(savedSettings is null))
            {
                ObsWebSocketClient connectedClient = new ObsWebSocketClient(savedSettings);
                await connectedClient.ConnectAsync();

                IPresentationSubscriber subscriber = new PowerPointPresentationSubscriber();
                subscriber.Subscribe("OBS", async scene => connectedClient.ChangeScene(scene));

                return connectedClient;
            }

            return null;
        }
    }
}
