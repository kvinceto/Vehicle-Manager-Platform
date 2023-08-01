namespace Vmp.Services
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

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

        /// <summary>
        /// This method creates a new entry in the Database of type TaskModel
        /// </summary>
        /// <param name="model">View model of type TaskViewModelAdd</param>
        /// <param name="userId">The Id of the user as string?</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">If userId is null throws exception</exception>
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

        /// <summary>
        /// This method marks a task entity for completed
        /// </summary>
        /// <param name="taskId">The Id of the task</param>
        /// <param name="myId">This Id of the user</param>
        /// <returns>True or False</returns>
        public async Task<bool> CompleteTaskByIdAsync(int taskId, string? myId)
        {
            TaskModel? task = await dbContext.Tasks
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
            {
                return false;
            }

            if(task.UserId.ToString() != myId)
            {
               return false;
            }

            task.IsCompleted = true;

            await dbContext.SaveChangesAsync();
            return task.IsCompleted;
        }

        /// <summary>
        /// This method makes changes to a entity of type TaskModel
        /// </summary>
        /// <param name="model">View Model with the new data</param>
        /// <param name="myId">The Id of tha task to be edited</param>
        /// <returns>True or False</returns>
        public async Task<bool> EditTaskAsync(TaskViewModelAdd model, string? myId)
        {
            bool isMine = await dbContext.Tasks
                .AnyAsync(t => t.Id == model.Id && t.UserId.ToString() == myId);

            if (!isMine)
            {
                return isMine;
            }

            TaskModel? task = await dbContext.Tasks
                .FirstAsync(t => t.Id == model.Id && t.UserId.ToString() == myId);



            task.Name = model.Name;
            task.Description = model.Description;
            task.EndDate = DateTime.Parse(model.Deadline);

            await dbContext.SaveChangesAsync();
            return isMine;
        }

        /// <summary>
        /// This method returns all active tasks, for all users, ordered by End date accending
        /// </summary>
        /// <returns>Collection of tasks</returns>
        public async Task<ICollection<TaskViewModelAll>> GetAllActiveTasksAsync()
        {

            var tasks = await dbContext.Tasks
                 .Where(t => t.IsCompleted == false)
                 .OrderBy(t => t.EndDate)
                 .AsNoTracking()
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

        /// <summary>
        /// This method returns all active tasks, for one user, ordered by End date accending
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Collection of tasks</returns>
        public async Task<ICollection<TaskViewModelAll>> GetMyTasksAsync(string? userId)
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
                .OrderBy(t => t.EndDate)
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

        /// <summary>
        /// This method returns one task entity
        /// </summary>
        /// <param name="taskId">The Id of the task to be viewed</param>
        /// <returns>Task of type TaskViewModelDetails</returns>
        /// <exception cref="NullReferenceException">If there is no such task throws exception</exception>
        public async Task<TaskViewModelDetails> GetTaskByIdAsync(int taskId)
        {
            TaskModel? task = await dbContext.Tasks
                .Include(t => t.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
            {
                throw new NullReferenceException(nameof(task));
            }


            TaskViewModelDetails model = new TaskViewModelDetails()
            {
                Id = task.Id,
                Name = task.Name,
                EndDate = task.EndDate.ToString("dd/MM/yyyy"),
                User = task.User.UserName.ToString(),
                Description = task.Description
            };           

            return model;
        }

        /// <summary>
        /// This method returns one task with the same Id
        /// </summary>
        /// <param name="taskId">The Id of the task</param>
        /// <returns>View Model of type TaskViewModelAdd</returns>
        /// <exception cref="NullReferenceException">If there is no such task throws exception</exception>
        public async Task<TaskViewModelAdd> GetTaskByIdForEditAsync(int taskId)
        {
            TaskModel? task = await dbContext.Tasks
               .Include(t => t.User)
               .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
            {
                throw new NullReferenceException(nameof(task));
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
