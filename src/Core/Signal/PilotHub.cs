using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TurboBuba.Signal
{
    public class PilotHub : Hub
    {

        public async Task Subscribe(string topic)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, topic);
        }

        public async Task Unsubscribe(string topic)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, topic);
        }

        // бот вызывает
        /*
        public async Task BroadcastUpdate(ServerStatus topic)
        {
            //await Clients.Group(update.Symbol).SendAsync("OnMarketUpdate", update);
        }
        */


        //разовые запрос - ответ
        public record ExchangeDto(string Id, string Name, string Url);
        public Task<List<ExchangeDto>> GetExchanges()
        {
            // в реале: из БД/конфига/сервиса
            var exchanges = new List<ExchangeDto>
            {
                new("binance", "Binance", "https://api.binance.com"),
                new("bybit", "Bybit", "https://api.bybit.com")
            };
            return Task.FromResult(exchanges);
        }
        // Метод для HFT-бота: отправить обновление всем подписчикам
        //public async Task BroadcastUpdate(ServerStatus update)
        //{
        //    await Clients.All.SendAsync("OnMarketUpdate", update);   
        //}
    }
}
