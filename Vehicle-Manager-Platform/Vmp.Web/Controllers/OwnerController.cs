namespace Vmp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.OwnerViewModels;

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
    }
}
