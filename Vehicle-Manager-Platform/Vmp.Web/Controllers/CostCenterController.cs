namespace Vmp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.CostCenterViewModels;

    using static Vmp.Common.GlobalApplicationConstants;
    using static Vmp.Common.NotificationMessagesConstants;

    [Authorize]
    public class CostCenterController : Controller
    {
        private readonly ICostCenterService costCenterService;

        public CostCenterController(ICostCenterService costCenterService)
        {
            this.costCenterService = costCenterService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            try
            {
                ICollection<CostCenterViewModelAll> models =
                    await costCenterService.GetAllCostCentersAsync();
                TempData[InformationMessage] = "All active cost centers are viewed";
                return View(models);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
            CostCenterViewModelAdd modelAdd = new CostCenterViewModelAdd();
            return View(modelAdd);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CostCenterViewModelAdd model)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = InvalidDataErrorMassage;
                return View(model);
            }

            try
            {
                await costCenterService.AddNewCostCenterAsync(model);
                TempData[SuccessMessage] = "New Cost Center Added";
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
            }

            return RedirectToAction("All", "CostCenter");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                CostCenterViewModelDetails model = await costCenterService.GetCostCenterByIdAsync(id);
                TempData[InformationMessage] = "Cost center info is viewed";
                return View(model);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("All", "CostCenter");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool isCompleted = await costCenterService.DeleteCostCenterByIdAsync(id);
                if (isCompleted)
                {
                    TempData[SuccessMessage] = "Cost Center Deleted! To restore contact Admin!";
                    return RedirectToAction("All", "CostCenter");
                }
                TempData[ErrorMessage] = "Cost Center is not deleted";
                return RedirectToAction("All", "Cost Center");
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("All", "Cost Center");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await costCenterService.GetCostCenterByIdAsync(id);
                CostCenterViewModelEdit viewModel = new CostCenterViewModelEdit()
                {
                    Id = model.Id,
                    Name = model.Name
                };

                TempData[WarningMessage] = "Cost center viewed for edit";
                return View(viewModel);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("All", "CostCenter");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CostCenterViewModelEdit model)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = InvalidDataErrorMassage;
                return View(model);
            }

            try
            {
                await costCenterService.EditCostCenterAsync(model);
                TempData[SuccessMessage] = "Cost Center edited!";
                return RedirectToAction("All", "CostCenter");
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("All", "CostCenter");
            }
        }

    }
}
