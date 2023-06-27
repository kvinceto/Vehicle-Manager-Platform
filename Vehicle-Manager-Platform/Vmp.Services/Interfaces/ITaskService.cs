namespace Vmp.Services.Interfaces
{
    using Vmp.Web.ViewModels.TaskViewModels;

    public interface ITaskService
    {
        Task<ICollection<TaskViewModelAll>> GetAllActiveTasksAsync();

        Task AddNewTaskAsync(TaskViewModelAdd model, string? userId);
    }
}
