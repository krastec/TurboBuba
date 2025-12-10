using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CommandCenter.Events
{
    public class EventBus
    {
        private readonly Dictionary<Type, List<Subscription>> _subscribers = new();
        private readonly object _sync = new();

        public Subscription Subscribe<TEvent>(Action<TEvent> handler, object subscriber, Func<TEvent, bool>? filter = null) where TEvent : IEvent
        {
            var type = typeof(TEvent);
            List<Subscription> subscriptions;
            lock (_sync)
            {
                if (!_subscribers.TryGetValue(type, out subscriptions))
                {
                    subscriptions = new List<Subscription>();
                    _subscribers[type] = subscriptions;
                }
            }

            // Capture the current SynchronizationContext (may be null).
            var dispatchContext = SynchronizationContext.Current;

            var subscription = new Subscription(type, handler, subscriber, filter != null ? e => filter((TEvent)e) : null, dispatchContext);

            lock (_sync)
            {
                _subscribers[type].Add(subscription);
            }

            return subscription;
        }

        public void Unsubscribe(Subscription subscription)
        {
            lock (_sync)
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
        }

        public void Publish<TEvent>(TEvent eventData) where TEvent : IEvent
        {
            var type = typeof(TEvent);
            List<Subscription>? subscriptionsSnapshot = null;

            lock (_sync)
            {
                if (_subscribers.TryGetValue(type, out var subscriptions))
                {
                    subscriptionsSnapshot = new List<Subscription>(subscriptions);
                }
            }

            if (subscriptionsSnapshot == null)
                return;

            var deadSubscriptions = new List<Subscription>();

            foreach (var subscription in subscriptionsSnapshot)
            {
                if (subscription.Handler is Action<TEvent> action)
                {
                    try
                    {
                        // Check filter (filter uses object->bool wrapper)
                        if (subscription.Filter == null || subscription.Filter(eventData))
                        {
                            if (subscription.DispatchContext != null)
                            {
                                // Post to captured context (non-blocking)
                                try
                                {
                                    subscription.DispatchContext.Post(state =>
                                    {
                                        try
                                        {
                                            action((TEvent)state!);
                                        }
                                        catch
                                        {
                                            // swallow individual handler exceptions to protect other subscribers
                                        }
                                    }, eventData);
                                }
                                catch
                                {
                                    // If posting to context fails, fall back to direct call
                                    try
                                    {
                                        action(eventData);
                                    }
                                    catch
                                    {
                                        // ignore
                                    }
                                }
                            }
                            else
                            {
                                // No context captured, call directly
                                action(eventData);
                            }
                        }
                    }
                    catch
                    {
                        // If handler casting or filter throws, mark as dead to remove later
                        deadSubscriptions.Add(subscription);
                    }
                }
                else
                {
                    deadSubscriptions.Add(subscription);
                }
            }

            if (deadSubscriptions.Count > 0)
            {
                lock (_sync)
                {
                    if (_subscribers.TryGetValue(type, out var liveList))
                    {
                        foreach (var dead in deadSubscriptions)
                        {
                            liveList.Remove(dead);
                        }

                        if (liveList.Count == 0)
                            _subscribers.Remove(type);
                    }
                }
            }
        }
    }
}
