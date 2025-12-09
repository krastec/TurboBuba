public record ServerStatus (
    string Exchange,
    int Connected,
    int Ping_in,
    int Ping_out
);