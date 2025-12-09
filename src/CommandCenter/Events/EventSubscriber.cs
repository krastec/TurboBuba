using System;
using System.Collections.Generic;
using System.Text;

namespace CommandCenter.Events
{
    public class EventSubscriber : IDisposable
    {
        private readonly List<Subscription> _subscriptions = new();
        private EventBus _eventBus = null!;

        public EventSubscriber(EventBus eventBus)
        {
            _eventBus = eventBus;            
        }

        public void Subscribe<TEvent>(Action<TEvent> handler, object subscriber, Func<TEvent, bool>? filter = null) where TEvent : IEvent
        {
            var subscription = _eventBus.Subscribe(handler, subscriber, filter);
            _subscriptions.Add(subscription);
        }

        public void Dispose()
        {
            foreach (var subscription in _subscriptions)
            {
                _eventBus.Unsubscribe(subscription);
            }
            _subscriptions.Clear();
        }
    }
}
