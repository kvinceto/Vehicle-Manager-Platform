namespace Vmp.Web.Controllers
{
    using System.Web;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.VehicleViewModels;

    using static Vmp.Common.NotificationMessagesConstants;
    using static Vmp.Services.Extensions.HelperClass;

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
            foreach (var item in models)
            {
                item.EncodedNumber = ReplaceBulgarianCharsWithEnglish(item.Number);
            }
            return View(models);
        }

        [HttpGet]
        public IActionResult Add()
        {
            VehicleViewModelAdd modelAdd = new VehicleViewModelAdd();
            return View(modelAdd);
        }

        [HttpPost]
        public async Task<IActionResult> Add(VehicleViewModelAdd viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "Enter valid data!";
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
    }
}
