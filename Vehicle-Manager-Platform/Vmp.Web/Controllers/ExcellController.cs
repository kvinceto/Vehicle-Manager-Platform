namespace Vmp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using Vmp.Services.Interfaces;

    using static Vmp.Common.GlobalApplicationConstants;
    using static Vmp.Common.NotificationMessagesConstants;

    [Authorize]
    public class ExcellController : Controller
    {
        private readonly IExcelService excelService;

        public ExcellController(IExcelService excelService)
        {
            this.excelService = excelService;
        }

        [HttpGet]
        public async Task<IActionResult> ExportWaybill(int id)
        {
            try
            {
                var file = await excelService.GenerateExcelFileWaybillAsync(id);

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "waybill.xlsx");
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportVehicle(string registrationNumber)
        {
            try
            {
                var file = await excelService.GenerateExcelFileVehicleAsync(registrationNumber);

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "vehicle.xlsx");
            }
            catch (Exception)
            {

                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportAllTasks()
        {
            try
            {
                var file = await excelService.GenerateExcelFileAllTasksAsync();
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "alltasks.xlsx");
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpGet]
        public async Task<IActionResult> ExportWaybills(string number, string start, string end)
        {
            try
            {
                var file = await excelService.GenerateExcelFileForAllWaybillsAsync(number, start, end);

                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "allwaybills.xlsx");
            }
            catch (Exception)
            {

                TempData[ErrorMessage] = DatabaseErrorMassage;
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
