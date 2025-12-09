using CommandCenter.Events;
using CommandCenter.Grpc;
using CommandCenter.Infrastructure;
using CommandCenter.Signal;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using PingPong;
using System;



namespace CommandCenter
{
    public partial class MainWindow : Form
    {
        private AppController _appController;
        private EventSubscriber _eventSubscriber;

        private readonly PingPong.PingPong.PingPongClient _client;

        public MainWindow(AppController appController)
        {
            _appController = appController;

            InitializeComponent();
           
            this.RegisterEventHandlers();
            /*
            // Разрешаем unencrypted HTTP/2 (нужно для локального http вместо https)
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            // Создаём канал
            var httpHandler = new HttpClientHandler();
            //httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress("https://localhost:5000", new GrpcChannelOptions
            {
                HttpHandler = httpHandler
            });

            _client = new PingPong.PingPong.PingPongClient(channel);
            */
        }

        private void RegisterEventHandlers()
        {
            this._eventSubscriber = new EventSubscriber(_appController.EventBus);            
            _eventSubscriber.Subscribe<GrpcEvents.ConnectionStatusChanged>(OnGrpcConnectionStatusChanged, this);
        }
        private void OnGrpcConnectionStatusChanged(GrpcEvents.ConnectionStatusChanged evt)
        {
            if (evt.Status == GrpcConnectionStatus.Connected)
            {
                grpcConnectButton.Text = "Disconnect";
            }
            else
            {
                grpcConnectButton.Text = "Connect";
            }
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            var request = new PingRequest { Message = "Ping" };
            var reply = await _client.SendPingAsync(request);
            MessageBox.Show(reply.Message);
        }

        private void grpcConnectButton_Click(object sender, EventArgs e)
        {
            var url = grpcUrlTextBox.Text;
            //var grpcClient = _appController.ServiceProvider.GetRequiredService<GrpcClient>();
            //grpcClient.StartAsync(url);
            var signalClient = _appController.ServiceProvider.GetService<SignalClient>();
            signalClient.StartAsync(url);
        }
    }
}
