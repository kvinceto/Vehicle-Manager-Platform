namespace Vmp.Web.Areas.Admin.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Authorization;

    using Vmp.Data.Models;
    using Vmp.Services.Extensions;
    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.AdminViewModels;

    using static Vmp.Common.GlobalApplicationConstants;
    using static Vmp.Common.NotificationMessagesConstants;

    [Authorize(Roles = AdminRoleName)]
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService adminService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;

        public AdminController(IAdminService adminService, UserManager<ApplicationUser> userManager, IUserService userService)
        {
            this.adminService = adminService;
            this.userManager = userManager;
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/Admin/AllTasks")]
        public async Task<IActionResult> AllTasks()
        {
            try
            {
                ICollection<TaskRestoreViewModel> taskModels = await adminService.GetAllTasksAsync();
                return View("AllTasks", taskModels);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error in the Database!";
                return RedirectToAction("Index", "Admin");
            }
        }


        [HttpPost]
        [Route("Admin/RestoreTask")]
        public async Task<IActionResult> RestoreTask(int taskId)
        {
            try
            {
                string result = await adminService.RestoreTaskByIdAsync(taskId);
                TempData[InformationMessage] = result;
            }
            catch (Exception)
            {
                TempData[InformationMessage] = "Error in the Database!";
            }

            return RedirectToAction("Index", "Admin");
        }


        [HttpGet]
        [Route("/Admin/AllOwners")]
        public async Task<IActionResult> AllOwners()
        {
            try
            {
                ICollection<OwnerRestoreViewModel> ownerModels = await adminService.GetAllOwnersAsync();
                return View("AllOwners", ownerModels);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error in the Database!";
                return RedirectToAction("Index", "Admin");
            }
        }

        [HttpPost]
        [Route("Admin/RestoreOwner")]
        public async Task<IActionResult> RestoreOwner(int ownerId)
        {
            try
            {
                string result = await adminService.RestoreOwnerByIdAsync(ownerId);
                TempData[InformationMessage] = result;
            }
            catch (Exception)
            {
                TempData[InformationMessage] = "Error in the Database!";
            }

            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        [Route("/Admin/AllVehicles")]
        public async Task<IActionResult> AllVehicles()
        {
            try
            {
                ICollection<VehicleRestoreViewModel> vehiclesModels = await adminService.GetAllVehiclesAsync();
                return View("AllVehicles", vehiclesModels);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error in the Database!";
                return RedirectToAction("Index", "Admin");
            }
        }

        [HttpPost]
        [Route("Admin/RestoreVehicle")]
        public async Task<IActionResult> RestoreVehicle(string regNumber)
        {
            try
            {
                string result = await adminService.RestoreVehicleByIdAsync(regNumber);
                TempData[InformationMessage] = result;
            }
            catch (Exception)
            {
                TempData[InformationMessage] = "Error in the Database!";
            }

            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        [Route("/Admin/AllCostCenters")]
        public async Task<IActionResult> AllCostCenters()
        {
            try
            {
                ICollection<CostCenterRestoreViewModel> costCntersModels = await adminService.GetAllCostCenteresAsync();
                return View("AllCostCenters", costCntersModels);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error in the Database!";
                return RedirectToAction("Index", "Admin");
            }
        }

        [HttpPost]
        [Route("Admin/RestoreCostCenter")]
        public async Task<IActionResult> RestoreCostCenter(int id)
        {
            try
            {
                string result = await adminService.RestoreCostCenterByIdAsync(id);
                TempData[InformationMessage] = result;
            }
            catch (Exception)
            {
                TempData[InformationMessage] = "Error in the Database!";
            }

            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        [Route("/Admin/AllMileageChecks")]
        public async Task<IActionResult> AllMileageChecks()
        {
            try
            {
                ICollection<MileageCheckRestoreViewModel> mileageChecksModels = await adminService.GetAllMileageChecksAsync();
                return View("AllMileageChecks", mileageChecksModels);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error in the Database!";
                return RedirectToAction("Index", "Admin");
            }
        }

        [HttpPost]
        [Route("Admin/RestoreMileageCheck")]
        public async Task<IActionResult> RestoreMileageCheck(int id)
        {
            try
            {
                string result = await adminService.RestoreMileageCheckByIdAsync(id);
                TempData[InformationMessage] = result;
            }
            catch (Exception)
            {
                TempData[InformationMessage] = "Error in the Database!";
            }

            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        [Route("/Admin/AllDateChecks")]
        public async Task<IActionResult> AllDateChecks()
        {
            try
            {
                ICollection<DateCheckRestoreViewModel> mileageChecksModels = await adminService.GetAllDateChecksAsync();
                return View("AllDateChecks", mileageChecksModels);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error in the Database!";
                return RedirectToAction("Index", "Admin");
            }
        }

        [HttpPost]
        [Route("Admin/RestoreDateCheck")]
        public async Task<IActionResult> RestoreDateCheck(int id)
        {
            try
            {
                string result = await adminService.RestoreDateCheckByIdAsync(id);
                TempData[InformationMessage] = result;
            }
            catch (Exception)
            {
                TempData[InformationMessage] = "Error in the Database!";
            }

            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        [Route("/Admin/AllUsers")]
        public async Task<IActionResult> AllUsers()
        {
            try
            {
                ICollection<UserViewModel> users = await adminService.GetAllUsers();

                return View("AllUsers", users);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error in the Database!";
                return RedirectToAction("Index", "Admin");
            }
        }

        [HttpPost]
        [Route("Admin/ChangeUserRole")]
        public async Task<IActionResult> ChangeUserRole(string userId, string newRole)
        {
            string myId = User.GetId()!.ToUpper();

            if (myId == userId)
            {
                TempData[ErrorMessage] = $"Can not change your own Role!";
                return RedirectToAction("Index", "Admin");
            }
            try
            {
                ApplicationUser? user = await userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    TempData[ErrorMessage] = $"User with Id: {userId} does not exist!";
                    return RedirectToAction("Index", "Admin");
                }

                var roles = await userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    await userManager.RemoveFromRoleAsync(user, role);
                }

                await userManager.AddToRoleAsync(user, newRole);

                TempData[InformationMessage] = $"User with Id: {userId} has been asigned to role: {newRole}";

                return RedirectToAction("Index", "Admin");
            }
            catch (Exception)
            {
                TempData[InformationMessage] = "Error in the Database!";
                return RedirectToAction("Index", "Admin");
            }

        }

        [HttpPost]
        [Route("Admin/DeleteUser")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            string myId = User.GetId()!.ToUpper();

            if (myId == userId)
            {
                TempData[ErrorMessage] = $"Can not delete Admin!";
                return RedirectToAction("Index", "Admin");
            }
            try
            {
                ApplicationUser? user = await userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    TempData[ErrorMessage] = $"User with Id: {userId} does not exist!";
                    return RedirectToAction("Index", "Admin");
                }

                var roles = await userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    if (role == AdminRoleName)
                    {
                        TempData[ErrorMessage] = $"Can not delete Admin!";
                        return RedirectToAction("Index", "Admin");
                    }

                    await userManager.RemoveFromRoleAsync(user, role);
                }

                await userService.DeleteDataAsync(userId);

                TempData[InformationMessage] = $"User with Id: {userId} has been deleted!";

                return RedirectToAction("Index", "Admin");
            }
            catch (Exception)
            {
                TempData[InformationMessage] = "Error in the Database!";
                return RedirectToAction("Index", "Admin");
            }

        }
    }
}
