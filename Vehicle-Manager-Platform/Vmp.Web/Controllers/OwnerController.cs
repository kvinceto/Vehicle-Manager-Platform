namespace Vmp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Vmp.Services;
    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.OwnerViewModels;
    using Vmp.Web.ViewModels.TaskViewModels;

    using static Vmp.Common.NotificationMessagesConstants;

    [Authorize]
    public class OwnerController : Controller
    {
        private readonly IOwnerService ownerService;

        public OwnerController(IOwnerService ownerService)
        {
            this.ownerService = ownerService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            ICollection<OwnerViewModelAll> models = await ownerService.GetAllOwnersAsync();
            return View(models);
        }

        [HttpGet]
        public IActionResult Add()
        {
            OwnerViewModelAdd modelAdd = new OwnerViewModelAdd();
            return View(modelAdd);
        }

        [HttpPost]
        public async Task<IActionResult> Add(OwnerViewModelAdd model)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "Enter valid data!";
                return View(model);
            }

            try
            {
                await ownerService.AddNewOwnerAsync(model);
                TempData[SuccessMessage] = "New Owner Added";
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error in the database!";
            }

            return RedirectToAction("All", "Owner");
        }

        [HttpGet]
        public async Task<IActionResult> ViewOwnerInfo(int id)
        {
            try
            {
                OwnerViewModelInfo model = await ownerService.GetOwnerByIdAsync(id);
                TempData[SuccessMessage] = "Owner info is viewed";
                return View(model);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = $"No info for owner with Id: {id}";
                return RedirectToAction("All", "Owner");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            bool isCompleted = await ownerService.DeleteOwnerByIdAsync(id);
            if (isCompleted)
            {
                TempData[SuccessMessage] = "Owner Deleted! To restore contact Admin!";
                return RedirectToAction("All", "Owner");
            }
            TempData[ErrorMessage] = "Owner is not deleted";
            return RedirectToAction("All", "Owner");
        }
    }
}
