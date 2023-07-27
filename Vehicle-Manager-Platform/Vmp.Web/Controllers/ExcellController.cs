namespace Vmp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using Vmp.Services.Interfaces;

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
            var file = await excelService.GenerateExcelFileWaybillAsync(id);

            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "waybill.xlsx");
        }

        [HttpGet]
        public async Task<IActionResult> ExportVehicle(string registrationNumber)
        {
            var file = await excelService.GenerateExcelFileVehicleAsync(registrationNumber);

            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "vehicle.xlsx");
        }
    }
}
