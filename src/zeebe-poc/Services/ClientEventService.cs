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

    public delegate void ClientEvent(string command, string state, Dictionary<string, object> payload);


    public interface IClientEventService
    {
        void Register(Uri url);
        string ConnectionId();
        void SubcribeMeForClientEvent(ClientEvent callback);
    }
    public class ClientEventService : IClientEventService
    {
        private readonly ILogger<ClientEventService> _logger;

        private HubConnection hubConnection;


        private List<ClientEvent> ClientEventSubscribers = new List<ClientEvent>();


        public ClientEventService(ILogger<ClientEventService> logger)
        {
            _logger = logger;
        }

        public void SubcribeMeForClientEvent(ClientEvent callback)
        {
            ClientEventSubscribers.Add(callback);
        }

        public async void Register(Uri url)
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(url)
                .Build();

            hubConnection.On<string , string , Dictionary<string, object> >("FireClientEvent", ( command,  state,  payload) =>
            {
                string message = string.Format("FireClientEvent is catched - State : {0} and Command: {1} with payload {2} ", state, command , payload);
                _logger.LogInformation(message);

                ClientEventSubscribers.ForEach(s => s.Invoke(command, state, payload));
            });

            await hubConnection.StartAsync();
        }

        public string ConnectionId()
        {
            return hubConnection.ConnectionId;
        }

    }
}