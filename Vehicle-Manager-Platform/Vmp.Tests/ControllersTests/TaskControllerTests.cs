namespace Vmp.Tests.ControllersTests
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    using Moq;

    using Vmp.Services.Interfaces;
    using Vmp.Web.Controllers;
    using Vmp.Web.ViewModels.TaskViewModels;

    using static Vmp.Common.NotificationMessagesConstants;

    [TestFixture]
    public class TaskControllerTests
    {
        private Mock<ClaimsPrincipal> userMock;
        protected ControllerContext testControllerContext;
        private string testUserId = "5c35b64b-b218-4548-ba50-5b75879a422f";
        private TaskController taskController;
        private Mock<ITaskService> taskService;

        [SetUp]
        public void SetUp()
        {
            userMock = new Mock<ClaimsPrincipal>();

            userMock.Setup(mock => mock
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, testUserId));

            taskService = new Mock<ITaskService>();

            testControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userMock.Object }
            };

            taskController = new TaskController(taskService.Object)
            {
                ControllerContext = testControllerContext
            };

            taskController.TempData = new TempDataDictionary(
             new DefaultHttpContext(),
             Mock.Of<ITempDataProvider>());
        }

        [Test]
        public async Task AllActionShouldReturnView()
        {
            taskService
                .Setup(s => s.GetAllActiveTasksAsync())
                .ReturnsAsync(new List<TaskViewModelAll>());

            var result = await taskController.All();
            var viewResult = result as ViewResult;
            var expectedModel = new List<TaskViewModelAll>();

            Assert.NotNull(viewResult);
            CollectionAssert.AreEqual(expectedModel, (List<TaskViewModelAll>)viewResult.Model);
            Assert.That(viewResult.TempData.ContainsKey(InformationMessage));
            Assert.That(viewResult.TempData[InformationMessage], Is.EquivalentTo("All active tasks are viewed"));
        }

        [Test]
        public async Task AllActionShouldRedirect()
        {
            taskService
                .Setup(s => s.GetAllActiveTasksAsync())
                .ThrowsAsync(new Exception());

            var result = await taskController.All();
            var viewResult = result as RedirectToActionResult;

            Assert.NotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(viewResult.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task MineActionShouldReturnView()
        {
            taskService
                .Setup(s => s.GetMyTasksAsync(testUserId))
                .ReturnsAsync(new List<TaskViewModelAll>());

            var result = await taskController.Mine();
            var viewResult = result as ViewResult;
            var expectedModel = new List<TaskViewModelAll>();

            Assert.NotNull(viewResult);
            CollectionAssert.AreEqual(expectedModel, (List<TaskViewModelAll>)viewResult.Model);
            Assert.That(viewResult.TempData.ContainsKey(InformationMessage));
            Assert.That(viewResult.TempData[InformationMessage], Is.EquivalentTo("All my active tasks are viewed"));
        }

        [Test]
        public async Task MineActionShouldRedirect()
        {
            taskService
                .Setup(s => s.GetMyTasksAsync(testUserId))
                .ThrowsAsync(new Exception());

            var result = await taskController.Mine();
            var viewResult = result as RedirectToActionResult;

            Assert.NotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EqualTo("Home"));
            Assert.That(viewResult.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public void AddShouldReturnView()
        {
            var result = taskController.Add();
            var viewResult = result as ViewResult;

            Assert.NotNull(viewResult);
        }

        [Test]
        public async Task AddPostShouldRedirect()
        {
            TaskViewModelAdd taskViewModelAdd = new TaskViewModelAdd();
            taskService
                .Setup(s => s.AddNewTaskAsync(taskViewModelAdd, testUserId));

            var result = await taskController.Add(taskViewModelAdd);
            var redirect = result as RedirectToActionResult;

            Assert.NotNull(redirect);
            Assert.That(redirect.ControllerName, Is.EqualTo("Task"));
            Assert.That(redirect.ActionName, Is.EqualTo("Mine"));
        }

        [Test]
        public async Task AddPostShouldReturnView()
        {
            TaskViewModelAdd taskViewModelAdd = new TaskViewModelAdd();
            taskService
                .Setup(s => s.AddNewTaskAsync(taskViewModelAdd, testUserId))
                .ThrowsAsync(new Exception());

            var result = await taskController.Add(taskViewModelAdd);
            var viewResult = result as ViewResult;

            Assert.NotNull(viewResult);
        }

        [Test]
        public async Task EditRetursView()
        {
            taskService
                .Setup(s => s.GetTaskByIdForEditAsync(1))
                .ReturnsAsync(new TaskViewModelAdd());

            var result = await taskController.Edit(1);
            var viewResult = result as ViewResult;

            Assert.NotNull(viewResult);
            Assert.IsTrue(viewResult.TempData.ContainsKey(WarningMessage));
            Assert.That(viewResult.TempData[WarningMessage], Is.EquivalentTo("Task viewed for edit"));
        }

        [Test]
        public async Task EditRedirects()
        {
            taskService
                .Setup(s => s.GetTaskByIdForEditAsync(1))
                .ThrowsAsync(new Exception());

            var result = await taskController.Edit(1);
            var viewResult = result as RedirectToActionResult;

            Assert.NotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Task"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("Mine"));
        }

        [Test]
        public async Task EditPostRedirects1()
        {
            taskService
                .Setup(s => s.EditTaskAsync(new TaskViewModelAdd(), testUserId))
                .ReturnsAsync(false);

            var result = await taskController.Edit(new TaskViewModelAdd());
            var viewResult = result as RedirectToActionResult;

            Assert.NotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Task"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("Mine"));
        }

        [Test]
        public async Task EditPostRedirects2()
        {
            taskService
                .Setup(s => s.EditTaskAsync(new TaskViewModelAdd(), testUserId))
                .ReturnsAsync(true);

            var result = await taskController.Edit(new TaskViewModelAdd());
            var viewResult = result as RedirectToActionResult;

            Assert.NotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Task"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("Mine"));
        }

        [Test]
        public async Task EditPostRedirects3()
        {
            TaskViewModelAdd taskViewModelAdd = new TaskViewModelAdd();
            taskService
                .Setup(s => s.EditTaskAsync(taskViewModelAdd, testUserId))
                .ThrowsAsync(new Exception());

            var result = await taskController.Edit(new TaskViewModelAdd());
            var viewResult = result as RedirectToActionResult;

            Assert.NotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Task"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("Mine"));
        }

        [Test]
        public async Task ViewTaskReturnsView()
        {
            taskService
                .Setup(s => s.GetTaskByIdAsync(1))
                .ReturnsAsync(new TaskViewModelDetails());

            var result = await taskController.ViewTask(1);
            var viewResult = result as ViewResult;

            Assert.NotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<TaskViewModelDetails>());
            Assert.IsTrue(viewResult.TempData.ContainsKey(InformationMessage));
            Assert.That(viewResult.TempData[InformationMessage], Is.EquivalentTo("Task Info Viewed"));
        }

        [Test]
        public async Task ViewTaskRedirects()
        {
            taskService
                .Setup(s => s.GetTaskByIdAsync(1))
                .ThrowsAsync(new Exception());

            var result = await taskController.ViewTask(1);
            var viewResult = result as RedirectToActionResult;

            Assert.NotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Task"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task CompleteRedirects1()
        {
            taskService
                .Setup(s => s.CompleteTaskByIdAsync(1, testUserId))
                .ReturnsAsync(true);

            var result = await taskController.Complete(new TaskViewModelAdd() { Id = 1 });
            var viewResult = result as RedirectToActionResult;

            Assert.NotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Task"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("Mine"));
        }

        [Test]
        public async Task CompleteRedirects2()
        {
            taskService
                .Setup(s => s.CompleteTaskByIdAsync(1, testUserId))
                .ReturnsAsync(false);

            var result = await taskController.Complete(new TaskViewModelAdd() { Id = 1 });
            var viewResult = result as RedirectToActionResult;

            Assert.NotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Task"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("Mine"));
        }

        [Test]
        public async Task CompleteRedirects3()
        {
            taskService
                .Setup(s => s.CompleteTaskByIdAsync(1, testUserId))
                .ThrowsAsync(new Exception());

            var result = await taskController.Complete(new TaskViewModelAdd() { Id = 1 });
            var viewResult = result as RedirectToActionResult;

            Assert.NotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Task"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("Mine"));
        }
    }
}
