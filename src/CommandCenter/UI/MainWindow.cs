using CommandCenter.Events;
using CommandCenter.Infrastructure;
using CommandCenter.Signal;

using Microsoft.Extensions.DependencyInjection;




namespace CommandCenter
{
    public partial class MainWindow : Form
    {
        private AppController _appController;
        private EventSubscriber _eventSubscriber;

        public MainWindow(AppController appController)
        {
            _appController = appController;

            InitializeComponent();

            //this.RegisterEventHandlers();
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

        private void MainWindow_Load(object sender, EventArgs e)
        {
            signalConnectionPanel.Initialize(_appController);
        }
    }
}
