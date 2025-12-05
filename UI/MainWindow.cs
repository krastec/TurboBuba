using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace TurboBuba.UI
{
    internal class MainWindow
    {
        private readonly IServiceProvider _serviceProvider;

        public MainWindow(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;            
        }

        public void Show()
        {
            using IApplication app = Application.Create();
            app.Init();

            using Window window = new() { Title = "Hello World (Esc to quit)" };
            Label label = new()
            {
                Text = "Hello, Terminal.Gui v2!",
                X = Pos.Center(),
                Y = Pos.Center()
            };

            BottomPanel bottomPanel = new();
            window.Add(label);
            window.Add(bottomPanel);

            OrderBook orderBook = new();
            orderBook.Width = Dim.Percent(50);
            orderBook.Height = Dim.Fill() - 1;
            window.Add(orderBook);

            /*
            // Key handler: Ctrl+Shift+H toggles visibility
            window.KeyDown += (s, args) =>
            {                
                // Check Ctrl+Shift+H
                var isCtrl = args.IsCtrl;
                var isShift = args.IsShift;
                var baseKey = args.KeyCode & Terminal.Gui.Drivers.KeyCode.CharMask;

                // Compare to 'H' (uppercase)
                var isH = baseKey == Terminal.Gui.Drivers.KeyCode.H;
                Console.WriteLine($"KeyDown: Ctrl={isCtrl}, Shift={isShift}, KeyCode={args.KeyCode}, IsH={isH}");
                if (isCtrl && isShift && isH)
                {
                    window.Visible = !window.Visible;
                    // Avoid redraw when hidden
                    //window.SetNeedsDisplay(window.Visible);
                    args.Handled = true;
                }
            };
            */

            app.Run(window);
        }
    }
}
