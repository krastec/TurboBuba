using CommandCenter.Events;
using CommandCenter.Signal;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommandCenter.Infrastructure
{

    public class AppController
    {
        public static AppController Instance { get; private set; }

        private EventBus _eventBus;
        private IServiceProvider _serviceProvider;
        private Logger _logger;
        private SignalClient _signalClient;


        public EventBus EventBus => _eventBus;
        public IServiceProvider ServiceProvider => _serviceProvider;
        public Logger Logger => _logger;
        public SignalClient SignalClient => _signalClient;

        public AppController(EventBus eventBus, IServiceProvider serviceProvider, Logger logger, SignalClient signalClient)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _eventBus = eventBus;                       
            _signalClient = signalClient;

            Instance = this;
        }
    }
}
