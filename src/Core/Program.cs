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
                await binanceController.Connect();

                binanceController.RegisterContract("BTCUSDT", ContractType.Perp, 100, 1);
                binanceController.SubscribeOrderBook("BTCUSDT", 10, null);

            });

            var builder = WebApplication.CreateBuilder();
            builder.Services.AddGrpc();
            var app = builder.Build();

            // Маршрутизируем gRPC
            app.MapGrpcService<PingPongService>();
            // Дополнительно простой hello endpoint для проверки
            app.MapGet("/", () => "PingPong gRPC server is running.");
            app.Urls.Add("https://localhost:5000"); // слушать на 5000 (HTTP/2 over plaintext)

            app.Run();

            //var mainWindow = serviceProvider.GetService<MainWindow>();
            //mainWindow?.Show(); 
        }
    }
}