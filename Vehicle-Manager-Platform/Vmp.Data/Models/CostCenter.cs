namespace Vmp.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.EntityFrameworkCore;

    using static Vmp.Common.EntityValidationConstants.CostCenter;

    [Comment("Department, Construction site or other cost center")]
    public class CostCenter
    {
        public CostCenter()
        {
            Waybills = new HashSet<Waybill>();
        }

        [Comment("Cost center identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Name of the cost center")]
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Waybill> Waybills { get; set; }

        [Comment("Status of the cost center")]
        public bool IsClosed { get; set; }
    }
}
