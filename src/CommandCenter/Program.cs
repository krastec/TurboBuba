using CommandCenter.Events;
using CommandCenter.Infrastructure;
using CommandCenter.Signal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.Logging;
using Serilog;
using Serilog.Core;
using System.Net.NetworkInformation;

namespace CommandCenter
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs\\commandcenter.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            //logger.Information("Hello, Serilog!");

            var serviceProvider = new ServiceCollection()               
                .AddSingleton<Logger>(logger)
                .AddSingleton<MainWindow>()                
                .AddSingleton<EventBus>()                                
                .AddSingleton<SignalClient>()
                .AddSingleton<AppController>()
                .BuildServiceProvider();


            var appController = serviceProvider.GetService<AppController>();


            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var signalClient = serviceProvider.GetService<SignalClient>();
            //signalClient.RunAsync();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            
            Application.Run(mainWindow);
        }
    }
}