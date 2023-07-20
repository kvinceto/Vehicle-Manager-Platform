using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using Vmp.Services.Extensions;
using Vmp.Services.Interfaces;
using Vmp.Web.ViewModels.VehicleViewModels;
using Vmp.Web.ViewModels.WaybillViewModels;

using static Vmp.Common.NotificationMessagesConstants;

namespace Vmp.Web.Controllers
{
    [Authorize]
    public class WaybillController : Controller
    {
        private readonly IWaybillService waybillService;
        private readonly IVehicleService vehicleService;
        private readonly ICostCenterService costCenterService;
        private readonly IOwnerService ownerService;

        public WaybillController(IWaybillService waybillService, IVehicleService vehicleService, ICostCenterService costCenterService, IOwnerService ownerService)
        {
            this.waybillService = waybillService;
            this.vehicleService = vehicleService;
            this.costCenterService = costCenterService;
            this.ownerService = ownerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ICollection<VehicleViewModelShortInfo> vehicles = await vehicleService.GetAllVehiclesAsync();
            return View(vehicles);
        }

        [HttpPost]
        public async Task<IActionResult> Add(string regNumber)
        {
            WaybillViewModelAdd viewModel = 
                await waybillService.GetWaybillForAddingAsync(regNumber);
            viewModel.CostCenters = await costCenterService.GetAllCostCentersAsync();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddWaybill(WaybillViewModelAdd viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "Error! Invalid data!";
                return RedirectToAction("Index", "Waybill");
            }

            if(viewModel.MileageTraveled < 0)
            {
                TempData[ErrorMessage] = "Error! Invalid data!";
                return RedirectToAction("Index", "Waybill");
            }

            string myId = User.GetId()!;

            try
            {
                await waybillService.AddWaybill(viewModel, myId);
                TempData[SuccessMessage] = $"Waybill for {viewModel.VehicleNumber} added!";
                return RedirectToAction("Index", "Waybill");
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error in the Database!";
                return RedirectToAction("Index", "Waybill");
            }
        }
    }
}
