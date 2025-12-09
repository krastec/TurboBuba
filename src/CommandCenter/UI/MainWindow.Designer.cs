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
            grpcUrlTextBox = new ToolStripTextBox();
            grpcConnectButton = new ToolStripButton();
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
            toolStrip1.Items.AddRange(new ToolStripItem[] { grpcUrlTextBox, grpcConnectButton, toolStripSeparator1 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1317, 25);
            toolStrip1.TabIndex = 2;
            toolStrip1.Text = "toolStrip1";
            // 
            // grpcUrlTextBox
            // 
            grpcUrlTextBox.Name = "grpcUrlTextBox";
            grpcUrlTextBox.Size = new Size(150, 25);
            grpcUrlTextBox.Text = "https://localhost:5001/";
            // 
            // grpcConnectButton
            // 
            grpcConnectButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            grpcConnectButton.ImageTransparentColor = Color.Magenta;
            grpcConnectButton.Name = "grpcConnectButton";
            grpcConnectButton.Size = new Size(56, 22);
            grpcConnectButton.Text = "Connect";
            grpcConnectButton.TextImageRelation = TextImageRelation.ImageAboveText;
            grpcConnectButton.Click += grpcConnectButton_Click;
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
        private ToolStripTextBox grpcUrlTextBox;
        private ToolStripButton grpcConnectButton;
        private ToolStripSeparator toolStripSeparator1;
        private StatusBarExchangeStatusHost statusBarExchangeStatusHost1;
    }
}
