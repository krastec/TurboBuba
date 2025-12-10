namespace CommandCenter.UI.ToolBar
{
    partial class SignalConnectionPanel
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
            InnerDispose();
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
            urlTextBox = new TextBox();
            connectButton = new Button();
            ((System.ComponentModel.ISupportInitialize)statusIcon).BeginInit();
            SuspendLayout();
            // 
            // statusIcon
            // 
            statusIcon.Image = Properties.Resources.Connection_error;
            statusIcon.Location = new Point(3, 3);
            statusIcon.Name = "statusIcon";
            statusIcon.Size = new Size(20, 20);
            statusIcon.SizeMode = PictureBoxSizeMode.Zoom;
            statusIcon.TabIndex = 0;
            statusIcon.TabStop = false;
            // 
            // urlTextBox
            // 
            urlTextBox.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            urlTextBox.Location = new Point(29, 3);
            urlTextBox.Name = "urlTextBox";
            urlTextBox.Size = new Size(169, 23);
            urlTextBox.TabIndex = 1;
            urlTextBox.Text = "https://localhost:5001/pilot";
            // 
            // connectButton
            // 
            connectButton.Location = new Point(204, 2);
            connectButton.Name = "connectButton";
            connectButton.Size = new Size(75, 23);
            connectButton.TabIndex = 2;
            connectButton.Text = "Connect";
            connectButton.UseVisualStyleBackColor = true;
            connectButton.Click += connectButton_Click;
            // 
            // SignalConnectionPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(connectButton);
            Controls.Add(urlTextBox);
            Controls.Add(statusIcon);
            Name = "SignalConnectionPanel";
            Size = new Size(282, 29);
            ((System.ComponentModel.ISupportInitialize)statusIcon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox statusIcon;
        private TextBox urlTextBox;
        private Button connectButton;
    }
}
