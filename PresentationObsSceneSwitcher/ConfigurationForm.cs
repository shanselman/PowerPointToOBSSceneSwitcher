using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationObsSceneSwitcher
{
    public partial class ConfigurationForm : Form
    {
        #region GUI Behavior fields

        private bool manualClosing;
        private bool showDisplay;

        #endregion

        public ConfigurationForm()
        {
            InitializeComponent();
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


    }
}
