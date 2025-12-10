namespace CommandCenter.StatusBar
{
    partial class ExchangeStatusPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            statusIcon = new PictureBox();
            exchangeLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)statusIcon).BeginInit();
            SuspendLayout();
            // 
            // statusIcon
            // 
            statusIcon.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            statusIcon.Image = Properties.Resources.Connection_error;
            statusIcon.Location = new Point(2, 2);
            statusIcon.Name = "statusIcon";
            statusIcon.Size = new Size(24, 25);
            statusIcon.SizeMode = PictureBoxSizeMode.Zoom;
            statusIcon.TabIndex = 0;
            statusIcon.TabStop = false;
            // 
            // exchangeLabel
            // 
            exchangeLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            exchangeLabel.Location = new Point(28, 2);
            exchangeLabel.Name = "exchangeLabel";
            exchangeLabel.Size = new Size(50, 25);
            exchangeLabel.TabIndex = 1;
            exchangeLabel.Text = "Binance";
            exchangeLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // ExchangeStatusPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(exchangeLabel);
            Controls.Add(statusIcon);
            Margin = new Padding(0);
            Name = "ExchangeStatusPanel";
            Padding = new Padding(2);
            Size = new Size(141, 29);
            ((System.ComponentModel.ISupportInitialize)statusIcon).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox statusIcon;
        private Label exchangeLabel;
    }
}
