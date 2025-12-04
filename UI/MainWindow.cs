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

            app.Run(window);
        }
    }
}
