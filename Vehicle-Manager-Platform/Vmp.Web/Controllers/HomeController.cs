namespace Vmp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    public class HomeController : Controller
    {
        public HomeController() { }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
    }
}