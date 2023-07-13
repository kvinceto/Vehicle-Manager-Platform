namespace Vmp.Web.ViewModels.VehicleViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Vmp.Common.EntityValidationConstants.Vehicle;

    public class VehicleViewModelAdd
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(NumberMaxLength, MinimumLength = NumberMinLength, ErrorMessage = NumberErrorMessage)]
        public string Number { get; set; } = null!;

        [Required(AllowEmptyStrings = false)]
        [StringLength(VinMaxLength, MinimumLength = VinMinLength, ErrorMessage = VinErrorMessage)]
        public string VIN { get; set; } = null!;

        [MaxLength(ModelMaxLength, ErrorMessage = ModelErrorMessage)]
        public string? Model { get; set; }

        [MaxLength(MakeMaxLength, ErrorMessage = MakeErrorMessage)]
        public string? Make { get; set; }

        [Range(MileageMinValue, MileageMaxValue, ErrorMessage = MileageErrorMessage)]
        public int Mileage { get; set; }

        [Range((double)FuelQuantityMinValue, double.MaxValue, ErrorMessage = FuelErrorMessage)]
        public decimal FuelQuantity { get; set; }

        [Range((double)FuelCapacityMinValue, double.MaxValue, ErrorMessage = FuelErrorMessage)]
        public decimal FuelCapacity { get; set; }

        public int OwnerId { get; set; }

        [Range((double)FuelCostRateMinValue, double.MaxValue, ErrorMessage = FuelCostRateErrorMessage)]
        public decimal FuelCostRate { get; set; }

        [MaxLength(ImgUrlMaxLength)]
        public string? ModelImgUrl { get; set; }
    }
}
