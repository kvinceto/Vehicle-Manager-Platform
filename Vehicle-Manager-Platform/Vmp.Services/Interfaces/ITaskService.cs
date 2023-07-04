namespace Vmp.Services.Interfaces
{
    using Vmp.Web.ViewModels.TaskViewModels;

    public interface ITaskService
    {
        Task<ICollection<TaskViewModelAll>> GetAllActiveTasksAsync();

        Task AddNewTaskAsync(TaskViewModelAdd model, string? userId);

        Task<TaskViewModelDetails> GetTaskByIdAsync(int taskId);

        Task<ICollection<TaskViewModelAll>> GetMyTasks(string? userId);

        Task<TaskViewModelAdd> GetTaskByIdForEdit(int taskId);

        Task EditTask(TaskViewModelAdd model);

        Task<bool> CompleteTaskByIdAsync(int taskId);
    }
}
