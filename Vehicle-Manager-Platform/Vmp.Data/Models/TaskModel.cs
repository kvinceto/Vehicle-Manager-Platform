namespace Vmp.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;

    using static Vmp.Common.EntityValidationConstants.TaskModel;

    [Comment("Task for a specific user")]
    public class TaskModel
    {
        [Comment("Task identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Name of the task")]
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Comment("Task deadline")]
        [Required]
        public DateTime EndDate { get; set; }

        [Comment("Identifier of the user corresponding with this task")]
        [Required]        
        public string UserId { get; set; } = null!;

        public virtual ApplicationUser User { get; set; } = null!;

        [Comment("Status of the task")]
        public bool IsCompleted { get; set; }
    }
}
