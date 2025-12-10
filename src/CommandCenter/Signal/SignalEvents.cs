using CommandCenter.Events;

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
