using System;
using System.Collections.Generic;
using System.Text;
using TurboBuba.Events;

namespace TurboBuba.Infrastructure
{
    

    public class AppController
    {
        private static AppController _instance = null!;
        public static AppController Instance { get { return _instance; } }

        private EventBus _eventBus = null!;
        public EventBus EventBus { get { return _eventBus; } }

        private IServiceProvider _serviceProvider = null!;
        public IServiceProvider ServiceProvider { get { return _serviceProvider; } }

        public AppController(EventBus eventBus, IServiceProvider serviceProvider)
        {
            _eventBus = eventBus;
            _serviceProvider = serviceProvider;

            _instance = this;
        }



     
    }
}
