namespace Vmp.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;

    using static Vmp.Common.EntityValidationConstants.MileageCheck;

    [Comment("Check to reach a certain Mileage")]
    public class MileageCheck
    {
        [Comment("Check identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Name of the check")]
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Comment("Mileage to be reached")]
        [Required]
        public int ExpectedMileage { get; set; }

        [Comment("Licence plate of the Vehicle corresponding with this check")]
        public string VehicleNumber { get; set; } = null!;

        public virtual Vehicle Vehicle { get; set; } = null!;

        [Comment("Status of the check: Completed or Ongoing")]
        public bool IsCompleted { get; set; }

        [Comment("Identifier of the cretor of this check")]
        public string UserId { get; set; } = null!;

        public virtual ApplicationUser User { get; set; } = null!;
    }
}
