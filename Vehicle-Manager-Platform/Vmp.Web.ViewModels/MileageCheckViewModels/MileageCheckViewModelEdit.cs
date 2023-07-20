namespace Vmp.Web.ViewModels.MileageCheckViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Vmp.Web.ViewModels.VehicleViewModels;

    using static Vmp.Common.EntityValidationConstants.MileageCheck;

    public class MileageCheckViewModelEdit
    {
        public MileageCheckViewModelEdit()
        {
            this.Vehicles = new HashSet<VehicleViewModelShortInfo>();
        }

        public int? Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Range(ExpectedMileageMinValue, ExpectedMileageMaxValue)]
        public int ExpectedMileage { get; set; }

        public string VehicleNumber { get; set; } = null!;

        public string? UserId { get; set; }

        public ICollection<VehicleViewModelShortInfo> Vehicles { get; set; }

    }
}
