using System;
using System.Collections.Generic;
using System.Text;

namespace CommandCenter.Events
{
    public class Subscription
    {
        public Type EventType { get; }
        public Delegate Handler { get; }
        public object Subscriber { get; } // Объект-подписчик
        public Func<object, bool>? Filter { get; } // Фильтр для подписчика

        public Subscription(Type eventType, Delegate handler, object subscriber, Func<object, bool>? filter = null)
        {
            EventType = eventType;
            Handler = handler;
            Subscriber = subscriber;
            Filter = filter;
        }
    }
}
