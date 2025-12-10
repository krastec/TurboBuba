using CommandCenter.Events;
using CommandCenter.Infrastructure;
using CommandCenter.Signal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CommandCenter.UI.ToolBar
{
    public partial class SignalConnectionPanel : UserControl
    {
        private AppController _appController;
        private EventSubscriber _eventSubscriber;        
        

        public SignalConnectionPanel()
        {
            InitializeComponent();
        }

        #region Initialization
        private bool _initialized = false;
        public void Initialize(AppController appController)
        {
            if (appController == null) throw new ArgumentNullException(nameof(appController));
            if (_initialized) return;

            // Avoid running at design time
            if (IsInDesignMode())
            {
                _appController = appController;
                _initialized = true;
                return;
            }

            _appController = appController;
            RegisterEventHandlers();
            _initialized = true;
        }
        private bool IsInDesignMode()
        {
            // Reliable check for controls
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode;
        }
        #endregion

        private void connectButton_Click(object sender, EventArgs e)
        {
            string url = urlTextBox.Text;
            _appController.SignalClient.StartAsync(url);
        }

        private void RegisterEventHandlers()
        {
            this._eventSubscriber = new EventSubscriber(_appController.EventBus);
            _eventSubscriber.Subscribe<SignalEvents.ConnectionStatusChanged>(OnSignalConnectionStatusChanged, this);
        }

        private void OnSignalConnectionStatusChanged(SignalEvents.ConnectionStatusChanged evt)
        {            
            if (evt.Status == SignalConnectionStatus.Connected)
            {
                connectButton.Text = "Disconnect";
                connectButton.Enabled = true;
                statusIcon.Image = Properties.Resources.Connection_ok;
            }
            else if (evt.Status == SignalConnectionStatus.Connecting)
            {
                connectButton.Text = "Connecting...";
                connectButton.Enabled = false;
                statusIcon.Image = Properties.Resources.Connection_error;
            }
            else
            {
                connectButton.Text = "Connect";
                connectButton.Enabled = true;
                statusIcon.Image = Properties.Resources.Connection_error;
            }
        }

        private void InnerDispose()
        {
            _eventSubscriber?.Dispose();            
        }

    }
}
