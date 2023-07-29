namespace Vmp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.AdminViewModels;

    using static Vmp.Common.GlobalApplicationConstants;
    using static Vmp.Common.NotificationMessagesConstants;

    [Authorize(Roles = AdminRoleName)]
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService adminService;

        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //public async Task<IActionResult> RestoreObject()
        //{
        //    AdminRestoreViewModel viewModel = new AdminRestoreViewModel();
        //    return View(viewModel);
        //}


        [HttpPost]
        public async Task<IActionResult> RestoreObject(AdminRestoreViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "Invalid model!";
                return RedirectToAction("Index", "Admin");
            }

            try
            {
                string result = string.Empty;

                if (viewModel.TaskId != null)
                {
                    result = await adminService.RestoreTaskByIdAsync(viewModel.TaskId.Value);
                }
                else if (viewModel.OwnerId != null)
                {
                    result = await adminService.RestoreOwnerByIdAsync(viewModel.OwnerId.Value);
                }
                else if (viewModel.CostCenterId != null)
                {
                    result = await adminService.RestoreCostCenterByIdAsync(viewModel.CostCenterId.Value);
                }
                else if (viewModel.VehicleNumber != null)
                {
                    result = await adminService.RestoreVehicleByIdAsync(viewModel.VehicleNumber);
                }
                else if (viewModel.DateCheckId != null)
                {
                    result = await adminService.RestoreDateCheckByIdAsync(viewModel.DateCheckId.Value);

                }
                else if (viewModel.MileageCheckId != null)
                {
                    result = await adminService.RestoreMileageCheckByIdAsync(viewModel.MileageCheckId.Value);
                }

                TempData[InformationMessage] = result;

            }
            catch (Exception)
            {
                TempData[InformationMessage] = "Error in the Database!";
            }


            return RedirectToAction("Index", "Admin");
        }
    }
}
