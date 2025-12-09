using System;
using System.Collections.Generic;
using System.Text;

namespace CommandCenter.Grpc
{
    public enum GrpcConnectionStatus
    {
        Unknown = 0,
        Connected = 1,
        Disconnected = 2,
        Connecting = 3
    }
}
