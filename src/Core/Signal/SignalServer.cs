using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;


namespace TurboBuba.Signal
{
    public class SignalServer
    {
        public void Run()
        {
            var builder = WebApplication.CreateBuilder();
            builder.WebHost.UseUrls("http://localhost:5000");//, "https://localhost:5001");
            builder.Services.AddSignalR();


            var app = builder.Build();

            app.MapHub<PilotHub>("/pilot");

            app.Run();
        }
    }
}
