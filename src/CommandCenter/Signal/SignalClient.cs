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

        public event Action<HubConnectionState>? ConnectionStateChanged;
        public event Action<ServerStatus>? OnMarketUpdate;

        public HubConnectionState State => _hub?.State ?? HubConnectionState.Disconnected;

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

            // Track lifecycle events
            hub.Reconnecting += ex =>
            {
                ConnectionStateChanged?.Invoke(HubConnectionState.Reconnecting);
                Console.WriteLine($"SignalClient: Reconnecting... {ex?.Message}");
                return Task.CompletedTask;
            };

            hub.Reconnected += connectionId =>
            {
                ConnectionStateChanged?.Invoke(HubConnectionState.Connected);
                Console.WriteLine($"SignalClient: Reconnected (id={connectionId}).");
                return Task.CompletedTask;
            };

            hub.Closed += ex =>
            {
                ConnectionStateChanged?.Invoke(HubConnectionState.Disconnected);
                Console.WriteLine($"SignalClient: Closed. {ex?.Message}");
                return Task.CompletedTask;
            };

            lock (_sync)
            {
                _hub = hub;
            }

            try
            {
                ConnectionStateChanged?.Invoke(_hub.State);
                await _hub.StartAsync(cancellationToken).ConfigureAwait(false);
                ConnectionStateChanged?.Invoke(_hub.State);
                Console.WriteLine("SignalClient: Connected.");
            }
            catch (OperationCanceledException) { throw; }
            catch (Exception ex)
            {
                ConnectionStateChanged?.Invoke(HubConnectionState.Disconnected);
                Console.WriteLine($"SignalClient: Start failed: {ex}");
                // Keep _hub instance so caller can attempt restart/Stop/Dispose.
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
                Console.WriteLine($"SignalClient: StopAsync error: {ex}");
            }
            finally
            {
                try { await hub.DisposeAsync().ConfigureAwait(false); } catch { }
                ConnectionStateChanged?.Invoke(HubConnectionState.Disconnected);
            }
        }

        // Wait until connected or timeout. Returns true if connected.
        public async Task<bool> WaitForConnectedAsync(TimeSpan timeout)
        {
            if (State == HubConnectionState.Connected)
                return true;

            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            void Handler(HubConnectionState s)
            {
                if (s == HubConnectionState.Connected)
                    tcs.TrySetResult(true);
                else if (s == HubConnectionState.Disconnected)
                    tcs.TrySetResult(false);
            }

            ConnectionStateChanged += Handler;
            try
            {
                // If already connected after subscribing, return immediately
                if (State == HubConnectionState.Connected)
                    return true;

                var delay = Task.Delay(timeout);
                var completed = await Task.WhenAny(tcs.Task, delay).ConfigureAwait(false);
                return completed == tcs.Task && tcs.Task.Result;
            }
            finally
            {
                ConnectionStateChanged -= Handler;
            }
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
    }
}
