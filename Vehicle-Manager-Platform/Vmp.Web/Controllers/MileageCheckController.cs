namespace Vmp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.MileageCheckViewModels;

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

                dtoModel.Checks = await mileageCheckService.GetAllAsync();

                TempData[SuccessMessage] = "All Mileage check viewed";
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error with the database!";
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

                TempData[SuccessMessage] = $"All Mileage check viewed for vehicle with Registration number: {vehicleNumber}";
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error with the database!";
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

                TempData[SuccessMessage] = "All My Mileage check viewed";
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error with the database!";
            }

            return View("All", dtoModel);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            MileageCheckViewModelAdd modelAdd = new MileageCheckViewModelAdd();
            modelAdd.Vehicles = await vehicleService.GetAllVehiclesAsync();
            return View(modelAdd);
        }

        [HttpPost]
        public async Task<IActionResult> Add(MileageCheckViewModelAdd modelAdd)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "Enter valid data!";
                return View(modelAdd);
            }

            string myId = User.GetId()!;

            try
            {
                await mileageCheckService.AddAsync(modelAdd, myId);
                TempData[SuccessMessage] = "Check added!";
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error with the database!";
            }

            return RedirectToAction("All", "MileageCheck");
        }


        [HttpGet]
        public async Task<IActionResult> ViewInfo(int id)
        {
            try
            {
                MileageCheckViewModelDetails viewModel = await mileageCheckService.GetChechByIdAsync(id);
                TempData[SuccessMessage] = "Info viewed";
                return View(viewModel);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error with the database!";
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
                TempData[ErrorMessage] = "Enter valid data";
                viewModel.Vehicles = await vehicleService.GetAllVehiclesAsync();
                return View(viewModel);
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
                    ViewData["Error"] = "Only the creator of the current chech can complete it!";
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
                TempData[ErrorMessage] = "Error in the Database!";
                return RedirectToAction("All", "MileageCheck");
            }
        }
    }
}
