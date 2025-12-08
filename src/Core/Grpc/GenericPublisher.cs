using Grpc.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace TurboBuba.Grpc
{
    public class GenericPublisher<TMessage> : IPublisher<TMessage>, IDisposable
    where TMessage : class
    {
        private readonly ConcurrentDictionary<Guid, Subscriber> _subs = new();
        private readonly Func<TMessage, IEnumerable<string>> _topicSelector;
        private readonly int _perSubscriberBuffer; // размер буфера для каждого подписчика

        public GenericPublisher(Func<TMessage, IEnumerable<string>> topicSelector, int perSubscriberBuffer = 1024)
        {
            _topicSelector = topicSelector ?? throw new ArgumentNullException(nameof(topicSelector));
            _perSubscriberBuffer = perSubscriberBuffer;
        }

        private sealed class Subscriber
        {
            public Subscriber(IServerStreamWriter<TMessage> stream, IReadOnlyCollection<string> topics, Channel<TMessage> channel, CancellationTokenSource cts)
            {
                Stream = stream;
                Topics = topics == null ? null : new HashSet<string>(topics, StringComparer.OrdinalIgnoreCase);
                Channel = channel;
                Cts = cts;
            }
            public IServerStreamWriter<TMessage> Stream { get; }
            public HashSet<string> Topics { get; } // null или пустой => все
            public Channel<TMessage> Channel { get; }
            public CancellationTokenSource Cts { get; }
        }

        public void RegisterSubscriber(Guid id, IServerStreamWriter<TMessage> stream, IReadOnlyCollection<string> topics)
        {
            // создаём bounded channel, drop oldest/throw on full? выбираем bounded + drop oldest to protect server.
            var channel = Channel.CreateBounded<TMessage>(new BoundedChannelOptions(_perSubscriberBuffer)
            {
                FullMode = BoundedChannelFullMode.DropOldest,
                SingleReader = true,
                SingleWriter = false
            });

            var cts = new CancellationTokenSource();

            var sub = new Subscriber(stream, topics, channel, cts);
            if (!_subs.TryAdd(id, sub))
            {
                cts.Cancel();
                return;
            }

            // Запускаем consumer-петлю для этого подписчика
            _ = Task.Run(() => SubscriberWriterLoop(id, sub), CancellationToken.None);
        }

        public void UnregisterSubscriber(Guid id)
        {
            if (_subs.TryRemove(id, out var sub))
            {
                try
                {
                    sub.Cts.Cancel();
                    sub.Channel.Writer.TryComplete();
                }
                catch { /* ignore */ }
            }
        }

        public async Task PublishAsync(TMessage message)
        {
            if (message == null) return;

            // Получаем топики для этого сообщения
            var msgTopics = _topicSelector(message)?.ToArray() ?? Array.Empty<string>();

            var subscribers = _subs.ToArray(); // snapshot
            foreach (var kv in subscribers)
            {
                var id = kv.Key;
                var sub = kv.Value;

                // фильтрация: если подписчик имеет ненулевой набор топиков и ни один не совпал — skip
                if (sub.Topics != null && sub.Topics.Count > 0)
                {
                    bool any = false;
                    foreach (var t in msgTopics)
                    {
                        if (sub.Topics.Contains(t))
                        {
                            any = true;
                            break;
                        }
                    }
                    if (!any) continue;
                }

                // Попытка написать в канал — неблокирующая благодаря DropOldest
                try
                {
                    // Note: TryWrite синхронный — если full, DropOldest был настроен -> будет успех
                    sub.Channel.Writer.TryWrite(message);
                }
                catch
                {
                    // если что-то пошло не так — удаляем подписчика
                    _subs.TryRemove(id, out _);
                }
            }
        }

        // Consumer: читает из channel и пишет в IServerStreamWriter
        private async Task SubscriberWriterLoop(Guid id, Subscriber sub)
        {
            try
            {
                var reader = sub.Channel.Reader;
                while (await reader.WaitToReadAsync(sub.Cts.Token).ConfigureAwait(false))
                {
                    while (reader.TryRead(out var msg))
                    {
                        try
                        {
                            await sub.Stream.WriteAsync(msg).ConfigureAwait(false);
                        }
                        catch
                        {
                            // клиент умер — удаляем и выходим
                            UnregisterSubscriber(id);
                            return;
                        }
                    }
                }
            }
            catch (OperationCanceledException) { }
            catch
            {
                // на случай необработанной ошибки — удалим подписчика
                UnregisterSubscriber(id);
            }
        }

        public void Dispose()
        {
            foreach (var kv in _subs)
            {
                try
                {
                    kv.Value.Cts.Cancel();
                    kv.Value.Channel.Writer.TryComplete();
                }
                catch { }
            }
            _subs.Clear();
        }
    }
}
