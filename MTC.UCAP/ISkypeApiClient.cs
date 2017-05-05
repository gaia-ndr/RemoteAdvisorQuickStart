using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkypeApi.Models;
using MTC.RemoteAdvisor.Common;

namespace MTC.UCAP
{
    public interface ISkypeApiClient
    {
        MeetingOutput ScheduleAdhocMeeting(MeetingInput adhocMeetingInput);
        SkypeAnonTokenInfo GetAnonymousToken(SkypeAnonTokenInfo tokenInput);
    }
}
