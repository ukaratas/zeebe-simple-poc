using System;
using System.Collections.Generic;


namespace zeebe_poc
{
    public class ApplicationModel
    {
        public ApplicationModel()
        {
            Logs = new List<string>();
        }
        public string EventingConntectionId { get; set; }
        public bool? IsCompleted { get; set; }
        public string InstanceId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public RequestType? Request { get; set; }
        public int? Donation { get; set; }
        public string State { get; set; }
        public List<String> Logs { get; set; }
    }
}

