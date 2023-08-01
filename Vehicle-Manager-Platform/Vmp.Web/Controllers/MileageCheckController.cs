namespace Vmp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.MileageCheckViewModels;

    using static Vmp.Common.GlobalApplicationConstants;
    using static Vmp.Common.NotificationMessagesConstants;
    using static Vmp.Services.Extensions.ClaimsPrincipalExtensions;

    [Authorize]
    public class MileageCheckController : Controller
    {
        private readonly IMileageCheckService mileageCheckService;
        private readonly IVehicleService vehicleService;

        public MileageCheckController(IMileageCheckService mileageCheckService, IVehicleService vehicleService)
        {
            this.mileageCheckService = mileageCheckService;
            this.vehicleService = vehicleService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            MileageCheckViewModelAllDto dtoModel = new MileageCheckViewModelAllDto();

            try
            {
                dtoModel.Vehicles = await vehicleService.GetAllVehiclesAsync();

                dtoModel.Checks = await mileageCheckService.GetAllActiveAsync();

                TempData[InformationMessage] = "All Mileage check viewed";
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
            }

            return View(dtoModel);
        }

        [HttpPost]
        public async Task<IActionResult> All(string vehicleNumber)
        {
            MileageCheckViewModelAllDto dtoModel = new MileageCheckViewModelAllDto();

            try
            {
                dtoModel.Vehicles = await vehicleService.GetAllVehiclesAsync();

                dtoModel.Checks = await mileageCheckService.GetAllForVehicleAsync(vehicleNumber);

                TempData[InformationMessage] = $"All Mileage check viewed for vehicle with Registration number: {vehicleNumber}";
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
            }

            return View(dtoModel);
        }

        [HttpPost]
        public async Task<IActionResult> AllMine()
        {
            string myId = User.GetId()!;

            MileageCheckViewModelAllDto dtoModel = new MileageCheckViewModelAllDto();

            try
            {
                dtoModel.Vehicles = await vehicleService.GetAllVehiclesAsync();

                dtoModel.Checks = await mileageCheckService.GetAllMineAsync(myId);

                TempData[InformationMessage] = "All My Mileage check viewed";
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
            }

            return View("All", dtoModel);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            MileageCheckViewModelAdd modelAdd = new MileageCheckViewModelAdd();
            try
            {
                modelAdd.Vehicles = await vehicleService.GetAllVehiclesAsync();
                return View(modelAdd);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(MileageCheckViewModelAdd modelAdd)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = InvalidDataErrorMassage;
                try
                {
                    modelAdd.Vehicles = await vehicleService.GetAllVehiclesAsync();
                    return View(modelAdd);
                }
                catch (Exception)
                {
                    TempData[ErrorMessage] = DatabaseErrorMassage;
                    return RedirectToAction("Index", "Home");
                }              
            }

            string myId = User.GetId()!;

            try
            {
                await mileageCheckService.AddAsync(modelAdd, myId);
                TempData[SuccessMessage] = "Check added!";
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
            }

            return RedirectToAction("All", "MileageCheck");
        }

        [HttpGet]
        public async Task<IActionResult> ViewInfo(int id)
        {
            try
            {
                MileageCheckViewModelDetails viewModel = await mileageCheckService.GetChechByIdAsync(id);
                TempData[InformationMessage] = "Info viewed";
                return View(viewModel);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("All", "MileageCheck");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            string myId = User.GetId()!;

            try
            {
                MileageCheckViewModelEdit model = await mileageCheckService.GetCheckByIdForEditAsync(id);

                if (myId != model.UserId)
                {
                    ViewData["Error"] = "Only the creator of the current Check can Edit it!";
                    return View("NoAccess");
                }

                TempData[WarningMessage] = "Mileage check viewed for edit";
                return View(model);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error in the database!";
                return RedirectToAction("All", "MileageCheck");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MileageCheckViewModelEdit viewModel)
        {
            viewModel.UserId = User.GetId()!;
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = InvalidDataErrorMassage;
                try
                {
                    viewModel.Vehicles = await vehicleService.GetAllVehiclesAsync();
                    return View(viewModel);
                }
                catch (Exception)
                {
                    TempData[ErrorMessage] = DatabaseErrorMassage;
                    return RedirectToAction("All", "MileageCheck");
                }               
            }

            try
            {
                await mileageCheckService.EditAsync(viewModel);
                TempData[SuccessMessage] = "Mileage Check edited!";
                return RedirectToAction("All", "MileageCheck");
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error in the database!";
                return RedirectToAction("All", "MileageCheck");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Complete(int id)
        {
            string myId = User.GetId()!;
            try
            {
                string result = await mileageCheckService.CompleteCheckByIdAsync(id, myId);

                if (result == "not me")
                {
                    ViewData["Error"] = "Only the creator of the current check can complete it!";
                    return View("NoAccess");
                }
                else if (result == "not changed")
                {
                    TempData[ErrorMessage] = "Check is not completed";
                    return RedirectToAction("All", "MileageCheck");
                }
                else if (result == "changed")
                {
                    TempData[SuccessMessage] = "Check Completed";
                    return RedirectToAction("All", "MileageCheck");
                }
                else
                {
                    TempData[ErrorMessage] = "Check is not completed";
                    return RedirectToAction("All", "MileageCheck");
                }
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("All", "MileageCheck");
            }
        }
    }
}
