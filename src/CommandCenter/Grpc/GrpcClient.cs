using CommandCenter.Infrastructure;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using TurboBuba.Status.V1;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace CommandCenter.Grpc
{
    public class GrpcClient
    {
        private AppController _appController;

        private HttpClientHandler _clientHandler;
        private GrpcChannel _channel;
        private CancellationTokenSource? _monitorCts;

        public GrpcClient(AppController appController)
        {
            _appController = appController;
        }

        public async Task StartAsync(string url, CancellationToken cancellationToken = default)
        {
            PublishStatusEvent(GrpcConnectionStatus.Connecting);

            try
            {
                var socketsHandler = new SocketsHttpHandler
                {
                    EnableMultipleHttp2Connections = true,
                    // Optional: TLS cert validation; avoid for production
                    SslOptions = new SslClientAuthenticationOptions
                    {
                        RemoteCertificateValidationCallback = (sender, cert, chain, errors) =>
                        {
                            // Trust loopback for dev, else require a valid chain
                            if (sender is HttpRequestMessage req && req.RequestUri.IsLoopback) return true;
                            return errors == SslPolicyErrors.None;
                        }
                    },
                    // Optional: HTTP/2 keepalive
                    KeepAlivePingDelay = TimeSpan.FromSeconds(20),
                    KeepAlivePingTimeout = TimeSpan.FromSeconds(10),
                    //EnableMultipleHttp2Connections = true
                };

                _channel = GrpcChannel.ForAddress(url, new GrpcChannelOptions
                {
                    HttpHandler = socketsHandler
                });

                await _channel.ConnectAsync(cancellationToken).ConfigureAwait(false);

                PublishStatusEvent(GrpcConnectionStatus.Connected);
                StartConnectivityMonitor();
            }
            catch (Exception ex)
            {
                PublishStatusEvent(GrpcConnectionStatus.Disconnected, ex.Message);
                //PublishStatusEvent(GrpcConnectionStatus.TransientFailure, ex.Message);
            }
        }

        public async Task StopAsync()
        {
            //PublishStatusEvent(GrpcConnectionStatus.Shutdown);
            PublishStatusEvent(GrpcConnectionStatus.Disconnected);

            try
            {
                _monitorCts?.Cancel();
                _monitorCts?.Dispose();
                _monitorCts = null;

                if (_channel != null)
                {
                    await _channel.ShutdownAsync().ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                // ignore
            }
            finally
            {
                PublishStatusEvent(GrpcConnectionStatus.Disconnected);
            }
        }

        private async Task WarmupAsync(CancellationToken ct)
        {
            // If you have a ping service, call it here. Otherwise do a channel wait.
            // Wait for Ready state briefly to ensure transport is up
            await _channel.ConnectAsync(ct).ConfigureAwait(false);
        }

        private void StartConnectivityMonitor()
        {
            _monitorCts = new CancellationTokenSource();

            // Monitor channel connectivity and publish status changes
            _ = Task.Run(async () =>
            {
                var last = ConnectivityState.Idle;

                while (!_monitorCts!.IsCancellationRequested)
                {
                    var state = _channel.State;

                    if (state != last)
                    {
                        PublishStatusEvent(ToStatus(state));
                        last = state;
                    }

                    // If not Ready, ask channel to re-connect
                    if (state == ConnectivityState.TransientFailure || state == ConnectivityState.Idle)
                    {
                        try
                        {
                            await _channel.ConnectAsync(_monitorCts.Token).ConfigureAwait(false);
                        }
                        catch
                        {
                            // swallow; ConnectAsync will throw when canceled or failing
                        }
                    }

                    await Task.Delay(500, _monitorCts.Token).ConfigureAwait(false);
                }
            }, _monitorCts.Token);
        }

        public void Start(string url)
        {
            _clientHandler = new HttpClientHandler();
            _clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (message.RequestUri.IsLoopback) 
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };

            _channel = GrpcChannel.ForAddress(url, new GrpcChannelOptions
            {
                HttpClient = new HttpClient(_clientHandler)
            });


            //this.SubscribeStatus();
        }

        private static GrpcConnectionStatus ToStatus(ConnectivityState state) =>
            state switch
            {
                ConnectivityState.Ready => GrpcConnectionStatus.Connected,
                ConnectivityState.Connecting => GrpcConnectionStatus.Connecting,
                ConnectivityState.TransientFailure => GrpcConnectionStatus.Disconnected,//GrpcConnectionStatus.TransientFailure,
                ConnectivityState.Idle => GrpcConnectionStatus.Disconnected,//GrpcConnectionStatus.Idle,
                ConnectivityState.Shutdown => GrpcConnectionStatus.Disconnected, //GrpcConnectionStatus.Shutdown,
                _ => GrpcConnectionStatus.Disconnected
            };

        private void PublishStatusEvent(GrpcConnectionStatus status, string? error = null)
        {
            _appController.EventBus.Publish(new GrpcEvents.ConnectionStatusChanged(status, error));
        }

        private void SubscribeStatus()
        {
            var statusClient = new StatusService.StatusServiceClient(_channel);
            var cts = new CancellationTokenSource();
            _ = Task.Run(async () =>
            {
                try
                {
                    using var call = statusClient.SubscribeStatus(new StatusSubscribeRequest());
                    await foreach (var s in call.ResponseStream.ReadAllAsync(cts.Token))
                    {
                        Console.WriteLine($"Status {s.Component} = {s.State} @ {s.TimestampMs}");
                    }
                }
                catch (OperationCanceledException) { }
                catch (Exception ex) { Console.WriteLine("Status stream error: " + ex.Message); }
            });
        }
    }
}
