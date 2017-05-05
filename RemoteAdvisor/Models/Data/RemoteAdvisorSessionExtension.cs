namespace RemoteAdvisor.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RemoteAdvisorSession
    {
        public DateTime? ExpiredDate
        {
            get { return CreateDate.AddHours(8); }
        }

        public bool Active
        {
            get { return DateTime.Now.Ticks < ExpiredDate.Value.Ticks; }
        }
    }
}
