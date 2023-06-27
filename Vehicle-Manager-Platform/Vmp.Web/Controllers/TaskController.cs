namespace Vmp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Vmp.Services.Extensions;
    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.TaskViewModels;

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
        public IActionResult Add()
        {
            var model = new TaskViewModelAdd();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(TaskViewModelAdd model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string? userId = this.User.GetId();

            try
            {
                await taskService.AddNewTaskAsync(model, userId);
            }
            catch (Exception)
            {
                return View();
            }

            return RedirectToAction("Mine", "Task");
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
