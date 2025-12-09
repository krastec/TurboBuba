using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TurboBuba.Signal
{
    public class PilotHub : Hub
    {
        // Метод для HFT-бота: отправить обновление всем подписчикам
        public async Task BroadcastUpdate(ServerStatus update)
        {
            await Clients.All.SendAsync("OnMarketUpdate", update);
            
        }
    }
}
