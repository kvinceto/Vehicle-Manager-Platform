namespace Vmp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.VehicleViewModels;

    using static Vmp.Common.GlobalApplicationConstants;
    using static Vmp.Common.NotificationMessagesConstants;

    [Authorize]
    public class VehicleController : Controller
    {
        private readonly IVehicleService vehicleService;
        private readonly IOwnerService ownerService;

        public VehicleController(IVehicleService vehicleService, IOwnerService ownerService)
        {
            this.vehicleService = vehicleService;
            this.ownerService = ownerService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            try
            {
                var models = await vehicleService.GetAllVehiclesAsync();
                TempData[InformationMessage] = "All active vehicles are viewed";
                return View(models);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            VehicleViewModelAdd modelAdd = new VehicleViewModelAdd();
            try
            {
                modelAdd.Owners = await ownerService.GetAllActiveOwnersAsync();
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("Index", "Home");
            }

            return View(modelAdd);
        }

        [HttpPost]
        public async Task<IActionResult> Add(VehicleViewModelAdd viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = InvalidDataErrorMassage;
                try
                {
                    viewModel.Owners = await ownerService.GetAllActiveOwnersAsync();
                    return View(viewModel);
                }
                catch (Exception)
                {
                    TempData[ErrorMessage] = DatabaseErrorMassage;
                    return RedirectToAction("Index", "Home");
                }
            }

            try
            {
                await vehicleService.AddNewVehicleAsync(viewModel);
                TempData[SuccessMessage] = "New vehicle added!";
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
            }

            return RedirectToAction("All", "Vehicle");
        }

        [HttpPost]
        public async Task<IActionResult> ViewVehicleInfo(string regNumber)
        {
            try
            {
                VehicleViewModelDetails model = await vehicleService.GetVehicleByIdAsync(regNumber);
                TempData[SuccessMessage] = "Vehicle info is viewed";
                return View(model);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = $"No info for vehicle with Registration number: {regNumber}";
                return RedirectToAction("All", "Vehicle");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string regNumber)
        {
            try
            {
                VehicleViewModelAdd model = await vehicleService.GetVehicleByIdForEditAsync(regNumber);
                model.Owners = await ownerService.GetAllActiveOwnersAsync();
                TempData[WarningMessage] = "Vehicle is viewed for Edit!";
                return View("EditVehicle", model);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = $"No info for vehicle with Registration number: {regNumber}";
                return RedirectToAction("All", "Vehicle");
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditVehicle(VehicleViewModelAdd vehicleModelEdit)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    TempData[ErrorMessage] = InvalidDataErrorMassage;
                    vehicleModelEdit.Owners = await ownerService.GetAllActiveOwnersAsync();
                }
                catch (Exception)
                {
                    TempData[ErrorMessage] = DatabaseErrorMassage;
                }
                return View("EditVehicle", vehicleModelEdit);
            }

            try
            {
                await vehicleService.EditVehicleAsync(vehicleModelEdit);
                TempData[SuccessMessage] = "Vehicle edited!";
                return RedirectToAction("All", "Vehicle");
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("All", "Vehicle");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string regNumber)
        {
            try
            {
                bool isCompleted = await vehicleService.DeleteVehicleByIdAsync(regNumber);

                if (isCompleted)
                {
                    TempData[SuccessMessage] = "Vehicle Deleted! To restore contact Admin!";
                    return RedirectToAction("All", "Vehicle");
                }
                TempData[ErrorMessage] = "Vehicle is not deleted";
                return RedirectToAction("All", "Vehicle");
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("All", "Vehicle");
            }
        }
    }
}
