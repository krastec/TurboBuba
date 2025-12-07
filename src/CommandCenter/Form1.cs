using Grpc.Net.Client;
using PingPong;



namespace CommandCenter
{
    public partial class Form1 : Form
    {

        private readonly PingPong.PingPong.PingPongClient _client;

        public Form1()
        {
            InitializeComponent();

            // Разрешаем unencrypted HTTP/2 (нужно для локального http вместо https)
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            // Создаём канал
            var httpHandler = new HttpClientHandler();
            var channel = GrpcChannel.ForAddress("http://localhost:5000", new GrpcChannelOptions
            {
                HttpHandler = httpHandler
            });

            _client = new PingPong.PingPong.PingPongClient(channel);

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var request = new PingRequest { Message = "Ping" };
            var reply = await _client.SendPingAsync(request);
            MessageBox.Show(reply.Message);
        }
    }
}
