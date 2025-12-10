using CommandCenter.Infrastructure;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommandCenter.Signal
{
    // Simple client that exposes connection state and events for received messages.
    // Usage:
    //   var client = new SignalClient();
    //   client.ConnectionStateChanged += s => Console.WriteLine(s);
    //   client.OnMarketUpdate += upd => Console.WriteLine(...);
    //   await client.StartAsync(url, cancellationToken);
    //   await client.WaitForConnectedAsync(TimeSpan.FromSeconds(10));
    //   await client.StopAsync();
    public class SignalClient : IAsyncDisposable
    {        
        private HubConnection? _hub;
        private readonly object _sync = new();

        //public event Action<HubConnectionState>? ConnectionStateChanged;
        //public event Action<ServerStatus>? OnMarketUpdate;

        public HubConnectionState State => _hub?.State ?? HubConnectionState.Disconnected;

        public SignalClient()
        {
            
        }

        // Create and start connection. Safe to call multiple times (won't recreate if already started).
        public async Task StartAsync(string url, CancellationToken cancellationToken = default)
        {
            lock (_sync)
            {
                if (_hub != null)
                    return;
            }

            var hub = new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect()
                .Build();

            /*
            // Forward server messages
            hub.On<ServerStatus>("OnMarketUpdate", update =>
            {
                try
                {
                    OnMarketUpdate?.Invoke(update);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"OnMarketUpdate handler error: {ex}");
                }
            });
            */

            // Track lifecycle events
            hub.Reconnecting += ex =>
            {
                PublishConnectionStatus(SignalConnectionStatus.Connecting);
                //ConnectionStateChanged?.Invoke(HubConnectionState.Reconnecting);                
                AppController.Instance.Logger.Information($"SignalClient: Reconnecting... {ex?.Message}");
                return Task.CompletedTask;
            };

            hub.Reconnected += connectionId =>
            {
                PublishConnectionStatus(SignalConnectionStatus.Connected);
                AppController.Instance.Logger.Information($"SignalClient: Reconnected (id={connectionId}).");
                return Task.CompletedTask;
            };

            hub.Closed += ex =>
            {
                PublishConnectionStatus(SignalConnectionStatus.Disconnected);
                AppController.Instance.Logger.Information($"SignalClient: Closed. {ex?.Message}");
                return Task.CompletedTask;
            };

            try
            {
                PublishConnectionStatus(SignalConnectionStatus.Connecting);
                await hub.StartAsync(cancellationToken).ConfigureAwait(false);
                lock (_sync)
                {
                    _hub = hub;
                }
                PublishConnectionStatus(SignalConnectionStatus.Connected);
                AppController.Instance.Logger.Information("SignalClient: Connected.");
            }
            catch (OperationCanceledException) { throw; }
            catch (Exception ex)
            {
                PublishConnectionStatus(SignalConnectionStatus.Disconnected);
                AppController.Instance.Logger.Information($"SignalClient: Start failed: {ex}");
                // Ensure the failed hub is cleaned up so caller can retry.
                try { await hub.DisposeAsync().ConfigureAwait(false); } catch { }
                throw;
            }
        }

        // Stop and dispose connection (safe to call multiple times).
        public async Task StopAsync()
        {
            HubConnection? hub;
            lock (_sync)
            {
                hub = _hub;
                _hub = null;
            }

            if (hub == null)
                return;

            try
            {
                await hub.StopAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                AppController.Instance.Logger.Information($"SignalClient: StopAsync error: {ex}");
            }
            finally
            {
                try { await hub.DisposeAsync().ConfigureAwait(false); } catch { }
                PublishConnectionStatus(SignalConnectionStatus.Disconnected);
            }
        }

        private void PublishConnectionStatus(SignalConnectionStatus status)
        {
            AppController.Instance.EventBus.Publish<SignalEvents.ConnectionStatusChanged>(new SignalEvents.ConnectionStatusChanged(status, null));
        }

        // Convenience: current connection id (null if not connected)
        public string? ConnectionId => _hub?.ConnectionId;

        public async ValueTask DisposeAsync()
        {
            try
            {
                await StopAsync().ConfigureAwait(false);
            }
            catch { }
        }

        /*
         * разовые запросы
        public async Task GetExchanges()
        {
            var exchanges = await _hub.InvokeAsync<List<ExchangeDto>>("GetExchanges");
            foreach (var e in exchanges)
                Console.WriteLine($"{e.Id} - {e.Name} - {e.Url}");
        }
        */
    }
}
