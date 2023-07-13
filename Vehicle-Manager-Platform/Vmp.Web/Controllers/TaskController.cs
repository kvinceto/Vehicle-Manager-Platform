namespace Vmp.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Vmp.Services.Extensions;
    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.TaskViewModels;

    using static Vmp.Common.NotificationMessagesConstants;

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
        public async Task<IActionResult> Mine()
        {
            string? userId = this.User.GetId();
            ICollection<TaskViewModelAll> tasks = await taskService.GetMyTasks(userId);
            return View(tasks);
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
                TempData[ErrorMessage] = "Enter valid data";
                return View(model);
            }

            string? userId = this.User.GetId();

            try
            {
                await taskService.AddNewTaskAsync(model, userId);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error in the database! Refresh this page and enter valid data";
                return View();
            }

            return RedirectToAction("Mine", "Task");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                TaskViewModelAdd model = await taskService.GetTaskByIdForEdit(id);

                TempData[WarningMessage] = "Task viewed for edit";
                return View(model);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error in the database!";
                return RedirectToAction("Mine", "Task");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TaskViewModelAdd model)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "Enter valid data";
                return View(model);
            }

            try
            {
                await taskService.EditTask(model);
                TempData[SuccessMessage] = "Task edited!";
                return RedirectToAction("Mine", "Task");
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error in the database!";
                return RedirectToAction("Mine", "Task");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewTask(int id)
        {
            try
            {
                TaskViewModelDetails task = await taskService.GetTaskByIdAsync(id);

                TempData[SuccessMessage] = "Task Info Viewed";
                return View(task);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Error in the database!";
                return RedirectToAction("All", "Task");
            }
        }  

        [HttpPost]
        public async Task<IActionResult> Complete(TaskViewModelAdd model)
        {           
            bool isCompleted = await taskService.CompleteTaskByIdAsync((int)model.Id!);
            if (isCompleted)
            {
                TempData[SuccessMessage] = "Task Completed";
                return RedirectToAction("Mine", "Task");
            }
            TempData[ErrorMessage] = "Task is not completed";
            return RedirectToAction("Mine", "Task");
        }
    }
}
