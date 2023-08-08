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
    using Vmp.Web.ViewModels.MileageCheckViewModels;

    using static Vmp.Common.NotificationMessagesConstants;

    [TestFixture]
    public class MileageCheckControllerTests
    {
        private Mock<ClaimsPrincipal> userMock;
        protected ControllerContext testControllerContext;
        private string testUserId = "5c35b64b-b218-4548-ba50-5b75879a422f";
        private MileageCheckController mileageCheckController;
        private Mock<IMileageCheckService> mileageCheckServices;
        private Mock<IVehicleService> vehicleService;

        [SetUp]
        public void SetUp()
        {
            userMock = new Mock<ClaimsPrincipal>();

            userMock.Setup(mock => mock
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, testUserId));

            mileageCheckServices = new Mock<IMileageCheckService>();
            vehicleService = new Mock<IVehicleService>();

            testControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userMock.Object }
            };

            mileageCheckController = new MileageCheckController(mileageCheckServices.Object, vehicleService.Object)
            {
                ControllerContext = testControllerContext
            };

            mileageCheckController.TempData = new TempDataDictionary(
             new DefaultHttpContext(),
             Mock.Of<ITempDataProvider>());
        }

        [Test]
        public async Task AllReturnsView1()
        {
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ReturnsAsync(new List<VehicleViewModelShortInfo>());
            mileageCheckServices.Setup(s => s.GetAllActiveAsync())
                .ReturnsAsync(new List<MileageCheckViewModelAll>());

            var result = await mileageCheckController.All();
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<MileageCheckViewModelAllDto>());
            Assert.IsTrue(viewResult.TempData.ContainsKey(InformationMessage));
        }

        [Test]
        public async Task AllReturnsView2()
        {
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ThrowsAsync(new Exception());
            mileageCheckServices.Setup(s => s.GetAllActiveAsync())
                .ThrowsAsync(new Exception());


            var result = await mileageCheckController.All();
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<MileageCheckViewModelAllDto>());
            Assert.IsTrue(viewResult.TempData.ContainsKey(ErrorMessage));
        }

        [Test]
        public async Task AllPostReturnsView1()
        {
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ReturnsAsync(new List<VehicleViewModelShortInfo>());
            mileageCheckServices.Setup(s => s.GetAllForVehicleAsync("th1234ht"))
                .ReturnsAsync(new List<MileageCheckViewModelAll>());

            var result = await mileageCheckController.All("th1234ht");
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<MileageCheckViewModelAllDto>());
            Assert.IsTrue(viewResult.TempData.ContainsKey(InformationMessage));
        }

        [Test]
        public async Task AllPostReturnsView2()
        {
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ThrowsAsync(new Exception());
            mileageCheckServices.Setup(s => s.GetAllForVehicleAsync("th1234ht"))
                .ThrowsAsync(new Exception());

            var result = await mileageCheckController.All("th1234ht");
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<MileageCheckViewModelAllDto>());
            Assert.IsTrue(viewResult.TempData.ContainsKey(ErrorMessage));
        }

        [Test]
        public async Task AllMinePostReturnsView1()
        {
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ReturnsAsync(new List<VehicleViewModelShortInfo>());
            mileageCheckServices.Setup(s => s.GetAllMineAsync(testUserId))
                .ReturnsAsync(new List<MileageCheckViewModelAll>());

            var result = await mileageCheckController.AllMine();
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<MileageCheckViewModelAllDto>());
            Assert.IsTrue(viewResult.TempData.ContainsKey(InformationMessage));
        }

        [Test]
        public async Task AllMinePostReturnsView2()
        {
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ThrowsAsync(new Exception());
            mileageCheckServices.Setup(s => s.GetAllMineAsync(testUserId))
                .ThrowsAsync(new Exception());

            var result = await mileageCheckController.AllMine();
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<MileageCheckViewModelAllDto>());
            Assert.IsTrue(viewResult.TempData.ContainsKey(ErrorMessage));
        }

        [Test]
        public async Task AddReturnsView()
        {
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ReturnsAsync(new List<VehicleViewModelShortInfo>());

            var result = await mileageCheckController.Add();
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<MileageCheckViewModelAdd>());
        }

        [Test]
        public async Task AddRedirects()
        {
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ThrowsAsync(new Exception());

            var result = await mileageCheckController.Add();
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Home"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("Index"));
        }

        [Test]
        public async Task AddPostRedirects()
        {
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ReturnsAsync(new List<VehicleViewModelShortInfo>());
            mileageCheckServices.Setup(s => s.AddAsync(new MileageCheckViewModelAdd(), testUserId));

            var result = await mileageCheckController.Add(new MileageCheckViewModelAdd());
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("MileageCheck"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task ViewInfoReturnsView()
        {
            mileageCheckServices.Setup(s => s.GetChechByIdAsync(1))
                .ReturnsAsync(new MileageCheckViewModelDetails());

            var result = await mileageCheckController.ViewInfo(1);
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<MileageCheckViewModelDetails>());
            Assert.IsTrue(viewResult.TempData.ContainsKey(InformationMessage));
        }

        [Test]
        public async Task ViewInfoRedirects()
        {
            mileageCheckServices.Setup(s => s.GetChechByIdAsync(1))
                .ThrowsAsync(new Exception());

            var result = await mileageCheckController.ViewInfo(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("MileageCheck"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task EditReturnsViewNoAccess()
        {
            mileageCheckServices.Setup(s => s.GetCheckByIdForEditAsync(1))
                .ReturnsAsync(new MileageCheckViewModelEdit());
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ReturnsAsync(new List<VehicleViewModelShortInfo>());

            var result = await mileageCheckController.Edit(1);
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ViewName, Is.EquivalentTo("NoAccess"));
        }

        [Test]
        public async Task EditReturnsViewEdit()
        {
            mileageCheckServices.Setup(s => s.GetCheckByIdForEditAsync(1))
                .ReturnsAsync(new MileageCheckViewModelEdit() { UserId = testUserId });
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ReturnsAsync(new List<VehicleViewModelShortInfo>());

            var result = await mileageCheckController.Edit(1);
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.IsTrue(viewResult.TempData.ContainsKey(WarningMessage));
        }

        [Test]
        public async Task EditRedirects()
        {
            mileageCheckServices.Setup(s => s.GetCheckByIdForEditAsync(1))
                .ThrowsAsync(new Exception());
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ThrowsAsync(new Exception());

            var result = await mileageCheckController.Edit(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("MileageCheck"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task EditPostRedirects()
        {
            mileageCheckServices.Setup(s => s.EditAsync(new MileageCheckViewModelEdit()));

            var result = await mileageCheckController.Edit(new MileageCheckViewModelEdit());
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("MileageCheck"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task CompleteRedirects1()
        {
            mileageCheckServices.Setup(s => s.CompleteCheckByIdAsync(1, testUserId))
                .ReturnsAsync("not changed");

            var result = await mileageCheckController.Complete(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("MileageCheck"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task CompleteRedirects2()
        {
            mileageCheckServices.Setup(s => s.CompleteCheckByIdAsync(1, testUserId))
                .ReturnsAsync("changed");

            var result = await mileageCheckController.Complete(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("MileageCheck"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task CompleteRedirects3()
        {
            mileageCheckServices.Setup(s => s.CompleteCheckByIdAsync(1, testUserId))
                .ThrowsAsync(new Exception());

            var result = await mileageCheckController.Complete(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("MileageCheck"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task CompleteRedirects4()
        {
            mileageCheckServices.Setup(s => s.CompleteCheckByIdAsync(1, testUserId))
                .ReturnsAsync("invalid");

            var result = await mileageCheckController.Complete(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("MileageCheck"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task CompleteReturnsViewNoAccess()
        {
            mileageCheckServices.Setup(s => s.CompleteCheckByIdAsync(1, testUserId))
                .ReturnsAsync("not me");

            var result = await mileageCheckController.Complete(1);
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ViewName, Is.EquivalentTo("NoAccess"));
        }
    }
}
