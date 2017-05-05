using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RemoteAdvisor.Models
{
    public class JoinViewModel
    {
        public string SessionId { get; set; } = Guid.NewGuid().ToString();
        public string DiscoverUri { get; set; }
        public string OnlineMeetingUri { get; set; }
        public string OrganizerUri { get; set; }
        public string JoinUrl { get; set; }
        public string AnonymousToken { get; set; }
        public string Subject { get; set; }
        public string Customer { get; set; }

    }
}