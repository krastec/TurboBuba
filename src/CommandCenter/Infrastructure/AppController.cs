using CommandCenter.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommandCenter.Infrastructure
{

    public class AppController
    {
        private EventBus _eventBus;
        private IServiceProvider _serviceProvider;

        public EventBus EventBus => _eventBus;
        public IServiceProvider ServiceProvider => _serviceProvider;

        

        public AppController(EventBus eventBus, IServiceProvider serviceProvider)
        {
            _eventBus = eventBus;
            _serviceProvider = serviceProvider;
        }
    }
}
