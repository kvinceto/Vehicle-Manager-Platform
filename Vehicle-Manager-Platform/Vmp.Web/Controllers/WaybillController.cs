namespace Vmp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using Vmp.Services.Extensions;
    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.VehicleViewModels;
    using Vmp.Web.ViewModels.WaybillViewModels;

    using static Vmp.Common.GlobalApplicationConstants;
    using static Vmp.Common.NotificationMessagesConstants;

    [Authorize]
    public class WaybillController : Controller
    {
        private readonly IWaybillService waybillService;
        private readonly IVehicleService vehicleService;
        private readonly ICostCenterService costCenterService;

        public WaybillController(IWaybillService waybillService, IVehicleService vehicleService, ICostCenterService costCenterService)
        {
            this.waybillService = waybillService;
            this.vehicleService = vehicleService;
            this.costCenterService = costCenterService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                ICollection<VehicleViewModelShortInfo> vehicles = await vehicleService.GetAllVehiclesAsync();
                return View(vehicles);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Add(string regNumber)
        {
            try
            {
                WaybillViewModelAdd viewModel =
               await waybillService.GetWaybillForAddingAsync(regNumber);
                viewModel.CostCenters = await costCenterService.GetAllCostCentersAsync();
                return View(viewModel);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("Index", "Home");
            }


        }

        [HttpPost]
        public async Task<IActionResult> AddWaybill(WaybillViewModelAdd viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = InvalidDataErrorMassage;
                return RedirectToAction("Index", "Waybill");
            }

            if (viewModel.MileageTraveled < 0)
            {
                TempData[ErrorMessage] = InvalidDataErrorMassage;
                return RedirectToAction("Index", "Waybill");
            }

            string myId = User.GetId()!;

            try
            {
                await waybillService.AddWaybillAsync(viewModel, myId);
                TempData[SuccessMessage] = $"Waybill for {viewModel.VehicleNumber} added!";
                return RedirectToAction("Index", "Waybill");
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("Index", "Waybill");
            }
        }

        [HttpGet]
        public async Task<IActionResult> AllPeriod()
        {
            WaybillDatesViewModel viewModel = new WaybillDatesViewModel();
            try
            {
                viewModel.Vehicles = await vehicleService.GetAllAsync();
                return View(viewModel);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> All(WaybillDatesViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = InvalidDataErrorMassage;
                return RedirectToAction("AllPeriod", "Waybill");
            }

            try
            {
                ICollection<WaybillViewModelAll> waybills = await waybillService.GetAllForVehicleForPeriod(viewModel.VehicleNumber, viewModel.StartDate, viewModel.EndDate);
                TempData[SuccessMessage] = $"All Waybills for vehicle {viewModel.VehicleNumber} between {viewModel.StartDate} and {viewModel.EndDate} are viewed";
                ViewData["Text"] = $"for Vehicle: {viewModel.VehicleNumber} between {viewModel.StartDate} and {viewModel.EndDate}";
                ViewData["Number"] = viewModel.VehicleNumber;
                ViewData["Start"] = viewModel.StartDate;
                ViewData["End"] = viewModel.EndDate;

                return View(waybills);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> AllForCostCenter(int id)
        {
            try
            {
                ICollection<WaybillViewModelAll> waybills = await waybillService.GetAllForCostCenterAsync(id);
                TempData[InformationMessage] = $"All Waybills for Cost center with id: {id} are viewed";
                ViewData["Text"] = $"for Cost center with id: {id}";
                return View("All", waybills);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("All", "CostCenter");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AllForVehicle(string regNumber)
        {
            try
            {
                ICollection<WaybillViewModelAll> waybills = await waybillService.GetAllForVehicleAsync(regNumber);
                TempData[SuccessMessage] = $"All Waybills for {regNumber} are viewed";
                return View("All", waybills);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                WaybillViewModelDetails viewModel = await waybillService.GetWaybillByIdAsync(id);
                TempData[InformationMessage] = "Details for waybill are viewed";
                return View(viewModel);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("All", "Waybill");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                WaybillViewModelShort modelToCheckId = await waybillService.GetShortWaybillByIdAsync(id);

                if (!User.IsInRole(AdminRoleName))
                {
                    string myId = User.GetId()!;

                    if (myId != modelToCheckId.UserId)
                    {
                        ViewData["Error"] = "Only the creator of the waybill can edit it!";
                        return View("NoAccess");
                    }
                }

                WaybillViewModelEdit modelForEdit = await waybillService.GetWaybillForEditByIdAsync(id);

                TempData[WarningMessage] = "Waybill is viewed for Edit!";

                return View(modelForEdit);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
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
                return RedirectToAction("AllPeriod", "Waybill");
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("AllPeriod", "Waybill");
            }
        }
    }
}
