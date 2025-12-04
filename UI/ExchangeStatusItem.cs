using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Terminal.Gui.Drawing;
using Terminal.Gui.Input;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;
using TurboBuba.Events;
using TurboBuba.Exchanges;
using TurboBuba.Infrastructure;

namespace TurboBuba.UI
{
    public class ExchangeStatusItem : View
    {
        private int _pingIn = 0; //пинг на получение данных
        private int _pingOut = 0; //пинг на отправку, например, на постановку ордеров
        private ExchangeConnectionStatus _connectionStatus = ExchangeConnectionStatus.Unknown;
        private readonly ExchangesList _exchangeId = ExchangesList.None;
        private EventSubscriber _subscriber = null!;

        private string _exchangeName = String.Empty;

        private Label _mainLabel = null!;
        
        public ExchangeStatusItem(ExchangesList exchandeId)        
        {
            _exchangeId = exchandeId;

            _exchangeName = ExchangeUtils.GetExchangeName(_exchangeId);
            this.Render();
            this.SubscribeEvents();

            this.UpdateLabel();
        }

        private void Render()
        {
            SetScheme(new Scheme()
            {
                Normal = new Terminal.Gui.Drawing.Attribute(StandardColor.White, StandardColor.DimGray),
                Focus = new Terminal.Gui.Drawing.Attribute(StandardColor.Black, StandardColor.LightGray),
                HotNormal = new Terminal.Gui.Drawing.Attribute(StandardColor.Yellow, StandardColor.DimGray),
                HotFocus = new Terminal.Gui.Drawing.Attribute(StandardColor.Yellow, StandardColor.LightGray)
            });

            Width = Dim.Auto(DimAutoStyle.Content);
            Height = 1;
            
            _mainLabel = new Label()
            {
                Text = "Label",
                Width = Dim.Auto(DimAutoStyle.Text),
                Height = 1
            };
            Add(_mainLabel);            
            AddCommand(Command.Select, () => SelectItem());
            MouseEnter += (sender, e) =>
            {
                Logger.Log("ExchangeStatusItem MouseEnter");
                SetScheme(new Scheme()
                {
                    Normal = new Terminal.Gui.Drawing.Attribute(StandardColor.Blue, StandardColor.DimGray),
                    Focus = new Terminal.Gui.Drawing.Attribute(StandardColor.Black, StandardColor.LightGray),
                    HotNormal = new Terminal.Gui.Drawing.Attribute(StandardColor.Yellow, StandardColor.DimGray),
                    HotFocus = new Terminal.Gui.Drawing.Attribute(StandardColor.Yellow, StandardColor.LightGray)
                });


                //Mouse entered the view
                //UpdateTooltip("Hovering over button");
            };

            MouseLeave += (sender, e) =>
            {
                Logger.Log("ExchangeStatusItem MouseLeave");
                SetScheme(new Scheme()
                {
                    Normal = new Terminal.Gui.Drawing.Attribute(StandardColor.White, StandardColor.DimGray),
                    Focus = new Terminal.Gui.Drawing.Attribute(StandardColor.Black, StandardColor.LightGray),
                    HotNormal = new Terminal.Gui.Drawing.Attribute(StandardColor.Yellow, StandardColor.DimGray),
                    HotFocus = new Terminal.Gui.Drawing.Attribute(StandardColor.Yellow, StandardColor.LightGray)
                });

                // Mouse left the view  
                //HideTooltip();
            };

        }

        public bool SelectItem()
        {
            SetFocus();
            Debug.WriteLine("BottomPanel SelectItem()");
            return true;
        }

        private void SubscribeEvents()
        {
            this._subscriber = new EventSubscriber(AppController.Instance.EventBus);
            _subscriber.Subscribe<ExchangeEvents.ConnectionStatusChanged>(OnExchangeConnectionStatusChanged, this);
            _subscriber.Subscribe<ExchangeEvents.LatencyUpdated>(OnExchangeLatencyUpdated, this);
        }   

        private void UpdateLabel()
        {
            string str = "";
            if(_connectionStatus == ExchangeConnectionStatus.Connected)
            {
                str += "🟢 ";
            }            
            else
            {
                str += "🟠 ";
            }
            str += _exchangeName;
            str += $" ({_pingIn}ms | {_pingOut}ms)";

            this._mainLabel.Text = str;
        }

        private void OnExchangeConnectionStatusChanged(ExchangeEvents.ConnectionStatusChanged status)
        {
            //Logger.Log(_exchangeName + " status changed to " + status.Status.ToString());
            _connectionStatus = status.Status;
            this.UpdateLabel();
        }

        private void OnExchangeLatencyUpdated(ExchangeEvents.LatencyUpdated latency)
        {
            _pingIn = latency.LatencyIn;
            _pingOut = latency.LatencyOut;
            this.UpdateLabel() ;
        }
        
    }
}
