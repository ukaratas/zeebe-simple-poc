
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Linq;



namespace zeebe_poc.Services
{
    public interface IBusinessService
    {
        ConcurrentBag<ApplicationModel> Applications();

        ApplicationModel GetActiveApplication(string email);

        ApplicationModel NewApplication(string email, string InstanceId);
    }
    public class BusinessService : IBusinessService
    {
        ConcurrentBag<ApplicationModel> applications = new ConcurrentBag<ApplicationModel>();

        private readonly ILogger<BusinessService> _logger;

        public BusinessService(ILogger<BusinessService> logger)
        {
            _logger = logger;
        }

        public ConcurrentBag<ApplicationModel> Applications()
        {
            return applications;
        }

        public ApplicationModel GetActiveApplication(string email)
        {
            return applications.Where(i => i.Email == email && i.IsCompleted == false).FirstOrDefault();
        }

        public ApplicationModel NewApplication(string email, string InstanceId)
        {

            var newApplication = new ApplicationModel
            {
                Email = email,
                InstanceId = InstanceId,
                IsCompleted = true
            };


            applications.Add(newApplication);
            return newApplication;
        }
    }
}