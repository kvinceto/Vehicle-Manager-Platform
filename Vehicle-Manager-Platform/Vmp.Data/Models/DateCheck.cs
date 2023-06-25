namespace Vmp.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;

    using static Vmp.Common.EntityValidationConstants.DateCheck;

    [Comment("Check to reach a certain date")]
    public class DateCheck
    {
        [Comment("Chech identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Name of the check")]
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Comment("Date to be reached")]
        [Required]
        public DateTime EndDate { get; set; }

        [Comment("Licence plate of the Vehicle corresponding with this check")]
        public string VehicleNumber { get; set; } = null!;

        public virtual Vehicle Vehicle { get; set; } = null!;

        [Comment("Status of the check")]
        public bool IsCompleted { get; set; }

        [Comment("Identifier of the cretor of this check")]
        public Guid UserId { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;
    }

}
