using System;
using System.Collections.Generic;
using System.Text;

namespace TurboBuba.Events
{
    public class EventBus
    {
        private readonly Dictionary<Type, List<Subscription>> _subscribers = new();

        public Subscription Subscribe<TEvent>(Action<TEvent> handler, object subscriber, Func<TEvent, bool>? filter = null) where TEvent : IEvent
        {
            var type = typeof(TEvent);
            if (!_subscribers.TryGetValue(type, out var subscriptions))
            {
                subscriptions = new List<Subscription>();
                _subscribers[type] = subscriptions;
            }

            var subscription = new Subscription(type, handler, subscriber, filter != null ? e => filter((TEvent)e) : null);
            subscriptions.Add(subscription);
            return subscription;
        }

        public void Unsubscribe(Subscription subscription)
        {
            if (_subscribers.TryGetValue(subscription.EventType, out var subscriptions))
            {
                subscriptions.Remove(subscription);

                if (subscriptions.Count == 0)
                {
                    _subscribers.Remove(subscription.EventType);
                }
            }
        }

        public void Publish<TEvent>(TEvent eventData) where TEvent : IEvent
        {
            var type = typeof(TEvent);
            if (_subscribers.TryGetValue(type, out var subscriptions))
            {
                var deadSubscriptions = new List<Subscription>();

                foreach (var subscription in subscriptions)
                {
                    if (subscription.Handler is Action<TEvent> action)
                    {
                        // Проверяем фильтр
                        if (subscription.Filter == null || subscription.Filter(eventData))
                        {
                            action(eventData);
                        }
                    }
                    else
                    {
                        deadSubscriptions.Add(subscription);
                    }
                }

                foreach (var dead in deadSubscriptions)
                {
                    subscriptions.Remove(dead);
                }
            }
        }
    }

}
