namespace Vmp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using Vmp.Services.Extensions;
    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.DateCheckViewModels;
    using Vmp.Web.ViewModels.VehicleViewModels;

    using static Vmp.Common.GlobalApplicationConstants;
    using static Vmp.Common.NotificationMessagesConstants;

    [Authorize]
    public class DateCheckController : Controller
    {
        private readonly IVehicleService vehicleService;
        private readonly IDateCheckService dateCheckService;

        public DateCheckController(IVehicleService vehicleService, IDateCheckService dateCheckService)
        {
            this.vehicleService = vehicleService;
            this.dateCheckService = dateCheckService;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            try
            {
                ICollection<VehicleViewModelShortInfo> vehicles =
                    await vehicleService.GetAllVehiclesAsync();
                DateCheckViewModelAdd viewModel = new DateCheckViewModelAdd()
                {
                    Vehicles = vehicles
                };

                return View(viewModel);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Add(DateCheckViewModelAdd viewModel)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    viewModel.Vehicles = await vehicleService.GetAllVehiclesAsync();
                    TempData[ErrorMessage] = InvalidDataErrorMassage;
                }
                catch (Exception)
                {
                    TempData[ErrorMessage] = DatabaseErrorMassage; ;
                }

                return View(viewModel);
            }

            string? myId = User.GetId();

            if (myId == null)
            {
                TempData[ErrorMessage] = "Use a registered account!";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                await dateCheckService.AddAsync(viewModel, myId);
                TempData[SuccessMessage] = "New Date check added!";
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
            }

            return RedirectToAction("All", "DateCheck");
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            DateCheckViewModelAllDto dtoModel = new DateCheckViewModelAllDto();
            try
            {
                dtoModel.Vehicles = await vehicleService.GetAllVehiclesAsync();
                dtoModel.Checks = await dateCheckService.GetAllDateChecksAsync();
                TempData[InformationMessage] = "All Date checks viewed";
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
            }

            return View(dtoModel);
        }

        [HttpGet]
        public async Task<IActionResult> ViewInfo(int id)
        {
            try
            {
                DateCheckViewModelDetails viewModel =
                    await dateCheckService.GetDateCheckByIdAsync(id);

                TempData[InformationMessage] = "Date Check Details viewed";
                return View(viewModel);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("All", "DateCheck");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            string myId = User.GetId()!;

            try
            {
                DateCheckViewModelEdit viewModel = await dateCheckService.GetCheckByIdForEditAsync(id);

                if (myId != viewModel.UserId)
                {
                    ViewData["Error"] = "Only the creator of the current Check can Edit it!";
                    return View("NoAccess");
                }
                viewModel.Vehicles = await vehicleService.GetAllVehiclesAsync();

                TempData[WarningMessage] = "Date check viewed for edit";
                return View(viewModel);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("All", "DateCheck");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DateCheckViewModelEdit viewModel)
        {
            string? myId = User.GetId()!;
            if (myId == null || myId != viewModel.UserId)
            {
                ViewData["Error"] = "Only the creator of the current Check can Edit it!";
                return View("NoAccess");
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    TempData[ErrorMessage] = InvalidDataErrorMassage;
                    viewModel.Vehicles = await vehicleService.GetAllVehiclesAsync();
                    return View(viewModel);
                }
                catch (Exception)
                {
                    TempData[ErrorMessage] = DatabaseErrorMassage;
                    return RedirectToAction("All", "DateCheck");
                }
            }

            try
            {
                viewModel.UserId = myId;
                await dateCheckService.EditAsync(viewModel);
                TempData[SuccessMessage] = "Date Check edited!";
                return RedirectToAction("All", "DateCheck");
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("All", "DateCheck");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Complete(int id)
        {
            string myId = User.GetId()!;
            try
            {
                string result = await dateCheckService.CompleteCheckByIdAsync(id, myId);

                if (result == "not me")
                {
                    ViewData["Error"] = "Only the creator of the current chech can complete it!";
                    return View("NoAccess");
                }
                else if (result == "not changed")
                {
                    TempData[ErrorMessage] = "Check is not completed";
                    return RedirectToAction("All", "DateCheck");
                }
                else if (result == "changed")
                {
                    TempData[SuccessMessage] = "Check Completed";
                    return RedirectToAction("All", "DateCheck");
                }
                else
                {
                    TempData[ErrorMessage] = "Check is not completed";
                    return RedirectToAction("All", "DateCheck");
                }
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("All", "DateCheck");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AllMine()
        {
            string myId = User.GetId()!;

            DateCheckViewModelAllDto dtoModel = new DateCheckViewModelAllDto();

            try
            {
                dtoModel.Vehicles = await vehicleService.GetAllVehiclesAsync();

                dtoModel.Checks = await dateCheckService.GetAllMineAsync(myId);

                TempData[InformationMessage] = "All My Date check viewed";
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("All", "DateCheck");
            }

            return View("All", dtoModel);
        }

        [HttpPost]
        public async Task<IActionResult> All(string vehicleNumber)
        {
            DateCheckViewModelAllDto dtoModel = new DateCheckViewModelAllDto();

            try
            {
                dtoModel.Vehicles = await vehicleService.GetAllVehiclesAsync();

                dtoModel.Checks = await dateCheckService.GetAllForVehicleAsync(vehicleNumber);

                TempData[SuccessMessage] = $"All Mileage check viewed for vehicle with Registration number: {vehicleNumber}";
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("All", "DateCheck");
            }

            return View(dtoModel);
        }
    }
}
