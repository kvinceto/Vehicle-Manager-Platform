using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

            if (viewModel.MileageTraveled < 0)
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

        [HttpGet]
        public async Task<IActionResult> All()
        {
            try
            {
                ICollection<WaybillViewModelAll> waybills = await waybillService.GetAll();
                TempData[SuccessMessage] = "All Waybills are viewed";
                return View(waybills);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error in the Database!";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                WaybillViewModelDetails viewModel = await waybillService.GetWaybillByIdAsync(id);
                TempData[SuccessMessage] = "Details for waybill are viewed";
                return View(viewModel);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error in tha Database!";
                return RedirectToAction("All", "Waybill");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                WaybillViewModelShort modelToCheckId = await waybillService.GetShortWaybillByIdAsync(id);

                string myId = User.GetId()!;

                if(myId != modelToCheckId.UserId)
                {
                    ViewData["Error"] = "Only the creator of the waybill can edit it!";
                    return View("NoAccess");
                }

                WaybillViewModelEdit modelForEdit = await waybillService.GetWaybillForEditByIdAsync(id);

                TempData[WarningMessage] = "Waybill is viewed for Edit!";

                return View(modelForEdit);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error in the Database!";
                return RedirectToAction("All", "Waybills");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditWaybill(WaybillViewModelEdit modelToEdit)
        {
            string myId = User.GetId()!;

            try
            {
                await waybillService.EditWaybillAsync(modelToEdit, myId);
                TempData[SuccessMessage] = "Waybill Edited!";
                return RedirectToAction("All", "Waybill");
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error in tha Database!";
                return RedirectToAction("All", "Waybill");
            }
        }
    }
}
