using PowerPointToOBSSceneSwitcher.Obs;
using PresentationObsSceneSwitcher.PowerPoint;
using System;
using System.Windows.Forms;

namespace PresentationObsSceneSwitcher
{
    public partial class ConfigurationForm : Form
    {
        #region GUI Behavior fields

        private bool manualClosing;
        private bool showDisplay;

        #endregion

        private readonly JsonSettingsRepository settingsRepository;
        private ObsWebSocketClient connectedClient;

        // TODO: Refactor this Injection
        public ConfigurationForm(JsonSettingsRepository settingsRepository, ObsWebSocketClient connectedClient)
        {
            InitializeComponent();
            this.settingsRepository = settingsRepository;
        }

        #region GUI Behavior events

        protected override void SetVisibleCore(bool value) => base.SetVisibleCore(showDisplay && value);

        private void menuItemClose_Click(object sender, EventArgs e)
        {
            this.manualClosing = true;
            this.notifyIcon.Dispose();
            this.Close();
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.showDisplay = true;
            this.Visible = !this.Visible;
            this.WindowState = FormWindowState.Normal;
        }

        private void ConfigurationForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) this.Hide();
        }

        private void ConfigurationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!manualClosing)
            {
                e.Cancel = true;
                this.Hide();
            }

            connectedClient.Dispose();
        }


        #endregion

        private async void buttonStart_Click(object sender, EventArgs e)
        {
            // TODO: Handle the parse better.
            ObsWebSocketClientSettings settings = new ObsWebSocketClientSettings()
            {
                IpAddress = tbIpAddress.Text,
                Port = int.Parse(tbPort.Text),
                Password = tbPassword.Text
            };
            await settingsRepository.SaveAsync(settings);

            connectedClient?.Dispose();
            connectedClient = new ObsWebSocketClient(settings);
            await connectedClient.ConnectAsync();

            IPresentationSubscriber subscriber = new PowerPointPresentationSubscriber();

            subscriber.Subscribe("OBS", async scene => connectedClient.ChangeScene(scene));
        }
    }
}
