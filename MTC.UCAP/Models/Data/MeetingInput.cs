using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteAdvisor.UCAP
{
    public class MeetingInput
    {
        public string Subject { get; set; }
        public string Description { get; set; }
        public string AccessLevel { get; set; }
        public string EntryExitAnnouncement { get; set; }
        public string AutomaticLeaderAssignment { get; set; }
        public string PhoneUserAdmission { get; set; } 
        public string LobbyBypassForPhoneUsers { get; set; }
        public string[] Leaders { get; set; }
        public string MeetingIdentifier { get; set; }
    }
}
