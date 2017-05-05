using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RemoteAdvisor.Models.Data;

namespace RemoteAdvisor.Models
{
    public class DashboardViewModel
    {
        public IList<RemoteAdvisorSession> Sessions { get; set; }

        public DashboardViewModel()
        {
            
            using (RemoteSessionsModel dbContext = new RemoteSessionsModel())
            {
                Sessions = dbContext.RemoteAdvisorSessions.OrderByDescending(c=>c.CreateDate).ToList();
            }
        }

    }
}