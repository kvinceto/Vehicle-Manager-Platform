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
    using Vmp.Web.ViewModels.WaybillViewModels;
    using Vmp.Web.ViewModels.CostCenterViewModels;

    using static Vmp.Common.NotificationMessagesConstants;

    [TestFixture]
    public class WaybillControllerTests
    {
        private Mock<ClaimsPrincipal> userMock;
        protected ControllerContext testControllerContext;
        private string testUserId = "5c35b64b-b218-4548-ba50-5b75879a422f";
        private WaybillController waybillController;
        private Mock<IWaybillService> waybillService;
        private Mock<IVehicleService> vehicleService;
        private Mock<ICostCenterService> costCenterService;

        [SetUp]
        public void SetUp()
        {
            userMock = new Mock<ClaimsPrincipal>();

            userMock.Setup(mock => mock
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, testUserId));

            waybillService = new Mock<IWaybillService>();
            vehicleService = new Mock<IVehicleService>();
            costCenterService = new Mock<ICostCenterService>();

            testControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userMock.Object }
            };

            waybillController = new WaybillController(waybillService.Object, vehicleService.Object, costCenterService.Object)
            {
                ControllerContext = testControllerContext
            };

            waybillController.TempData = new TempDataDictionary(
             new DefaultHttpContext(),
             Mock.Of<ITempDataProvider>());
        }

        [Test]
        public async Task IndexReturnsView()
        {
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ReturnsAsync(new List<VehicleViewModelShortInfo>());

            var result = await waybillController.Index();
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<List<VehicleViewModelShortInfo>>());
        }

        [Test]
        public async Task IndexRedirects()
        {
            vehicleService.Setup(s => s.GetAllVehiclesAsync())
                .ThrowsAsync(new Exception());

            var result = await waybillController.Index();
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Home"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("Index"));
        }

        [Test]
        public async Task AddPostReturnsView()
        {
            waybillService.Setup(s => s.GetWaybillForAddingAsync("th1234ht"))
                .ReturnsAsync(new WaybillViewModelAdd());
            costCenterService.Setup(s => s.GetAllCostCentersAsync())
                .ReturnsAsync(new List<CostCenterViewModelAll>());

            var result = await waybillController.Add("th1234ht");
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<WaybillViewModelAdd>());
        }

        [Test]
        public async Task AddPostRedirects()
        {
            waybillService.Setup(s => s.GetWaybillForAddingAsync("th1234ht"))
                .ThrowsAsync(new Exception());
            costCenterService.Setup(s => s.GetAllCostCentersAsync())
                .ThrowsAsync(new Exception());

            var result = await waybillController.Add("th1234ht");
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Home"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("Index"));
        }

        [Test]
        public async Task AddWaybillRedirectsInvalidData()
        {
            WaybillViewModelAdd model = new WaybillViewModelAdd()
            {
                MileageStart = 100,
                MileageEnd = 90
            };

            var result = await waybillController.AddWaybill(model);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Waybill"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("Index"));
        }

        [Test]
        public async Task AddWaybillRedirects()
        {
            WaybillViewModelAdd model = new WaybillViewModelAdd();
            waybillService.Setup(s => s.AddWaybillAsync(model, testUserId));

            var result = await waybillController.AddWaybill(model);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Waybill"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("Index"));
        }

        [Test]
        public async Task AllPeriodReturnsView()
        {
            vehicleService.Setup(s => s.GetAllAsync())
                .ReturnsAsync(new List<VehicleViewModelShortInfo>());

            var result = await waybillController.AllPeriod();
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<WaybillDatesViewModel>());
        }

        [Test]
        public async Task AllPeriodRedirects()
        {
            vehicleService.Setup(s => s.GetAllAsync())
                .ThrowsAsync(new Exception());

            var result = await waybillController.AllPeriod();
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Home"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("Index"));
        }

        [Test]
        public async Task AllPostReturnsView()
        {
            WaybillDatesViewModel model = new WaybillDatesViewModel();

            waybillService.Setup(s => s.GetAllForVehicleForPeriod("th1234ht", "12/12/2023", "24/12/2023"))
                .ReturnsAsync(new List<WaybillViewModelAll>());

            var result = await waybillController.All(model);
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
        }

        [Test]
        public async Task AllPostRedirects()
        {
            WaybillDatesViewModel model = new WaybillDatesViewModel()
            {
                VehicleNumber = "th1234ht",
                StartDate = "12/12/2023",
                EndDate = "24/12/2023"
            };

            waybillService.Setup(s => s.GetAllForVehicleForPeriod("th1234ht", "12/12/2023", "24/12/2023"))
                .ThrowsAsync(new Exception());

            var result = await waybillController.All(model);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Home"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("Index"));
        }

        [Test]
        public async Task AllForCostCenterReturnsView()
        {
            waybillService.Setup(s => s.GetAllForCostCenterAsync(1))
                .ReturnsAsync(new List<WaybillViewModelAll>());

            var result = await waybillController.AllForCostCenter(1);
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.IsTrue(viewResult.TempData.ContainsKey(InformationMessage));
            Assert.IsTrue(viewResult.ViewData.ContainsKey("Text"));
            Assert.That(viewResult.Model, Is.TypeOf<List<WaybillViewModelAll>>());
        }

        [Test]
        public async Task AllForCostCenterRedirects()
        {
            waybillService.Setup(s => s.GetAllForCostCenterAsync(1))
                .ThrowsAsync(new Exception());

            var result = await waybillController.AllForCostCenter(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("CostCenter"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task AllForVehicleReturnsView()
        {
            waybillService.Setup(s => s.GetAllForVehicleAsync("th1234th"))
                .ReturnsAsync(new List<WaybillViewModelAll>());

            var result = await waybillController.AllForVehicle("th1234th");
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.IsTrue(viewResult.TempData.ContainsKey(SuccessMessage));
            Assert.That(viewResult.Model, Is.TypeOf<List<WaybillViewModelAll>>());
        }

        [Test]
        public async Task AllForVehicleRedirects()
        {
            waybillService.Setup(s => s.GetAllForVehicleAsync("th1234th"))
                .ThrowsAsync(new Exception());

            var result = await waybillController.AllForVehicle("th1234th");
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Home"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("Index"));
        }

        [Test]
        public async Task DetailsReturnsView()
        {
            waybillService.Setup(s => s.GetWaybillByIdAsync(1))
                .ReturnsAsync(new WaybillViewModelDetails());

            var result = await waybillController.Details(1);
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.IsTrue(viewResult.TempData.ContainsKey(InformationMessage));
            Assert.That(viewResult.Model, Is.TypeOf<WaybillViewModelDetails>());
        }

        [Test]
        public async Task DetailsRedirects()
        {
            waybillService.Setup(s => s.GetWaybillByIdAsync(1))
                .ThrowsAsync(new Exception());

            var result = await waybillController.Details(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Waybill"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task EditReturnsViewNoAccess()
        {
            waybillService.Setup(s => s.GetShortWaybillByIdAsync(1))
                .ReturnsAsync(new WaybillViewModelShort());

            var result = await waybillController.Edit(1);
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ViewName, Is.EquivalentTo("NoAccess"));
        }

        [Test]
        public async Task EditReturnsViewEdit()
        {
            waybillService.Setup(s => s.GetShortWaybillByIdAsync(1))
                .ReturnsAsync(new WaybillViewModelShort() { UserId = testUserId });

            waybillService.Setup(s => s.GetWaybillForEditByIdAsync(1))
                .ReturnsAsync(new WaybillViewModelEdit());

            var result = await waybillController.Edit(1);
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.IsTrue(viewResult.TempData.ContainsKey(WarningMessage));
        }

        [Test]
        public async Task EditRedirects()
        {
            waybillService.Setup(s => s.GetShortWaybillByIdAsync(1))
                .ThrowsAsync(new Exception());

            waybillService.Setup(s => s.GetWaybillForEditByIdAsync(1))
                .ThrowsAsync(new Exception());

            var result = await waybillController.Edit(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Waybills"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task EditWaybillRedirects()
        {
            waybillService.Setup(s => s.EditWaybillAsync(new WaybillViewModelEdit(), testUserId));


            var result = await waybillController.EditWaybill(new WaybillViewModelEdit());

            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Waybill"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("AllPeriod"));
        }    
    }
}
