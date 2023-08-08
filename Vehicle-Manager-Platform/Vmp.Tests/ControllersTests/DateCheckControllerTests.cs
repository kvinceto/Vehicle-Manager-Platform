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
    using Vmp.Web.ViewModels.VehicleViewModels;
    using Vmp.Web.ViewModels.DateCheckViewModels;

    using static Vmp.Common.NotificationMessagesConstants;

    [TestFixture]
    public class DateCheckControllerTests
    {
        private Mock<ClaimsPrincipal> userMock;
        protected ControllerContext testControllerContext;
        private string testUserId = "5c35b64b-b218-4548-ba50-5b75879a422f";
        private DateCheckController dateCheckController;
        private Mock<IDateCheckService> dateCheckService;
        private Mock<IVehicleService> vehicleService;

        [SetUp]
        public void SetUp()
        {
            userMock = new Mock<ClaimsPrincipal>();

            userMock.Setup(mock => mock
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, testUserId));

            dateCheckService = new Mock<IDateCheckService>();
            vehicleService = new Mock<IVehicleService>();

            testControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userMock.Object }
            };

            dateCheckController = new DateCheckController(vehicleService.Object, dateCheckService.Object)
            {
                ControllerContext = testControllerContext
            };

            dateCheckController.TempData = new TempDataDictionary(
             new DefaultHttpContext(),
             Mock.Of<ITempDataProvider>());
        }

        [Test]
        public async Task AddReturnsView()
        {
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ReturnsAsync(new List<VehicleViewModelShortInfo>());

            var result = await dateCheckController.Add();
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<DateCheckViewModelAdd>());
        }

        [Test]
        public async Task AddRedirects()
        {
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ThrowsAsync(new Exception());

            var result = await dateCheckController.Add();
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Home"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("Index"));
        }

        [Test]
        public async Task AddPostRedirects()
        {
            dateCheckService.Setup(s => s.AddAsync(new DateCheckViewModelAdd(), testUserId));

            var result = await dateCheckController.Add(new DateCheckViewModelAdd());
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("DateCheck"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task AllReturnsView1()
        {
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ReturnsAsync(new List<VehicleViewModelShortInfo>());
            dateCheckService.Setup(s => s.GetAllDateChecksAsync())
                .ReturnsAsync(new List<DateCheckViewModelAll>());

            var result = await dateCheckController.All();
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<DateCheckViewModelAllDto>());
            Assert.IsTrue(viewResult.TempData.ContainsKey(InformationMessage));
            Assert.That(viewResult.TempData[InformationMessage], Is.EquivalentTo("All Date checks viewed"));
        }

        [Test]
        public async Task AllReturnsView2()
        {
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ThrowsAsync(new Exception());
            dateCheckService.Setup(s => s.GetAllDateChecksAsync())
             .ThrowsAsync(new Exception());

            var result = await dateCheckController.All();
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<DateCheckViewModelAllDto>());
            Assert.IsTrue(viewResult.TempData.ContainsKey(ErrorMessage));
        }

        [Test]
        public async Task ViewInfoReturnsView()
        {
            dateCheckService.Setup(s => s.GetDateCheckByIdAsync(1))
                .ReturnsAsync(new DateCheckViewModelDetails());

            var result = await dateCheckController.ViewInfo(1);
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<DateCheckViewModelDetails>());
            Assert.IsTrue(viewResult.TempData.ContainsKey(InformationMessage));
        }

        [Test]
        public async Task ViewInfoRedirects()
        {
            dateCheckService.Setup(s => s.GetDateCheckByIdAsync(1))
                .ThrowsAsync(new Exception());

            var result = await dateCheckController.ViewInfo(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("DateCheck"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task EditReturnsViewNoAccess()
        {
            dateCheckService.Setup(s => s.GetCheckByIdForEditAsync(1))
                .ReturnsAsync(new DateCheckViewModelEdit());
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ReturnsAsync(new List<VehicleViewModelShortInfo>());

            var result = await dateCheckController.Edit(1);
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ViewName, Is.EquivalentTo("NoAccess"));
        }

        [Test]
        public async Task EditReturnsViewEdit()
        {
            dateCheckService.Setup(s => s.GetCheckByIdForEditAsync(1))
                .ReturnsAsync(new DateCheckViewModelEdit() { UserId = testUserId });
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ReturnsAsync(new List<VehicleViewModelShortInfo>());

            var result = await dateCheckController.Edit(1);
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.IsTrue(viewResult.TempData.ContainsKey(WarningMessage));
        }

        [Test]
        public async Task EditRedirects()
        {
            dateCheckService.Setup(s => s.GetCheckByIdForEditAsync(1))
                .ThrowsAsync(new Exception());
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ThrowsAsync(new Exception());

            var result = await dateCheckController.Edit(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("DateCheck"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task EditPostReturnsViewNoAccess()
        {
            dateCheckService.Setup(s => s.GetCheckByIdForEditAsync(1))
                .ReturnsAsync(new DateCheckViewModelEdit());
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ReturnsAsync(new List<VehicleViewModelShortInfo>());

            var result = await dateCheckController.Edit(new DateCheckViewModelEdit());
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ViewName, Is.EquivalentTo("NoAccess"));
        }

        [Test]
        public async Task EditPostRedirects()
        {
            dateCheckService.Setup(s => s.EditAsync(new DateCheckViewModelEdit() { UserId = testUserId }));
            var result = await dateCheckController.Edit(new DateCheckViewModelEdit() { UserId = testUserId });

            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("DateCheck"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task CompleteReturnsViewNoAccess()
        {
            dateCheckService.Setup(s => s.CompleteCheckByIdAsync(1, testUserId))
                .ReturnsAsync("not me");

            var result = await dateCheckController.Complete(1);
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ViewName, Is.EquivalentTo("NoAccess"));
        }

        [Test]
        public async Task CompleteRedirects1()
        {
            dateCheckService.Setup(s => s.CompleteCheckByIdAsync(1, testUserId))
                .ReturnsAsync("not changed");

            var result = await dateCheckController.Complete(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("DateCheck"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task CompleteRedirects2()
        {
            dateCheckService.Setup(s => s.CompleteCheckByIdAsync(1, testUserId))
                .ReturnsAsync("changed");

            var result = await dateCheckController.Complete(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("DateCheck"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task CompleteRedirects3()
        {
            dateCheckService.Setup(s => s.CompleteCheckByIdAsync(1, testUserId))
                .ThrowsAsync(new Exception());

            var result = await dateCheckController.Complete(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("DateCheck"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task CompleteRedirects4()
        {
            dateCheckService.Setup(s => s.CompleteCheckByIdAsync(1, testUserId))
                .ReturnsAsync("invalid");

            var result = await dateCheckController.Complete(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("DateCheck"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task AllMineReturnsView()
        {
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ReturnsAsync(new List<VehicleViewModelShortInfo>());
            dateCheckService.Setup(s => s.GetAllMineAsync(testUserId))
                .ReturnsAsync(new List<DateCheckViewModelAll>());

            var result = await dateCheckController.AllMine();
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ViewName, Is.EquivalentTo("All"));
            Assert.That(viewResult.Model, Is.TypeOf<DateCheckViewModelAllDto>());
            Assert.IsTrue(viewResult.TempData.ContainsKey(InformationMessage));
        }

        [Test]
        public async Task AllMineRedirects()
        {
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ThrowsAsync(new Exception());
            dateCheckService.Setup(s => s.GetAllMineAsync(testUserId))
                .ThrowsAsync(new Exception());

            var result = await dateCheckController.AllMine();
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("DateCheck"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task AllForVehicleReturnsView()
        {
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ReturnsAsync(new List<VehicleViewModelShortInfo>());
            dateCheckService.Setup(s => s.GetAllForVehicleAsync("th1234ht"))
                .ReturnsAsync(new List<DateCheckViewModelAll>());

            var result = await dateCheckController.All("th1234ht");
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<DateCheckViewModelAllDto>());
            Assert.IsTrue(viewResult.TempData.ContainsKey(SuccessMessage));
        }

        [Test]
        public async Task AllForVEhicleRedirects()
        {
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ThrowsAsync(new Exception());
            dateCheckService.Setup(s => s.GetAllForVehicleAsync("th1234ht"))
                .ThrowsAsync(new Exception());

            var result = await dateCheckController.All("th1234ht");
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("DateCheck"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }
    }
}
