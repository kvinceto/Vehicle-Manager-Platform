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
    using Vmp.Web.ViewModels.OwnerViewModels;

    using static Vmp.Common.NotificationMessagesConstants;

    [TestFixture]
    public class OwnerControllerTests
    {
        private Mock<ClaimsPrincipal> userMock;
        protected ControllerContext testControllerContext;
        private string testUserId = "5c35b64b-b218-4548-ba50-5b75879a422f";
        private OwnerController ownerController;
        private Mock<IOwnerService> ownerServices;

        [SetUp]
        public void SetUp()
        {
            userMock = new Mock<ClaimsPrincipal>();

            userMock.Setup(mock => mock
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, testUserId));

            ownerServices = new Mock<IOwnerService>();

            testControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userMock.Object }
            };

            ownerController = new OwnerController(ownerServices.Object)
            {
                ControllerContext = testControllerContext
            };

            ownerController.TempData = new TempDataDictionary(
             new DefaultHttpContext(),
             Mock.Of<ITempDataProvider>());
        }


        [Test]
        public async Task AllReturnsView()
        {
            ICollection<OwnerViewModelAll> collection = new List<OwnerViewModelAll>();
            ownerServices.Setup(s => s.GetAllActiveOwnersAsync())
                .ReturnsAsync(collection);

            var result = await ownerController.All();
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<List<OwnerViewModelAll>>());
            Assert.IsTrue(viewResult.TempData.ContainsKey(InformationMessage));
            Assert.That(viewResult.TempData[InformationMessage], Is.EquivalentTo("All active Owners are viewed"));
        }

        [Test]
        public async Task AllRedirects()
        {
            ownerServices.Setup(s => s.GetAllActiveOwnersAsync())
                .ThrowsAsync(new Exception());

            var result = await ownerController.All();
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Home"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("Index"));
        }

        [Test]
        public void AddReturnsView()
        {
            var result = ownerController.Add();
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<OwnerViewModelAdd>());
        }

        [Test]
        public async Task AddPostRedirects1()
        {
            ownerServices.Setup(s => s.AddNewOwnerAsync(new OwnerViewModelAdd()));
            var result = await ownerController.Add(new OwnerViewModelAdd());
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Owner"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task AddPostRedirects2()
        {
            ownerServices.Setup(s => s.AddNewOwnerAsync(new OwnerViewModelAdd()))
                .ThrowsAsync(new NullReferenceException());

            var result = await ownerController.Add(new OwnerViewModelAdd());
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Owner"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task ViewOwnerInfoReturnsView()
        {
            ownerServices
                .Setup(s => s.GetOwnerByIdAsync(1))
                .ReturnsAsync(new OwnerViewModelInfo());

            var result = await ownerController.ViewOwnerInfo(1);
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.TempData.ContainsKey(SuccessMessage));
            Assert.That(viewResult.TempData[SuccessMessage], Is.EquivalentTo("Owner info is viewed"));
        }

        [Test]
        public async Task ViewOwnerInfoRedirects()
        {
            ownerServices
                .Setup(s => s.GetOwnerByIdAsync(1))
                .ThrowsAsync(new Exception());

            var result = await ownerController.ViewOwnerInfo(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Owner"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));           
        }

        [Test]
        public async Task DeleteRedirects1()
        {
            ownerServices
                .Setup(s => s.DeleteOwnerByIdAsync(1))
                .ReturnsAsync(true);

            var result = await ownerController.Delete(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Owner"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task DeleteRedirects2()
        {
            ownerServices
                .Setup(s => s.DeleteOwnerByIdAsync(1))
                .ReturnsAsync(false);

            var result = await ownerController.Delete(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Owner"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task DeleteRedirects3()
        {
            ownerServices
                .Setup(s => s.DeleteOwnerByIdAsync(1))
                .ThrowsAsync(new Exception());

            var result = await ownerController.Delete(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Owner"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task EditReturnsView()
        {
            ownerServices.Setup(s => s.GetOwnerByIdForEditAsync(1))
                .ReturnsAsync(new OwnerViewModelEdit());

            var result = await ownerController.Edit(1);
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.Model, Is.TypeOf<OwnerViewModelEdit>());
        }

        [Test]
        public async Task EditRedirects()
        {
            ownerServices.Setup(s => s.GetOwnerByIdForEditAsync(1))
                .ThrowsAsync(new Exception());

            var result = await ownerController.Edit(1);
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Owner"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task EditPostRedirects1()
        {
            ownerServices.Setup(s => s.EditOwnerAsync(new OwnerViewModelEdit()))
                .ThrowsAsync(new Exception());

            var result = await ownerController.Edit(new OwnerViewModelEdit());
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Owner"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }

        [Test]
        public async Task EditPostRedirects2()
        {
            ownerServices.Setup(s => s.EditOwnerAsync(new OwnerViewModelEdit()));

            var result = await ownerController.Edit(new OwnerViewModelEdit());
            var viewResult = result as RedirectToActionResult;

            Assert.IsNotNull(viewResult);
            Assert.That(viewResult.ControllerName, Is.EquivalentTo("Owner"));
            Assert.That(viewResult.ActionName, Is.EquivalentTo("All"));
        }
    }
}
