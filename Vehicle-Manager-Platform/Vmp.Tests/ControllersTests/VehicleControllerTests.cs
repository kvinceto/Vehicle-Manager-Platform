namespace Vmp.Tests.ControllersTests
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Mvc;

    using Moq;

    using Vmp.Services.Interfaces;
    using Vmp.Web.Controllers;
    using Vmp.Web.ViewModels.VehicleViewModels;
    using Vmp.Web.ViewModels.OwnerViewModels;

    using static Vmp.Common.NotificationMessagesConstants;

    [TestFixture]
    public class VehicleControllerTests
    {
        private Mock<ClaimsPrincipal> userMock;
        protected ControllerContext testControllerContext;
        private string testUserId = "5c35b64b-b218-4548-ba50-5b75879a422f";
        private VehicleController vehicleController;
        private Mock<IVehicleService> vehicleService;
        private Mock<IOwnerService> ownerService;

        [SetUp]
        public void SetUp()
        {
            userMock = new Mock<ClaimsPrincipal>();

            userMock.Setup(mock => mock
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, testUserId));

            vehicleService = new Mock<IVehicleService>();
            ownerService = new Mock<IOwnerService>();

            testControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userMock.Object }
            };

            vehicleController = new VehicleController(vehicleService.Object, ownerService.Object)
            {
                ControllerContext = testControllerContext
            };

            vehicleController.TempData = new TempDataDictionary(
             new DefaultHttpContext(),
             Mock.Of<ITempDataProvider>());
        }

        [Test]
        public async Task AllReturnsView()
        {
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ReturnsAsync(new List<VehicleViewModelShortInfo>());

            var result = await vehicleController.All();
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<List<VehicleViewModelShortInfo>>());
            Assert.IsTrue(viewResult.TempData.ContainsKey(InformationMessage));
            Assert.That(viewResult.TempData[InformationMessage], Is.EquivalentTo("All active vehicles are viewed"));
        }

        [Test]
        public async Task AllRedirects()
        {
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ThrowsAsync(new Exception());

            var result = await vehicleController.All();
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Home"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("Index"));
        }

        [Test]
        public async Task AddReturnsView()
        {
            ownerService.Setup(s => s.GetAllActiveOwnersAsync())
                .ReturnsAsync(new List<OwnerViewModelAll>());

            var result = await vehicleController.Add();
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<VehicleViewModelAdd>());
        }

        [Test]
        public async Task AddRedirects()
        {
            ownerService.Setup(s => s.GetAllActiveOwnersAsync())
                .ThrowsAsync(new Exception());

            var result = await vehicleController.Add();
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Home"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("Index"));
        }

        [Test]
        public async Task AddPostRedirects()
        {
            vehicleService.Setup(s => s.AddNewVehicleAsync(new VehicleViewModelAdd()));

            var result = await vehicleController.Add(new VehicleViewModelAdd());
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Vehicle"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task ViewVehicleInfoReturnsView()
        {
            vehicleService.Setup(s => s.GetVehicleByIdAsync("th1234ht"))
                .ReturnsAsync(new VehicleViewModelDetails());

            var result = await vehicleController.ViewVehicleInfo("th1234ht");
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<VehicleViewModelDetails>());
            Assert.IsTrue(viewResult.TempData.ContainsKey(SuccessMessage));
            Assert.That(viewResult.TempData[SuccessMessage], Is.EquivalentTo("Vehicle info is viewed"));
        }

        [Test]
        public async Task ViewVehicleInfoRedirect()
        {
            vehicleService.Setup(s => s.GetVehicleByIdAsync("th1234ht"))
               .ThrowsAsync(new Exception());

            var result = await vehicleController.ViewVehicleInfo("th1234ht");
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Vehicle"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task EditReturnsView()
        {
            vehicleService.Setup(s => s.GetVehicleByIdForEditAsync("th1234ht"))
                .ReturnsAsync(new VehicleViewModelAdd());
            ownerService.Setup(s => s.GetAllActiveOwnersAsync())
                .ReturnsAsync(new List<OwnerViewModelAll>());

            var result = await vehicleController.Edit("th1234ht");
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<VehicleViewModelAdd>());
            Assert.IsTrue(viewResult.TempData.ContainsKey(WarningMessage));
            Assert.That(viewResult.TempData[WarningMessage], Is.EquivalentTo("Vehicle is viewed for Edit!"));
        }

        [Test]
        public async Task EditRedirects()
        {
            vehicleService.Setup(s => s.GetVehicleByIdForEditAsync("th1234ht"))
                .ThrowsAsync(new Exception());

            var result = await vehicleController.Edit("th1234ht");
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Vehicle"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task EditVehicleRedirects1()
        {
            vehicleService.Setup(s => s.EditVehicleAsync(new VehicleViewModelAdd()));

            var result = await vehicleController.EditVehicle(new VehicleViewModelAdd());
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Vehicle"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task EditVehicleRedirects2()
        {
            vehicleService.Setup(s => s.EditVehicleAsync(new VehicleViewModelAdd()))
                .ThrowsAsync(new Exception());

            var result = await vehicleController.EditVehicle(new VehicleViewModelAdd());
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Vehicle"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task DeleteRedirects1()
        {
            vehicleService.Setup(s => s.DeleteVehicleByIdAsync("th1234ht"))
                .ReturnsAsync(true);

            var result = await vehicleController.Delete("th1234ht");
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Vehicle"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task DeleteRedirects2()
        {
            vehicleService.Setup(s => s.DeleteVehicleByIdAsync("th1234ht"))
                .ReturnsAsync(false);

            var result = await vehicleController.Delete("th1234ht");
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Vehicle"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task DeleteRedirects3()
        {
            vehicleService.Setup(s => s.DeleteVehicleByIdAsync("th1234ht"))
                .ThrowsAsync(new Exception());

            var result = await vehicleController.Delete("th1234ht");
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Vehicle"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }
    }
}
