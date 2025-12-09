using CommandCenter.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommandCenter.Grpc
{
    public class GrpcEvents
    {
        public class ConnectionStatusChanged : IEvent
        {
            public GrpcConnectionStatus Status { get; }
            public string? Error { get; }
            public ConnectionStatusChanged(GrpcConnectionStatus status, string? error)
            {
                Status = status;
                Error = error;
            }
        }
    }
}
