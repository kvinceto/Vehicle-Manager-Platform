namespace Vmp.Web.ViewModels.CostCenterViewModels
{
    using System.ComponentModel.DataAnnotations;

    using static Vmp.Common.EntityValidationConstants.CostCenter;

    public class CostCenterViewModelAdd
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = NameErrorMessage)]
        public string Name { get; set; } = null!;
    }
}
