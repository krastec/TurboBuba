// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using Terminal.Gui;
using TurboBuba.UI;
using TurboBuba.Events;
using TurboBuba.Infrastructure;
using TurboBuba.Exchanges;
using Microsoft.AspNetCore.Builder;
using TurboBuba.Grpc;
using TurboBuba.Status.V1;



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

                .AddSingleton<GrpcServer>()
                //ui
                //.AddTransient<ExchangeStatusItem>()

                .BuildServiceProvider();

            var appController = serviceProvider.GetService<AppController>();

            Task.Run(async () =>
            {
                await Task.Delay(1000); // Simulate some startup delay
                var binanceController = new BinanceController();
                await binanceController.Connect();

                binanceController.RegisterContract("BTCUSDT", ContractType.Perp, 100, 1000, 1);
                binanceController.SubscribeOrderBook("BTCUSDT", 10, null);

            });

            var grpcServer = serviceProvider.GetService<GrpcServer>();
            grpcServer.Run();

            //Task.Run(async () =>
            //{
            //var grpcServer = new GrpcServer();
            //grpcServer.Run();

            //});



            //var mainWindow = serviceProvider.GetService<MainWindow>();
            //mainWindow?.Show(); 
        }
    }
}