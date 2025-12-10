namespace SignalModels
{
    public record ExchangeStatus (
        int Exchange,
        int Connected,
        int Ping_in,
        int Ping_out
    );
}
