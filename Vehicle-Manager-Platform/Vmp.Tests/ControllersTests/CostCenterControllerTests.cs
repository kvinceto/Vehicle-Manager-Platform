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
    using Vmp.Web.ViewModels.CostCenterViewModels;

    using static Vmp.Common.NotificationMessagesConstants;

    [TestFixture]
    public class CostCenterControllerTests
    {
        private Mock<ClaimsPrincipal> userMock;
        protected ControllerContext testControllerContext;
        private string testUserId = "5c35b64b-b218-4548-ba50-5b75879a422f";
        private CostCenterController costCenterController;
        private Mock<ICostCenterService> costCenterServices;

        [SetUp]
        public void SetUp()
        {
            userMock = new Mock<ClaimsPrincipal>();

            userMock.Setup(mock => mock
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, testUserId));

            costCenterServices = new Mock<ICostCenterService>();

            testControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userMock.Object }
            };

            costCenterController = new CostCenterController(costCenterServices.Object)
            {
                ControllerContext = testControllerContext
            };

            costCenterController.TempData = new TempDataDictionary(
             new DefaultHttpContext(),
             Mock.Of<ITempDataProvider>());
        }

        [Test]
        public async Task AllReturnsView()
        {
            List<CostCenterViewModelAll> collection = new List<CostCenterViewModelAll>();
            costCenterServices.Setup(s => s.GetAllCostCentersAsync())
                .ReturnsAsync(collection);

            var result = await costCenterController.All();
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<List<CostCenterViewModelAll>>());
            Assert.IsTrue(viewResult.TempData.ContainsKey(InformationMessage));
            Assert.That(viewResult.TempData[InformationMessage], Is.EquivalentTo("All active cost centers are viewed"));
        }

        [Test]
        public async Task AllRedirects()
        {
            costCenterServices.Setup(s => s.GetAllCostCentersAsync())
                .ThrowsAsync(new Exception());

            var result = await costCenterController.All();
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Home"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("Index"));
        }

        [Test]
        public void AddReturnsView()
        {
            var result = costCenterController.Add();
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<CostCenterViewModelAdd>());
        }

        [Test]
        public async Task AddPostRedirects1()
        {
            costCenterServices.Setup(s => s.AddNewCostCenterAsync(new CostCenterViewModelAdd() { Name = "Test" }));

            var result = await costCenterController.Add(new CostCenterViewModelAdd { Name = "Test" });
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("CostCenter"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task AddPostRedirects2()
        {
            costCenterServices.Setup(s => s.AddNewCostCenterAsync(new CostCenterViewModelAdd() { Name = "Test" }))
                .ThrowsAsync(new Exception());

            var result = await costCenterController.Add(new CostCenterViewModelAdd { Name = "Test" });
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("CostCenter"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task DetailsReturnsView()
        {
            costCenterServices.Setup(s => s.GetCostCenterByIdAsync(1))
                .ReturnsAsync(new CostCenterViewModelDetails());

            var result = await costCenterController.Details(1);
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<CostCenterViewModelDetails>());
            Assert.IsTrue(viewResult.TempData.ContainsKey(InformationMessage));
            Assert.That(viewResult.TempData[InformationMessage], Is.EquivalentTo("Cost center info is viewed"));
        }

        [Test]
        public async Task DetailsRedirects()
        {
            costCenterServices.Setup(s => s.GetCostCenterByIdAsync(1))
                .ThrowsAsync(new Exception());

            var result = await costCenterController.Details(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("CostCenter"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task DeleteRedirects1()
        {
            costCenterServices.Setup(s => s.DeleteCostCenterByIdAsync(1))
                .ReturnsAsync(true);

            var result = await costCenterController.Delete(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("CostCenter"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task DeleteRedirects2()
        {
            costCenterServices.Setup(s => s.DeleteCostCenterByIdAsync(1))
                .ReturnsAsync(false);

            var result = await costCenterController.Delete(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("CostCenter"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task DeleteRedirects3()
        {
            costCenterServices.Setup(s => s.DeleteCostCenterByIdAsync(1))
                .ThrowsAsync(new Exception());

            var result = await costCenterController.Delete(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("CostCenter"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task EditReturnsView()
        {
            costCenterServices.Setup(s => s.GetCostCenterByIdAsync(1))
                .ReturnsAsync(new CostCenterViewModelDetails());

            var result = await costCenterController.Edit(1);
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<CostCenterViewModelEdit>());
            Assert.IsTrue(viewResult.TempData.ContainsKey(WarningMessage));
            Assert.That(viewResult.TempData[WarningMessage], Is.EquivalentTo("Cost center viewed for edit"));
        }

        [Test]
        public async Task EditRedirects()
        {
            costCenterServices.Setup(s => s.GetCostCenterByIdAsync(1))
                .ThrowsAsync(new Exception());

            var result = await costCenterController.Edit(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("CostCenter"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task EditPostRedirects1()
        {
            costCenterServices.Setup(s => s.EditCostCenterAsync(new CostCenterViewModelEdit()));

            var result = await costCenterController.Edit(new CostCenterViewModelEdit());
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("CostCenter"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task EditPostRedirects2()
        {
            costCenterServices.Setup(s => s.EditCostCenterAsync(new CostCenterViewModelEdit()))
                .ThrowsAsync(new Exception());

            var result = await costCenterController.Edit(new CostCenterViewModelEdit());
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("CostCenter"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }
    }
}
