using CommandCenter.UI.StatusBar;

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
            button1 = new Button();
            statusStrip1 = new StatusStrip();
            statusBarExchangeStatusHost1 = new StatusBarExchangeStatusHost();
            toolStrip1 = new ToolStrip();
            serverConnectionStatusIcon = new ToolStripLabel();
            serverUrlTextBox = new ToolStripTextBox();
            serverConnectButton = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            statusStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(234, 358);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = SystemColors.Control;
            statusStrip1.Items.AddRange(new ToolStripItem[] { statusBarExchangeStatusHost1 });
            statusStrip1.Location = new Point(0, 853);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1317, 25);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // statusBarExchangeStatusHost1
            // 
            statusBarExchangeStatusHost1.AutoSize = false;
            statusBarExchangeStatusHost1.Margin = new Padding(0);
            statusBarExchangeStatusHost1.Name = "statusBarExchangeStatusHost1";
            statusBarExchangeStatusHost1.Size = new Size(150, 25);
            statusBarExchangeStatusHost1.Text = "statusBarExchangeStatusHost1";
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { serverConnectionStatusIcon, serverUrlTextBox, serverConnectButton, toolStripSeparator1 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1317, 25);
            toolStrip1.TabIndex = 2;
            toolStrip1.Text = "toolStrip1";
            // 
            // serverConnectionStatusIcon
            // 
            serverConnectionStatusIcon.AutoSize = false;
            serverConnectionStatusIcon.Image = Properties.Resources.Connection_error;
            serverConnectionStatusIcon.Name = "serverConnectionStatusIcon";
            serverConnectionStatusIcon.Size = new Size(22, 22);
            // 
            // serverUrlTextBox
            // 
            serverUrlTextBox.Name = "serverUrlTextBox";
            serverUrlTextBox.Size = new Size(150, 25);
            serverUrlTextBox.Text = "https://localhost:5001/pilot";
            // 
            // serverConnectButton
            // 
            serverConnectButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            serverConnectButton.ImageAlign = ContentAlignment.MiddleLeft;
            serverConnectButton.ImageTransparentColor = Color.Magenta;
            serverConnectButton.Name = "serverConnectButton";
            serverConnectButton.Size = new Size(56, 22);
            serverConnectButton.Text = "Connect";
            serverConnectButton.TextImageRelation = TextImageRelation.ImageAboveText;
            serverConnectButton.Click += serverConnectButton_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1317, 878);
            Controls.Add(toolStrip1);
            Controls.Add(statusStrip1);
            Controls.Add(button1);
            Name = "MainWindow";
            Text = "Turbo Buba";
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private ToolStripTextBox serverUrlTextBox;
        private ToolStripButton serverConnectButton;
        private ToolStripSeparator toolStripSeparator1;
        private StatusBarExchangeStatusHost statusBarExchangeStatusHost1;
        private ToolStripLabel serverConnectionStatusIcon;
    }
}
