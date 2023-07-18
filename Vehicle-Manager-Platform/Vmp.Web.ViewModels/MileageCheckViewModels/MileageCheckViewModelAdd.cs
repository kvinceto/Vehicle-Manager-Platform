namespace Vmp.Web.ViewModels.MileageCheckViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Vmp.Web.ViewModels.VehicleViewModels;

    using static Vmp.Common.EntityValidationConstants.MileageCheck;

    public class MileageCheckViewModelAdd
    {
        public MileageCheckViewModelAdd()
        {
            this.Vehicles = new HashSet<VehicleViewModelShortInfo>();
        }

        [Required(AllowEmptyStrings = false)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Range(ExpectedMileageMinValue, ExpectedMileageMaxValue)]
        public int ExpectedMileage { get; set; }

        public string VehicleNumber { get; set; } = null!;

        public ICollection<VehicleViewModelShortInfo> Vehicles { get; set; }

    }
}
