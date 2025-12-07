using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui.Drawing;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace TurboBuba.UI
{
    public  class OrderBook : View
    {
        private TextView _contractText;

        public OrderBook()
        {
            this.BorderStyle = LineStyle.Single;
            this.Title = "Order Book";

            _contractText = new TextView()
            {
                X = 1,
                Y = 1,
                Width = Dim.Fill() - 2,
                Height = Dim.Fill() - 2,
                ReadOnly = true,
                Text = "Order Book Data Will Appear Here"
            };
            this.Add(_contractText);

        }
    }
}
