using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;
using Zeebe.Client.Impl.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Collections.Generic;

namespace zeebe_poc.Services
{
    public interface IServerEventService
    {
        void Register(Uri url);
        void PublishEvent(string connectionId, string command, string state, Dictionary<string, object> payload);
    }
    public class ServerEventService : IServerEventService
    {
        private readonly ILogger<ServerEventService> _logger;

        private HubConnection hubConnection;


        public ServerEventService(ILogger<ServerEventService> logger)
        {
            _logger = logger;
        }

        public async void Register(Uri url)
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(url)
                .Build();

            await hubConnection.StartAsync();
        }

        public async void PublishEvent(string connectionId, string command, string state, Dictionary<string, object> payload = null)
        {
            while (hubConnection == null)
            {
                Thread.Sleep(100);
            }

            _logger.LogInformation("PublishEvent : Sending Message");

            await hubConnection.SendAsync("Publish", connectionId, command, state, payload);
        }

    }
}