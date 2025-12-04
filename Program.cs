// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using Terminal.Gui;
using TurboBuba.UI;
using TurboBuba.Events;
using TurboBuba.Infrastructure;
using TurboBuba.DataFeeds;



namespace TurboBuba
{
    static class Program
    {
        static void Main()
        {            
            Console.WriteLine("Hello, World!");

            var serviceProvider = new ServiceCollection()
                .AddSingleton<EventBus>()
                .AddSingleton<MainWindow>()
                .AddSingleton<AppController>()

                //ui
                //.AddTransient<ExchangeStatusItem>()
                
                .BuildServiceProvider();

            var appController = serviceProvider.GetService<AppController>();

            Task.Run(async () =>
            {
                await Task.Delay(1000); // Simulate some startup delay
                var binanceController = new BinanceController();
                binanceController.Connect();

            });

            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow?.Show(); 
        }
    }
}