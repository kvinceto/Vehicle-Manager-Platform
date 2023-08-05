namespace Vmp.Tests.ControllersTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
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
            var viewResult =  result as ViewResult;
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




























































    }
}
