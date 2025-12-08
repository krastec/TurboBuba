using System;
using System.Collections.Generic;
using System.Text;
using Grpc.Core;

namespace TurboBuba.Grpc
{
    public interface IPublisher<TMessage>
    where TMessage : class
    {
        void RegisterSubscriber(Guid id, IServerStreamWriter<TMessage> stream, IReadOnlyCollection<string> topics);
        void UnregisterSubscriber(Guid id);
        Task PublishAsync(TMessage message);
    }
}
