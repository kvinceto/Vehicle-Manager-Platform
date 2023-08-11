namespace Vmp.Tests.ControllersTests
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    using Moq;

    using Vmp.Services.Interfaces;
    using Vmp.Web.Controllers;

    [TestFixture]
    public class ExcellControllerTests
    {
        private Mock<ClaimsPrincipal> userMock;
        protected ControllerContext testControllerContext;
        private string testUserId = "5c35b64b-b218-4548-ba50-5b75879a422f";
        private ExcellController excellController;
        private Mock<IExcelService> excellServices;

        [SetUp]
        public void SetUp()
        {
            userMock = new Mock<ClaimsPrincipal>();

            userMock.Setup(mock => mock
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, testUserId));

            excellServices = new Mock<IExcelService>();

            testControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userMock.Object }
            };

            excellController = new ExcellController(excellServices.Object)
            {
                ControllerContext = testControllerContext
            };

            excellController.TempData = new TempDataDictionary(
             new DefaultHttpContext(),
             Mock.Of<ITempDataProvider>());
        }

        [Test]
        public async Task ExportWaybillReturnsFile()
        {
            excellServices.Setup(s => s.GenerateExcelFileWaybillAsync(1))
                .ReturnsAsync(new byte[1]);

            var result = await excellController.ExportWaybill(1);
            var fileResult = result as FileResult;

            Assert.IsNotNull(fileResult);
        }

        [Test]
        public async Task ExportWaybillRedirects()
        {
            excellServices.Setup(s => s.GenerateExcelFileWaybillAsync(1))
                .ThrowsAsync(new Exception());

            var result = await excellController.ExportWaybill(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Home"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("Index"));
        }

        [Test]
        public async Task ExportVehicleReturnsFile()
        {
            excellServices.Setup(s => s.GenerateExcelFileVehicleAsync("th1234ht"))
                .ReturnsAsync(new byte[1]);

            var result = await excellController.ExportVehicle("th1234ht");
            var fileResult = result as FileResult;

            Assert.IsNotNull(fileResult);
        }

        [Test]
        public async Task ExportVehicleRedirects()
        {
            excellServices.Setup(s => s.GenerateExcelFileVehicleAsync("th1234ht"))
                .ThrowsAsync(new Exception());

            var result = await excellController.ExportVehicle("th1234ht");
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Home"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("Index"));
        }

        [Test]
        public async Task ExportAllTasksReturnsFile()
        {
            excellServices.Setup(s => s.GenerateExcelFileAllTasksAsync())
                .ReturnsAsync(new byte[1]);

            var result = await excellController.ExportAllTasks();
            var fileResult = result as FileResult;

            Assert.IsNotNull(fileResult);
        }

        [Test]
        public async Task ExportAllTasksRedirects()
        {
            excellServices.Setup(s => s.GenerateExcelFileAllTasksAsync())
                .ThrowsAsync(new Exception());

            var result = await excellController.ExportAllTasks();
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Home"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("Index"));
        }
    }
}
