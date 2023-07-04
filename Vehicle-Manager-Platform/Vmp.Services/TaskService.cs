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

        public async Task<bool> CompleteTaskByIdAsync(int taskId)
        {
            TaskModel task = await dbContext.Tasks
                .FirstAsync(t => t.Id ==  taskId);

            task.IsCompleted = true;

            await dbContext.SaveChangesAsync();
            return task.IsCompleted;
        }

        public async Task EditTask(TaskViewModelAdd model)
        {
            TaskModel task = await dbContext.Tasks
                .FirstAsync(t => t.Id == model.Id);

            task.Name = model.Name;
            task.Description = model.Description;
            task.EndDate = DateTime.Parse(model.Deadline);

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

        public async Task<ICollection<TaskViewModelAll>> GetMyTasks(string? userId)
        {
            ICollection<TaskViewModelAll> tasks = new List<TaskViewModelAll>();

            if (string.IsNullOrEmpty(userId))
            {
                return tasks;
            }

            tasks = await dbContext.Tasks
                .Include(t => t.User)
                .AsNoTracking()
                .Where(t => t.IsCompleted == false)
                .Where(t => t.UserId.ToString() == userId)
                .Select(t => new TaskViewModelAll()
                {
                    Id = t.Id,
                    Name = t.Name,
                    EndDate = t.EndDate.ToString("dd/MM/yyyy"),
                    User = t.User.UserName.ToString()
                })
                .ToListAsync();

            return tasks;
        }

        public async Task<TaskViewModelDetails> GetTaskByIdAsync(int taskId)
        {
            TaskModel? task = await dbContext.Tasks
                .Include(t => t.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
            {
                throw new NullReferenceException();
            }


            TaskViewModelDetails model = new TaskViewModelDetails();
            model.Id = task.Id;
            model.Name = task.Name;
            model.Description = task.Description;
            model.EndDate = task.EndDate.ToString("dd/MM/yyyy");
            model.User = task.User.UserName;
            model.IsCompleted = task.IsCompleted;

            return model;
        }

        public async Task<TaskViewModelAdd> GetTaskByIdForEdit(int taskId)
        {
            TaskModel? task = await dbContext.Tasks
               .Include(t => t.User)
               .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
            {
                throw new NullReferenceException();
            }

            TaskViewModelAdd model = new TaskViewModelAdd()
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                Deadline = task.EndDate.ToString("dd/MM/yyyy")
            };

            return model;
        }
    }
}
