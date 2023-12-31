﻿namespace Vmp.Services.Interfaces
{
    using Vmp.Web.ViewModels.TaskViewModels;

    public interface ITaskService
    {
        Task<ICollection<TaskViewModelAll>> GetAllActiveTasksAsync();

        Task AddNewTaskAsync(TaskViewModelAdd model, string? userId);

        Task<TaskViewModelDetails> GetTaskByIdAsync(int taskId);

        Task<ICollection<TaskViewModelAll>> GetMyTasksAsync(string? userId);

        Task<TaskViewModelAdd> GetTaskByIdForEditAsync(int taskId);

        Task<bool> EditTaskAsync(TaskViewModelAdd model, string? myId);

        Task<bool> CompleteTaskByIdAsync(int taskId, string? myId);
    }
}
