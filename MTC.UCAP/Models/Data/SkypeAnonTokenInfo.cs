using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteAdvisor.UCAP
{
    public class SkypeAnonTokenInfo
    {
        public SkypeAnonTokenInfo()
        {
            AnonToken = "";
            DiscoverUrl = "";
        }
        public string AnonToken { get; set; }
        public string DiscoverUrl { get; set; }
        public string ConferenceUri { get; set; }
    }
}
