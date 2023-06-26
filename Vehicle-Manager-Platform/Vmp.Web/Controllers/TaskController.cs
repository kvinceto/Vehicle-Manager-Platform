namespace Vmp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Vmp.Services.Interfaces;

    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskService taskService;

        public TaskController(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var tasks = await taskService.GetAllActiveTasksAsync();
            return View(tasks);
        }

        [HttpGet]
        public IActionResult Mine()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(object model)
        {           
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(object model)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ViewTask()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Complete(int taskId)
        {
            return RedirectToAction();
        }
    }
}
