namespace Vmp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.CostCenterViewModels;

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
                return View(models);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error with the Database!";
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
                TempData[ErrorMessage] = "Enter valid data!";
                return View(model);
            }

            try
            {
                await costCenterService.AddNewCostCenter(model);
                TempData[SuccessMessage] = "New Cost Center Added";
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error in the database!";
            }

            return RedirectToAction("All", "CostCenter");
        }
    }
}
