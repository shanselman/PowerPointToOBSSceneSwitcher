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

            // TODO: Maybe handle this with DI.
            /* Read settings */
            JsonSettingsRepository settingsRepository = new JsonSettingsRepository();
            ObsWebSocketClientSettings settings = await settingsRepository.LoadAsync();

            /* Init the OBS client */
            using (ObsWebSocketClient client = new ObsWebSocketClient())
            {
                /* Try initial connection if there are saved settings */
                if (!(settings is null)) await client.ConnectAsync(settings);

                /* Suscribe the client. */
                IPresentationSubscriber subscriber = new PowerPointPresentationSubscriber();
                subscriber.Subscribe("OBS", async scene =>
                {
                    if (client.IsConnected)
                        client.ChangeScene(scene);
                });

                Application.Run(new ConfigurationForm(settings, client));
            }

            await settingsRepository.SaveAsync(settings);
        }
    }
}
