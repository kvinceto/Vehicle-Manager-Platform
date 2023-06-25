namespace Vmp.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;

    using static Vmp.Common.EntityValidationConstants.Owner;

    [Comment("Owner of vehicles")]
    public class Owner
    {
        public Owner()
        {
            Vehicles = new HashSet<Vehicle>();
        }

        [Comment("Owner identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Name of the owner")]
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Comment("Additional info for the owner")]
        [MaxLength(InfoMaxLength)]
        public string? Info { get; set; }

        [Comment("Status of the owner")]
        public bool IsInactive { get; set; } = false;

        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
