namespace Vmp.Web.ViewModels.TaskViewModels
{
    using System.ComponentModel.DataAnnotations;

    using static Vmp.Common.EntityValidationConstants.TaskModel;

    public class TaskViewModelAdd
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = NameErrorMessage)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = DescriptionErrorMessage)]
        public string Description { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = DeadlineErrorMessage)]
        public string Deadline { get; set; } = null!;
    }
}
