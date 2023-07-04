namespace Vmp.Web.ViewModels.TaskViewModels
{
    public class TaskViewModelDetails
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string EndDate { get; set; } = null!;

        public string User { get; set; } = null!;

        public bool IsCompleted { get; set; }
    }
}
