using PowerPointToOBSSceneSwitcher.Obs;
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

        private readonly ObsWebSocketClientSettings settings;
        private ObsWebSocketClient client;

        // TODO: Refactor this Injection
        public ConfigurationForm(ObsWebSocketClientSettings settings, ObsWebSocketClient client)
        {
            InitializeComponent();
            this.settings = settings ?? new ObsWebSocketClientSettings();
            this.client = client;
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
        }


        #endregion

        private async void buttonStart_Click(object sender, EventArgs e)
        {
            // TODO: Handle the parse better when bindingsource support come.
            settings.IpAddress = tbIpAddress.Text;
            settings.Port = int.Parse(tbPort.Text);
            settings.Password = tbPassword.Text;

            await client.ConnectAsync(settings);
        }
    }
}
