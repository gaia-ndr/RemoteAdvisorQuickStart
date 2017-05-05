namespace RemoteAdvisor.Models.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class RemoteSessionsModel : DbContext
    {
        public RemoteSessionsModel()
            : base("name=RemoteSessionsModel")
        {
        }

        public virtual DbSet<RemoteAdvisorSession> RemoteAdvisorSessions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
