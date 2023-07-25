namespace Vmp.Web.ViewModels.DateCheckViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Vmp.Web.ViewModels.VehicleViewModels;

    using static Vmp.Common.EntityValidationConstants.DateCheck;

    public class DateCheckViewModelAdd
    {
        public DateCheckViewModelAdd()
        {
            this.Vehicles = new HashSet<VehicleViewModelShortInfo>();
        }

        [Required(AllowEmptyStrings = false)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Required(AllowEmptyStrings = false)]
        public string EndDate { get; set; } = null!;

        [Required(AllowEmptyStrings = false)]
        public string VehicleNumber { get; set; } = null!;

        public ICollection<VehicleViewModelShortInfo> Vehicles { get; set; }
    }
}
