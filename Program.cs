// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using Terminal.Gui;
using TurboBuba.UI;
using TurboBuba.Events;



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
                
                .BuildServiceProvider();

            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow?.Show();


        }
    }
}