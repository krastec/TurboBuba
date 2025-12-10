namespace SignalModels
{
    public record ExchangeStatus (
        string Exchange,
        int Connected,
        int Ping_in,
        int Ping_out
    );
}
