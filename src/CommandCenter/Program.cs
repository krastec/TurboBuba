using CommandCenter.Events;
using CommandCenter.Infrastructure;
using CommandCenter.Signal;
using Microsoft.Extensions.DependencyInjection;

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

            var serviceProvider = new ServiceCollection()               
                .AddSingleton<MainWindow>()                
                .AddSingleton<EventBus>()
                .AddSingleton<AppController>()
                //.AddSingleton<GrpcClient>()
                .AddSingleton<SignalClient>()
                .BuildServiceProvider();


            serviceProvider.GetService<AppController>();


            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            //var signalClient = serviceProvider.GetService<SignalClient>();
            //signalClient.RunAsync();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            
            Application.Run(mainWindow);
        }
    }
}