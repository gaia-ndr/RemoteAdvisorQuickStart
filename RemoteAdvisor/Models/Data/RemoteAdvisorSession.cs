namespace RemoteAdvisor.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RemoteAdvisorSession
    {
        [Key]
        public Guid SessionId { get; set; }

        [Required]
        [StringLength(500)]
        public string Subject { get; set; }

        [Required]
        [StringLength(500)]
        public string Customer { get; set; }

        [Required]
        [StringLength(500)]
        public string OnlineMeetingUri { get; set; }

        [Required]
        [StringLength(2000)]
        public string JoinUrl { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? ExpireDate { get; set; }

        public DateTime? QueueStart { get; set; }
    }
}
