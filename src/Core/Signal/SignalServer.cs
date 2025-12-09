using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace TurboBuba.Signal
{
    public class SignalServer
    {
        public void Run()
        {
            var builder = WebApplication.CreateBuilder();
            builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001");
            builder.Services.AddSignalR();


            var app = builder.Build();

            app.MapHub<ServerStatusHub>("/serverstatus");

            app.Run();
        }
    }
}
