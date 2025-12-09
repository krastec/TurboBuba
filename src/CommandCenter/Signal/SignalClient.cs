using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommandCenter.Signal
{
    public class SignalClient
    {
        public async Task RunAsync()
        {
            var hub = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/market")
                .WithAutomaticReconnect()
                .Build();

            hub.On<ServerStatus>("OnMarketUpdate", update =>
            {
                Console.WriteLine(
                    $"Data received"
                );
            });

            await hub.StartAsync();

            Console.WriteLine("Подписан. Жду обновлений...");
            await Task.Delay(-1);
        }
    }
}
