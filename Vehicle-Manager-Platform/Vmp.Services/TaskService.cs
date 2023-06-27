namespace Vmp.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Vmp.Data;
    using Vmp.Data.Models;
    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.TaskViewModels;

    public class TaskService : ITaskService
    {
        private readonly VehicleManagerDbContext dbContext;

        public TaskService(VehicleManagerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddNewTaskAsync(TaskViewModelAdd model, string? userId)
        {
            if (userId == null)
            {
               throw new NullReferenceException(nameof(userId));
            }

            TaskModel newTask = new TaskModel()
            {
                Name = model.Name,
                Description = model.Description,
                EndDate = DateTime.Parse(model.Deadline),
                IsCompleted = false,
                UserId = Guid.Parse(userId)
            };

            await dbContext.Tasks.AddAsync(newTask);
            await dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<TaskViewModelAll>> GetAllActiveTasksAsync()
        {
            var tasks = await dbContext.Tasks
                 .Where(t => t.IsCompleted == false)
                 .OrderBy(t => t.EndDate)
                 .Select(t => new TaskViewModelAll()
                 {
                     Id = t.Id,
                     Name = t.Name,
                     EndDate = t.EndDate.ToString("dd/MM/yyyy"),
                     User = t.User.UserName
                 })
                 .ToArrayAsync();

            return tasks;
        }
    }
}
