namespace Vmp.Web.ViewModels.VehicleViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Vmp.Common.EntityValidationConstants.Vehicle;

    public class VehicleViewModelAdd
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(NumberMaxLength, MinimumLength = NumberMinLength)]
        public string Number { get; set; } = null!;

        [Required(AllowEmptyStrings = false)]
        [StringLength(VinMaxLength, MinimumLength = VinMinLength)]
        public string VIN { get; set; } = null!;

        [MaxLength(ModelMaxLength)]
        public string? Model { get; set; }

        [MaxLength(MakeMaxLength)]
        public string? Make { get; set; }

        [Range(MileageMinValue, MileageMaxValue)]
        public int Mileage { get; set; }

        [Range((double)FuelQuantityMinValue, double.MaxValue)]
        public decimal FuelQuantity { get; set; }

        [Range((double)FuelCapacityMinValue, double.MaxValue)]
        public decimal FuelCapacity { get; set; }

        [Range(1, int.MaxValue)]
        public int OwnerId { get; set; }

        [Range((double)FuelCostRateMinValue, double.MaxValue)]
        public decimal FuelCostRate { get; set; }

        [MaxLength(ImgUrlMaxLength)]
        public string? ModelImgUrl { get; set; }
    }
}
