namespace Vmp.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;

    using static Vmp.Common.EntityValidationConstants.Waybill;

    [Comment("Waybill of a vehicle")]
    public class Waybill
    {
        [Comment("Waybill identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Date of issue of the waybill")]
        [Required]
        public DateTime Date { get; set; }

        [Comment("Licence plate of the vehicle corresponding with this waybill")]
        public string VehicleNumber { get; set; } = null!;

        public virtual Vehicle Vehicle { get; set; } = null!;

        [Comment("Mileage at the beginning of the day for the corresponding vehicle")]
        [Required]
        public int MileageStart { get; set; }

        [Comment("Mileage at the end of the day for the corresponding vehicle")]
        [Required]
        public int MileageEnd { get; set; }

        [Comment("Mileage traveled for the day")]
        [Required]
        public int MileageTraveled { get; set; }

        [Comment("Traveled route for the day")]
        [Required]
        [MaxLength(RouteTraveledMaxLength)]
        public string RouteTraveled { get; set; } = null!;

        [Comment("Fuel at the beginning of the day for the corresponding vehicle")]
        [Required]
        public decimal FuelQuantityStart { get; set; }

        [Comment("Fuel at the end of the day for the corresponding vehicle")]
        [Required]
        public decimal FuelQuantityEnd { get; set; }

        [Comment("Fuel consumed for the day for the corresponding vehicle")]
        [Required]
        public decimal FuelConsumed { get; set; }

        [Comment("Fuel loaded for the day for the corresponding vehicle")]
        [Required]
        public decimal FuelLoaded { get; set; }

        [Comment("Status of the waybill")]
        public bool IsDeleted { get; set; }

        [Comment("Additional info for this waybill")]
        [MaxLength(InfoMaxLength)]
        public string? Info { get; set; }

        [Comment("Date and time of the creation of the waybill")]
        [Required]
        public DateTime DateCreated { get; set; }

        [Comment("Unique identifier for the cost center corresponding with this waybill")]
        public int CostCenterId { get; set; }

        public virtual CostCenter CostCenter { get; set; } = null!;

        [Comment("Identifier of the user who created this waybill")]
        public Guid UserId { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;
    }
}
