namespace Vmp.Web.ViewModels.OwnerViewModels
{
    using System.ComponentModel.DataAnnotations;

    using static Vmp.Common.EntityValidationConstants.Owner;

    public class OwnerViewModelEdit
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = NameErrorMessage)]
        public string Name { get; set; } = null!;

        [StringLength(InfoMaxLength, ErrorMessage = InfoErrorMessage)]
        public string? Info { get; set; }
    }
}
