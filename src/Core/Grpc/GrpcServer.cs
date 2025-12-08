using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TurboBuba.Grpc.Status;
using TurboBuba.Infrastructure;
using TurboBuba.Status.V1;

namespace TurboBuba.Grpc
{
    public class GrpcServer
    {
        private readonly IServiceProvider _serviceProvider;
        public GrpcServer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Run()
        {
            var appController = _serviceProvider.GetService<AppController>();  

            var builder = WebApplication.CreateBuilder();
            builder.Services.AddGrpc();
            
            builder.Services.AddSingleton<AppController>(appController);

            builder.Services.AddSingleton<IPublisher<StatusUpdate>>(sp => new GenericPublisher<StatusUpdate>(msg => new[] { msg.Component }));
            builder.Services.AddSingleton<StatusServiceImpl>();

            var app = builder.Build();

            // Маршрутизируем gRPC
            app.MapGrpcService<PingPongService>();
            app.MapGrpcService<StatusServiceImpl>();
            // Дополнительно простой hello endpoint для проверки
            //app.MapGet("/", () => "PingPong gRPC server is running.");
            app.Urls.Add("https://localhost:5000"); // слушать на 5000 (HTTP/2 over plaintext)

            app.Services.GetRequiredService<StatusServiceImpl>(); //инициализируем сервис сразу для того чтобы он подписался на нужные события

            app.Run();
        }
    }
}
