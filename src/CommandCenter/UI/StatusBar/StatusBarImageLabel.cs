using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;


namespace CommandCenter.UI.StatusBar
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.StatusStrip)]
    public class StatusBarImageLabel : ToolStripControlHost
    {
        private Panel panel;
        private PictureBox pic;
        private Label lbl;

        public StatusBarImageLabel() : base(new Panel())
        {
            panel = Control as Panel;
            panel.AutoSize = false;
            panel.Size = new Size(200, 24);

            pic = new PictureBox { Size = new Size(20, 20), Location = new Point(2, 2), SizeMode = PictureBoxSizeMode.Zoom };
            lbl = new Label { Location = new Point(24, 0), AutoSize = false, Size = new Size(170, 24), TextAlign = ContentAlignment.MiddleLeft };

            panel.Controls.Add(pic);
            panel.Controls.Add(lbl);
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Image Icon
        {
            get => pic.Image;
            set => pic.Image = value;
        }

        public override string Text
        {
            get => lbl.Text;
            set => lbl.Text = value;
        }

        protected override Size DefaultSize => new Size(200, 24);
    }
}