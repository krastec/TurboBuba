using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using Terminal.Gui;
using Terminal.Gui.App;
using Terminal.Gui.Drawing;
using Terminal.Gui.Input;
using Terminal.Gui.ViewBase;
using TurboBuba.DataFeeds;

namespace TurboBuba.UI
{
    public class BottomPanel : View
    {
        public BottomPanel()
        {
            this.Height = 1;
            this.Width = Dim.Fill();
            this.X = 0;
            this.Y = Pos.AnchorEnd(1);            

            SetScheme(new Scheme()
            {
                Normal = new Terminal.Gui.Drawing.Attribute(StandardColor.White, StandardColor.Charcoal)                
            });
            //AddCommand(Command.Select, () => SelectItem());            
            this.UpdateExchangeStatusItems();
        }

        public bool SelectItem()
        {
            Debug.WriteLine("BottomPanel SelectItem()");
            return true;
        }

        public void UpdateExchangeStatusItems()
        {            
            ExchangeStatusItem exchangeStatusItem = new ExchangeStatusItem(Exchanges.Binance);
            exchangeStatusItem.X = 0;
            exchangeStatusItem.Y = 0;
            this.Add(exchangeStatusItem);
        }


    }
   
}
