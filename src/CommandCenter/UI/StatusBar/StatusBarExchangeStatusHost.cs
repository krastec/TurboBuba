using CommandCenter.StatusBar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;

namespace CommandCenter.UI.StatusBar
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.StatusStrip)]
    public class StatusBarExchangeStatusHost : ToolStripControlHost
    {
        public StatusBarExchangeStatusHost() : base(new StatusBarExchangeStatus())
        {
            this.Margin = Padding.Empty;
            this.Padding = Padding.Empty;
            this.AutoSize = false;
        }

        public StatusBarExchangeStatus ItemControl => Control as StatusBarExchangeStatus;

        /*
        // Проксируем нужные свойства
        public string Title
        {
            get => ItemControl.Title;
            set => ItemControl.Title = value;
        }

        public Image Icon
        {
            get => ItemControl.Icon;
            set => ItemControl.Icon = value;
        }
        */
    }
}
