namespace Vmp.Tests.ServicesTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Vmp.Data;
    using Vmp.Data.Models;
    using Vmp.Services;
    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.TaskViewModels;

    [TestFixture]
    public class TaskServicesTests
    {
        private VehicleManagerDbContext dbContext;
        private ITaskService taskService;
        private string testUserId = "5c35b64b-b218-4548-ba50-5b75879a422f";
        private TaskModel newTask;
        private TaskViewModelAdd modelAdd;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<VehicleManagerDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;
            dbContext = new VehicleManagerDbContext(options);

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            ApplicationUser user = new ApplicationUser()
            {
                Id = Guid.Parse(testUserId),
                UserName = "user@user.bg",
                NormalizedUserName = "user@user.bg".ToUpper(),
                Email = "user@user.bg",
                NormalizedEmail = "user@user.bg".ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = "user@user.bg".ToUpper(),
            };

            await dbContext.AspNetUsers.AddAsync(user);
            await dbContext.SaveChangesAsync();

            taskService = new TaskService(dbContext);

            newTask = new TaskModel()
            {
                Id = 1,
                Name = "Test",
                Description = "Test",
                EndDate = DateTime.Parse("03/07/2023"),
                IsCompleted = false,
                UserId = Guid.Parse(testUserId)
            };

            modelAdd = new TaskViewModelAdd()
            {
                Id = newTask.Id,
                Name = newTask.Name,
                Description = newTask.Description,
                Deadline = newTask.EndDate.ToString("dd/MM/yyyy")
            };
        }

        [Test]
        public void AddNewTaskAsyncShouldThrowNullWithNullUserId()
        {
            TaskViewModelAdd model = new TaskViewModelAdd();

            Assert.ThrowsAsync<NullReferenceException>(async () => await taskService.AddNewTaskAsync(model, null));
        }

        [Test]
        public async Task AddNewTaskAsyncShouldAddTask()
        {
            await taskService.AddNewTaskAsync(modelAdd, testUserId);
            var taskFromDb = await taskService.GetTaskByIdAsync(modelAdd.Id!.Value);

            Assert.IsNotNull(taskFromDb);

            TaskViewModelAdd modelfromDb = new TaskViewModelAdd
            {
                Id = taskFromDb.Id,
                Name = taskFromDb.Name,
                Description = taskFromDb.Description,
                Deadline = taskFromDb.EndDate
            };

            Assert.That(modelAdd.Id, Is.EqualTo(modelfromDb.Id));
            Assert.That(modelAdd.Name, Is.EqualTo(modelfromDb.Name));
            Assert.That(modelAdd.Description, Is.EqualTo(modelfromDb.Description));
            Assert.That(modelAdd.Deadline, Is.EqualTo(modelfromDb.Deadline));
        }

        [Test]
        public async Task CompleteTaskByIdAsyncShouldCompleteTask()
        {
            await taskService.AddNewTaskAsync(modelAdd, testUserId);
            await taskService.CompleteTaskByIdAsync(modelAdd.Id!.Value, testUserId);

            var task = await dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == modelAdd.Id.Value);

            Assert.IsNotNull(task);
            Assert.That(task.IsCompleted);
        }

        [Test]
        public async Task CompleteTaskByIdAsyncShouldCompleteTask2()
        {
            await taskService.AddNewTaskAsync(modelAdd, testUserId);
            bool result = await taskService.CompleteTaskByIdAsync(modelAdd.Id!.Value, testUserId);

            Assert.IsTrue(result);
        }

        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        [TestCase(0)]
        public async Task CompleteTaskByIdAsyncShouldReturnFalseInvalidTaskId(int id)
        {
            await taskService.AddNewTaskAsync(modelAdd, testUserId);
            bool result = await taskService.CompleteTaskByIdAsync(id, testUserId);
            Assert.IsFalse(result);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("invalid")]
        public async Task CompleteTaskByIdAsyncShouldReturnFalseInvalidUserId(string id)
        {
            await taskService.AddNewTaskAsync(modelAdd, testUserId);
            bool result = await taskService.CompleteTaskByIdAsync(modelAdd.Id!.Value, id);
            Assert.IsFalse(result);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("invalid")]
        public async Task EditTaskAsyncShouldReturnFalseNotUsersTask(string id)
        {
            await taskService.AddNewTaskAsync(modelAdd, testUserId);

            TaskViewModelAdd editModel = new TaskViewModelAdd()
            {
                Id = 256,
                Name = modelAdd.Name,
                Deadline = modelAdd.Deadline,
                Description = modelAdd.Description
            };

            bool result = await taskService.EditTaskAsync(editModel, id);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task EditTaskAsyncShouldReturnTrueAndEditTask()
        {
            await taskService.AddNewTaskAsync(modelAdd, testUserId);

            TaskViewModelAdd editModel = new TaskViewModelAdd()
            {
                Id = modelAdd.Id!.Value,
                Name = "New name",
                Deadline = "31.12.2023",
                Description = "New description"
            };

            bool result = await taskService.EditTaskAsync(editModel, testUserId);

            var task = await dbContext.Tasks
                .FirstOrDefaultAsync(t => t.Id == editModel.Id);

            string date = task.EndDate.ToString("dd/MM/yyyy");
            Assert.IsNotNull(task);
            Assert.IsTrue(result);
            Assert.That(task.Id, Is.EqualTo(editModel.Id));
            Assert.That(task.Name, Is.EqualTo(editModel.Name));
            Assert.That(task.Description, Is.EqualTo(editModel.Description));
            Assert.That(date, Is.EqualTo(editModel.Deadline));
        }

        [Test]
        public async Task GetAllActiveTasksAsyncShouldReturnEmptyCollection()
        {
            var tasks = await taskService.GetAllActiveTasksAsync();

            Assert.IsNotNull(tasks);
            Assert.That(tasks.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetAllActiveTasksAsyncShouldReturnCollection()
        {
            await dbContext.Tasks.AddAsync(newTask);
            await dbContext.SaveChangesAsync();

            newTask = new TaskModel()
            {
                Id = 2,
                Name = "Test2",
                Description = "Test2",
                EndDate = DateTime.Parse("04/08/2023"),
                IsCompleted = false,
                UserId = Guid.Parse(testUserId)
            };

            await dbContext.Tasks.AddAsync(newTask);
            await dbContext.SaveChangesAsync();

            var tasks = await taskService.GetAllActiveTasksAsync();

            Assert.IsNotNull(tasks);
            Assert.That(tasks.Count(), Is.EqualTo(2));

            int n = 0;
            foreach (var task in tasks)
            {
                n++;
                Assert.That(task.Id, Is.EqualTo(n));
            }
        }

        [TestCase(null)]
        [TestCase("")]
        public async Task GetMyTasksAsyncShouldReturnEmptyCollection(string id)
        {
            var tasks = await taskService.GetMyTasksAsync(id);

            Assert.IsNotNull(tasks);
            Assert.That(tasks.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetMyTasksAsyncShouldReturnCollectionAndInOrder()
        {
            await dbContext.Tasks.AddAsync(newTask);
            await dbContext.SaveChangesAsync();

            newTask = new TaskModel()
            {
                Id = 2,
                Name = "Test2",
                Description = "Test2",
                EndDate = DateTime.Parse("04/08/2023"),
                IsCompleted = false,
                UserId = Guid.Parse(testUserId)
            };

            await dbContext.Tasks.AddAsync(newTask);
            await dbContext.SaveChangesAsync();

            var tasks = await taskService.GetMyTasksAsync(testUserId);

            Assert.IsNotNull(tasks);
            Assert.That(tasks.Count(), Is.EqualTo(2));

            int n = 0;
            foreach (var task in tasks)
            {
                n++;
                Assert.That(task.Id, Is.EqualTo(n));
            }
        }

        [Test]
        public async Task GetTaskByIdAsyncShouldThrowNullInvalidTaskId()
        {
            await taskService.AddNewTaskAsync(modelAdd, testUserId);
            Assert.ThrowsAsync<NullReferenceException>(async () => await taskService.GetTaskByIdAsync(256));
        }

        [Test]
        public async Task GetTaskByIdAsyncShouldReturnTask()
        {
            await taskService.AddNewTaskAsync(modelAdd, testUserId);
            var task = await taskService.GetTaskByIdAsync(modelAdd.Id!.Value);

            Assert.IsNotNull(task);
            Assert.That(task.Id, Is.EqualTo(modelAdd.Id.Value));
            Assert.That(task.Name, Is.EqualTo(modelAdd.Name));
            Assert.That(task.Description, Is.EqualTo(modelAdd.Description));
            Assert.That(task.EndDate, Is.EqualTo(modelAdd.Deadline));
            Assert.That(task.User, Is.EqualTo("user@user.bg"));
        }

        [Test]
        public async Task GetTaskByIdForEditAsyncShouldThrowNullInvalidTaskId()
        {
            await taskService.AddNewTaskAsync(modelAdd, testUserId);
            Assert.ThrowsAsync<NullReferenceException>(async () => await taskService.GetTaskByIdForEditAsync(256));
        }

        [Test]
        public async Task GetTaskByIdForEditAsyncShouldReturnTask()
        {
            await taskService.AddNewTaskAsync(modelAdd, testUserId);
            var task = await taskService.GetTaskByIdForEditAsync(modelAdd.Id!.Value);

            Assert.IsNotNull(task);
            Assert.That(task.Id, Is.EqualTo(modelAdd.Id.Value));
            Assert.That(task.Name, Is.EqualTo(modelAdd.Name));
            Assert.That(task.Description, Is.EqualTo(modelAdd.Description));
            Assert.That(task.Deadline, Is.EqualTo(modelAdd.Deadline));
        }

        [TearDown]
        public async Task TearDown()
        {
            await dbContext.Database.EnsureDeletedAsync();
        }
    }
}
