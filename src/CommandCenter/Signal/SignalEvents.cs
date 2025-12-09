using CommandCenter.Events;
using CommandCenter.Grpc;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommandCenter.Signal
{
    public class SignalEvents
    {
        public class ConnectionStatusChanged : IEvent
        {
            public SignalConnectionStatus Status { get; }
            public string? Error { get; }
            public ConnectionStatusChanged(SignalConnectionStatus status, string? error)
            {
                Status = status;
                Error = error;
            }
        }
    }
}
