using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using zeebe_poc.Services;


namespace BlazorWebAssemblySignalRApp.Server.Hubs
{

    public class EventHub : Hub
    {
        private readonly ILogger<EventHub> _logger;
        private readonly IBusinessService _businessService;

        public EventHub(ILogger<EventHub> logger, IBusinessService businessService)
        {
            _logger = logger;
            _businessService = businessService;
        }
        
        public async Task Publish(string connectionId, string command, string state, Dictionary<string, object> payload)
        {
            await Task.Run(() =>
            {
                string message = string.Format("Command '{0}' received from {1} @ '{2}' state", command, connectionId, command, state);
                _logger.LogInformation(message);
            });

            await Clients.Client(connectionId).SendAsync("FireClientEvent", command, state,  payload);
        }
    }
}