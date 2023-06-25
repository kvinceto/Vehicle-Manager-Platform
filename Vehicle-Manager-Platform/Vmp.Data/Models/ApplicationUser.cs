namespace Vmp.Data.Models
{
    using System;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid();
            this.Tasks = new HashSet<TaskModel>();
            this.Waybills = new HashSet<Waybill>();
            this.DateChecks = new HashSet<DateCheck>();
            this.MileageChecks = new HashSet<MileageCheck>();
        }

        public virtual ICollection<TaskModel> Tasks { get; set; }

        public virtual ICollection<Waybill> Waybills { get; set; }

        public virtual ICollection<DateCheck> DateChecks { get; set; }

        public virtual ICollection<MileageCheck> MileageChecks { get; set; }
    }
}
