namespace Vmp.Web.ViewModels.TaskViewModels
{
    using System.ComponentModel;

    public class TaskViewModelAll
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        [DisplayName("Task deadline")]
        public string EndDate { get; set; } = null!;

        [DisplayName("Task owner username")]
        public string User { get; set; } = null!;
    }
}
