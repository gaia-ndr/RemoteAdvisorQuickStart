using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteAdvisor.UCAP
{
    public class MeetingInfo
    {
        public string DiscoverUri { get; set; }
        public string OnlineMeetingUri { get; set; }
        public string OrganizerUri { get; set; }
        public string JoinUrl { get; set; }
        public DateTime ExpireTime { get; set; }
        public string[] Leaders { get; set; }
        public string AnonymousToken { get; set; }
        public string Subject { get; set; }
    }
}
