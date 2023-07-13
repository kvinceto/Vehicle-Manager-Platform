namespace Vmp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.VehicleViewModels;
    using static Vmp.Common.NotificationMessagesConstants;

    [Authorize]
    public class VehicleController : Controller
    {
        private readonly IVehicleService vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            this.vehicleService = vehicleService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var models = await vehicleService.GetAllVehiclesAsync();
            return View(models);
        }

        [HttpGet]
        public IActionResult Add()
        {
            VehicleViewModelAdd modelAdd = new VehicleViewModelAdd();
            return View(modelAdd);
        }

        [HttpPost]
        public async Task<IActionResult> Add(VehicleViewModelAdd model)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "Enter valid data!";
                return View(model);
            }

            try
            {
                await vehicleService.AddNewVehicleAsync(model);
                TempData[SuccessMessage] = "New vehicle added!";
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error in the database";
            }

            return RedirectToAction("All", "Vehicle");
        }
    }
}
