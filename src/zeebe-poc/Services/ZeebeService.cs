using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;
using Zeebe.Client.Impl.Builder;

namespace zeebe_poc.Services
{
    public interface IZeebeService
    {
        public Task<ITopology> Status();
        public Task<IDeployResponse> Deploy(string modelFilename);
        public Task<String> SendMessage(string instanceId, string messageName, string payload);
        public void StartWorkers();

    }
    public class ZeebeService : IZeebeService
    {
        private readonly IZeebeClient _client;
        private readonly ILogger<ZeebeService> _logger;
        private readonly IBusinessService _businessService;
        private readonly IServerEventService _serverEventService;

        public ZeebeService(ILogger<ZeebeService> logger, IBusinessService businessService, IServerEventService eventService, IConfiguration configuration)
        {
            _logger = logger;
            _businessService = businessService;
            _serverEventService = eventService;

            var authServer = configuration["authServer"];
            var clientId = configuration["clientId"];
            var clientSecret = configuration["clientSecret"];
            var zeebeUrl = configuration["zeebeUrl"];


            char[] port =
            {
                '4', '3', ':'
            };
            var audience = zeebeUrl?.TrimEnd(port);

            _client =
                ZeebeClient.Builder()
                    .UseGatewayAddress(zeebeUrl)
                    .UseTransportEncryption()
                    .UseAccessTokenSupplier(
                        CamundaCloudTokenProvider.Builder()
                            .UseAuthServer(authServer)
                            .UseClientId(clientId)
                            .UseClientSecret(clientSecret)
                            .UseAudience(audience)
                            .Build())
                    .Build();
        }

        public Task<ITopology> Status()
        {
            return _client.TopologyRequest().Send();
        }

        public async Task<IDeployResponse> Deploy(string modelFilename)
        {
            var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Processes", modelFilename);
            var deployment = await _client.NewDeployCommand().AddResourceFile(filename).Send();
            var res = deployment.Workflows[0];
            _logger.LogInformation("Deployed BPMN Model: " + res?.BpmnProcessId + " v." + res?.Version);
            return deployment;
        }

        public async Task<String> SendMessage(string instanceId, string messageName, string payload)
        {
            _logger.LogInformation("SendMessage:", messageName, payload);

            var instance = await _client.NewPublishMessageCommand()
                    .MessageName(messageName)
                    .CorrelationKey(instanceId)
                    .TimeToLive(TimeSpan.FromMinutes(30))
                    .Variables(payload)
                    .Send();

            string jsonText = JsonSerializer.Serialize(instance, new JsonSerializerOptions
            { Converters = { new JsonStringEnumConverter() } });

            return jsonText;
        }

        private void _createWorker(String jobType, JobHandler handleJob)
        {
            _client.NewWorker()
                    .JobType(jobType)
                    .Handler(handleJob)
                    .MaxJobsActive(5)
                    .Name(jobType)
                    .PollInterval(TimeSpan.FromSeconds(50))
                    .PollingTimeout(TimeSpan.FromSeconds(50))
                    .Timeout(TimeSpan.FromSeconds(10))
                    .Open();
        }

        public void CreatePersistDataWorker()
        {
            _logger.LogInformation("PersistData Worker registered ");

            _createWorker("PersistData", async (client, job) =>
            {
                _logger.LogInformation("Persist Data: " + job);

                Dictionary<string, object> customHeaders = JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<string, object>>(job.CustomHeaders);
                Dictionary<string, object> variables = JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<string, object>>(job.Variables);

                var state = customHeaders["State"].ToString();

                switch (state)
                {
                    case "Apply":
                        //First Publish
                        var application = _businessService.NewApplication(variables["Email"].ToString(), variables["InstanceId"].ToString());
                        application.FullName = variables["FullName"].ToString();
                        application.EventingConntectionId = variables["EventingConnectionId"].ToString();
                        application.Request = (RequestType)Enum.Parse(typeof(RequestType), variables["Request"].ToString());
                        application.Donation = int.Parse(variables["Donation"].ToString());
                        application.State = state;
                        break;
                    default:
                        break;
                }

                await client.NewCompleteJobCommand(job.Key)
                    .Variables("{\"PersistData\":\"Completed\"}")
                    .Send();
            });
        }

        public void CreatePublishWorker()
        {
            _logger.LogInformation("Publish Worker registered ");

            _createWorker("Publish", async (client, job) =>
            {
                _logger.LogInformation("Publish: " + job);

                Dictionary<string, object> customHeaders = JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<string, object>>(job.CustomHeaders);
                Dictionary<string, object> variables = JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<string, object>>(job.Variables);

                Dictionary<string, object> payload = new Dictionary<string, object>();
                payload.Add("Route", variables["TargetRoute"].ToString());

                _serverEventService.PublishEvent(variables["EventingConnectionId"].ToString(), variables["ClientCommand"].ToString(), customHeaders["State"].ToString(), payload);

                await client.NewCompleteJobCommand(job.Key).Send();
            });
        }

        public void CreateProcessPaymentWorker()
        {
            _logger.LogInformation("Process Payment Worker registered ");

            _createWorker("ProcessPayment", async (client, job) =>
            {
                _logger.LogInformation("Process Payment: " + job);

                Dictionary<string, object> customHeaders = JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<string, object>>(job.CustomHeaders);
                Dictionary<string, object> variables = JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<string, object>>(job.Variables);

                var amount = variables["Amount"].ToString();

                await client.NewCompleteJobCommand(job.Key)
                    .Variables("{\"Tax\":10}")
                    .Send();
            });
        }

        public void StartWorkers()
        {
            CreatePersistDataWorker();
            CreatePublishWorker();
            CreateProcessPaymentWorker();
        }
    }
}