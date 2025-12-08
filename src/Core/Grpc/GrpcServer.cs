using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace TurboBuba.Grpc
{
    public class GrpcServer
    {
        public void Run()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddGrpc();
            var app = builder.Build();

            // Маршрутизируем gRPC
            app.MapGrpcService<PingPongService>();
            //app.MapGrpcService<StatusService>();
            // Дополнительно простой hello endpoint для проверки
            //app.MapGet("/", () => "PingPong gRPC server is running.");
            app.Urls.Add("https://localhost:5000"); // слушать на 5000 (HTTP/2 over plaintext)

            app.Run();
        }
    }
}
