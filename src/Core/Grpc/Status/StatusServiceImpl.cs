using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;
using TurboBuba.Infrastructure;
using TurboBuba.Status.V1;

namespace TurboBuba.Grpc.Status
{
    public class StatusServiceImpl : StatusService.StatusServiceBase
    {
        private readonly IPublisher<StatusUpdate> _publisher;
        private readonly AppController _appController;

        public StatusServiceImpl(IPublisher<StatusUpdate> publisher, AppController appController)
        {
            _publisher = publisher;
            _appController = appController;
        }
        public override async Task SubscribeStatus(StatusSubscribeRequest request, IServerStreamWriter<StatusUpdate> responseStream, ServerCallContext context)
        {
            var id = Guid.NewGuid();
            var topics = request.Components.Count > 0 ? (IReadOnlyCollection<string>)request.Components : null; // null -> all
            _publisher.RegisterSubscriber(id, responseStream, topics);

            try
            {
                // Ждём отмены — канал/consumer будет писать в responseStream
                await Task.Delay(Timeout.Infinite, context.CancellationToken);
            }
            catch (OperationCanceledException) { }
            finally
            {
                _publisher.UnregisterSubscriber(id);
            }
        }

        private async Task SafePublishAsync(StatusUpdate upd)
        {
            try
            {
                await _publisher.PublishAsync(upd).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // логируем, но не кидаем исключение в шину
                Console.WriteLine($"Publish failed: {ex.Message}");
            }
        }

        public void Dispose()
        {
            // обязательно отписываемся от шины при остановке сервера
            //try { _bus.Unsubscribe(_onPing); } catch { }
        }


        /*
        public override async Task SubscribeStatus(StatusSubscribeRequest request, IServerStreamWriter<StatusUpdate> responseStream, ServerCallContext context)
        {
            var rnd = new Random();
            var components = request.Components.Count > 0 ? request.Components : { "ExchangeAPI:Binance", "ExchangeAPI:Bybit" }
            ;

            while (!context.CancellationToken.IsCancellationRequested)
            {
                foreach (var comp in components)
                {
                    var upd = new StatusUpdate
                    {
                        Component = comp,
                        State = (rnd.NextDouble() > 0.1) ? ConnState.Connected : ConnState.Disconnected,
                        TimestampMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        Message = "ok"
                    };
                    await responseStream.WriteAsync(upd);
                }

                await Task.Delay(1000, context.CancellationToken).ContinueWith(_ => { });
            }
        }
        */
    }
}
