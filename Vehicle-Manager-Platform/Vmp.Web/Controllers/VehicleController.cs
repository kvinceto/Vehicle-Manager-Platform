namespace Vmp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.VehicleViewModels;

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
            var models = await vehicleService.GetAllVehiclesAsync();
            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            VehicleViewModelAdd modelAdd = new VehicleViewModelAdd();
            modelAdd.Owners = await ownerService.GetAllOwnersAsync();
           
            return View(modelAdd);
        }

        [HttpPost]
        public async Task<IActionResult> Add(VehicleViewModelAdd viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "Enter valid data!";
                viewModel.Owners = await ownerService.GetAllOwnersAsync();
                return View(viewModel);
            }

            try
            {
                await vehicleService.AddNewVehicleAsync(viewModel);
                TempData[SuccessMessage] = "New vehicle added!";
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error in the database";
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
                model.Owners = await ownerService.GetAllOwnersAsync();
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
                TempData[ErrorMessage] = "Enter valid data";
                vehicleModelEdit.Owners = await ownerService.GetAllOwnersAsync();
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
                TempData[ErrorMessage] = "Error in the database!";
                return RedirectToAction("All", "Vehicle");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string regNumber)
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
    }
}
