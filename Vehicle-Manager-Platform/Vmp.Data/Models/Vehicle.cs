namespace Vmp.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;

    using static Vmp.Common.EntityValidationConstants.Vehicle;

    [Comment("Vehicle")]
    public class Vehicle
    {
        public Vehicle()
        {
            MileageChecks = new HashSet<MileageCheck>();
            DateChecks = new HashSet<DateCheck>();
            Waybills = new HashSet<Waybill>();
        }

        [Comment("Vehicle licence plate or unique identifier")]
        [Key]
        [MaxLength(NumberMaxLength)]
        public string Number { get; set; } = null!;

        [Comment("Vehicle VIN")]
        [Required]
        [MaxLength(VinMaxLength)]
        public string VIN { get; set; } = null!;

        [Comment("Vehicle model name")]
        [MaxLength(ModelMaxLength)]
        public string? Model { get; set; }

        [Comment("Vehicle make name")]
        [MaxLength(MakeMaxLength)]
        public string? Make { get; set; }

        [Comment("Vehicle current mileage")]
        [Required]
        public int Mileage { get; set; }

        [Comment("Vehicle current fuel quantity")]
        [Required]
        public decimal FuelQuantity { get; set; }

        [Comment("Vehicle maximum fuel capacity")]
        [Required]
        public decimal FuelCapacity { get; set; }

        [Comment("The unique identifier of the owner of this vehicle")]
        [Required]
        public int OwnerId { get; set; }

        public virtual Owner Owner { get; set; } = null!;

        public virtual ICollection<Waybill> Waybills { get; set; }

        [Comment("Consumption of fuel, per 100 km or 1 machine hour, for this vehicle")]
        [Required]
        public decimal FuelCostRate { get; set; }

        [Comment("Status of the vehicle")]
        public bool IsDeleted { get; set; }

        public virtual ICollection<MileageCheck> MileageChecks { get; set; }

        public virtual ICollection<DateCheck> DateChecks { get; set; }
    }
}
