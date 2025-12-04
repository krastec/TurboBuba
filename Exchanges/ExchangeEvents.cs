using System;
using System.Collections.Generic;
using System.Text;
using TurboBuba.Events;

namespace TurboBuba.DataFeeds
{
    public class ExchangeEvents
    {
        public class ConnectionStatusChanged : IEvent
        {
            public BaseExchangeController Exchange { get; }
            public ExchangeConnectionStatus Status { get; }
            public ConnectionStatusChanged(BaseExchangeController exchange, ExchangeConnectionStatus status)
            {
                Exchange = exchange;
                Status = status;
            }
        }

        public class LatencyUpdated : IEvent 
        {
            public BaseExchangeController Exchange { get; }
            public int LatencyIn { get; }
            public int LatencyOut { get; }

            public LatencyUpdated(BaseExchangeController exchange, int latencyIn, int latencyOut)
            {
                Exchange = exchange;
                LatencyIn = latencyIn;
                LatencyOut = latencyOut;
            }
        }
    }
}
