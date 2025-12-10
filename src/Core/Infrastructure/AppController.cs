using System;
using System.Collections.Generic;
using System.Text;
using TurboBuba.Events;

namespace TurboBuba.Infrastructure
{
    

    public class AppController
    {
        public static AppController Instance { get; private set; }

        private EventBus _eventBus = null!;
        public EventBus EventBus { get { return _eventBus; } }

        private IServiceProvider _serviceProvider = null!;
        public IServiceProvider ServiceProvider { get { return _serviceProvider; } }

        public AppController(EventBus eventBus, IServiceProvider serviceProvider)
        {
            _eventBus = eventBus;
            _serviceProvider = serviceProvider;

            Instance = this;
        }



     
    }
}
