namespace Vmp.Tests.ControllersTests
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Moq;

    using Vmp.Web.Controllers;

    [TestFixture]
    public class HomeControllerTests
    {
        private Mock<ClaimsPrincipal> userMock;
        protected ControllerContext testControllerContext;
        private string testUserId = "5c35b64b-b218-4548-ba50-5b75879a422f";
        private HomeController homeController;

        [SetUp]
        public void SetUp()
        {
            userMock = new Mock<ClaimsPrincipal>();

            userMock.Setup(mock => mock
                .FindFirst(ClaimTypes.NameIdentifier))
                .Returns(new Claim(ClaimTypes.NameIdentifier, testUserId));

            testControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userMock.Object }
            };

            homeController = new HomeController()
            {
                ControllerContext = testControllerContext
            };
        }

        [Test]
        public void IndexShouldReturnViewIndex()
        {
            var result = homeController.Index();
            var viewResult = result as ViewResult;         

            Assert.NotNull(viewResult);
        }
    }
}
