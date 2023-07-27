namespace Vmp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Authorization;

    using Vmp.Data.Models;
    using Vmp.Services.Extensions;
    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels;

    using static Vmp.Common.NotificationMessagesConstants;

    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private SignInManager<ApplicationUser> signInManager;


        public UserController(IUserService userService, SignInManager<ApplicationUser> signInManager)
        {
            this.userService = userService;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Delete()
        {
            UserViewModel model = new UserViewModel();
            model.Id = User.GetId();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserData(string? id)
        {
            string? myId = User.GetId();

            if(myId == null || myId != id)
            {
                TempData[ErrorMessage] = "No Access!";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                await userService.DeleteDataAsync(id);
                await signInManager.SignOutAsync();
                TempData[SuccessMessage] = "Account Deleted";
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Eror in the Database!";
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
