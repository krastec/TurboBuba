

namespace CommandCenter
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            toolBarPanel = new Panel();
            signalConnectionPanel = new CommandCenter.UI.ToolBar.SignalConnectionPanel();
            statusBarPanel = new Panel();
            exchangeStatusPanel1 = new CommandCenter.StatusBar.ExchangeStatusPanel();
            orderBookPanel1 = new CommandCenter.UI.OrderBook.OrderBookPanel();
            toolBarPanel.SuspendLayout();
            statusBarPanel.SuspendLayout();
            SuspendLayout();
            // 
            // toolBarPanel
            // 
            toolBarPanel.BackColor = SystemColors.InactiveBorder;
            toolBarPanel.Controls.Add(signalConnectionPanel);
            toolBarPanel.Dock = DockStyle.Top;
            toolBarPanel.Location = new Point(0, 0);
            toolBarPanel.Name = "toolBarPanel";
            toolBarPanel.Size = new Size(1317, 35);
            toolBarPanel.TabIndex = 3;
            // 
            // signalConnectionPanel
            // 
            signalConnectionPanel.Location = new Point(1, 3);
            signalConnectionPanel.Name = "signalConnectionPanel";
            signalConnectionPanel.Size = new Size(283, 29);
            signalConnectionPanel.TabIndex = 0;
            // 
            // statusBarPanel
            // 
            statusBarPanel.BackColor = SystemColors.InactiveBorder;
            statusBarPanel.Controls.Add(exchangeStatusPanel1);
            statusBarPanel.Dock = DockStyle.Bottom;
            statusBarPanel.Location = new Point(0, 843);
            statusBarPanel.Name = "statusBarPanel";
            statusBarPanel.Size = new Size(1317, 35);
            statusBarPanel.TabIndex = 4;
            // 
            // exchangeStatusPanel1
            // 
            exchangeStatusPanel1.Location = new Point(1, 3);
            exchangeStatusPanel1.Margin = new Padding(0);
            exchangeStatusPanel1.Name = "exchangeStatusPanel1";
            exchangeStatusPanel1.Padding = new Padding(2);
            exchangeStatusPanel1.Size = new Size(110, 29);
            exchangeStatusPanel1.TabIndex = 0;
            // 
            // orderBookPanel1
            // 
            orderBookPanel1.Location = new Point(385, 143);
            orderBookPanel1.Name = "orderBookPanel1";
            orderBookPanel1.Size = new Size(308, 596);
            orderBookPanel1.TabIndex = 5;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1317, 878);
            Controls.Add(orderBookPanel1);
            Controls.Add(statusBarPanel);
            Controls.Add(toolBarPanel);
            Name = "MainWindow";
            Text = "Turbo Buba";
            Load += MainWindow_Load;
            toolBarPanel.ResumeLayout(false);
            statusBarPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Panel toolBarPanel;
        private UI.ToolBar.SignalConnectionPanel toolBarSignalPanel1;
        private UI.ToolBar.SignalConnectionPanel signalConnectionPanel;
        private Panel statusBarPanel;
        private UI.OrderBook.OrderBookPanel orderBookPanel1;
        private StatusBar.ExchangeStatusPanel exchangeStatusPanel1;
    }
}
