using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CommandCenter.UI.OrderBook
{
    public partial class OrderBookPanel : UserControl
    {
        public OrderBookPanel()
        {
            InitializeComponent();

            orderBookCanvas.StartTestDataGeneration(basePrice: 65432.50m);
        }


    }
}
