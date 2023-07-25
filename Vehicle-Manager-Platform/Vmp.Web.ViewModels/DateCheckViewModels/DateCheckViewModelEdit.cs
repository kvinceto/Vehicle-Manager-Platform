namespace Vmp.Web.ViewModels.DateCheckViewModels
{
    using System.ComponentModel.DataAnnotations;

    using Vmp.Web.ViewModels.VehicleViewModels;

    using static Vmp.Common.EntityValidationConstants.DateCheck;

    public class DateCheckViewModelEdit
    {
        public DateCheckViewModelEdit()
        {
            this.Vehicles = new HashSet<VehicleViewModelShortInfo>();
        }

        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Required(AllowEmptyStrings = false)]
        public string EndDate { get; set; } = null!;

        [Required(AllowEmptyStrings = false)]
        public string VehicleNumber { get; set; } = null!;

        public string? UserId { get; set; }

        public ICollection<VehicleViewModelShortInfo> Vehicles { get; set; }
    }
}
