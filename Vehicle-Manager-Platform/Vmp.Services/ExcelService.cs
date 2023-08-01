namespace Vmp.Services
{
    using IronXL;

    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.VehicleViewModels;
    using Vmp.Web.ViewModels.WaybillViewModels;

    public class ExcelService : IExcelService
    {
        private bool license = IronXL.License.IsValidLicense("IRONSUITE.KVI.2012.ABV.BG.23029-7400CFFD9F-C5UTNBK-NSXHUP7OLUD2-TM7ZVXSR2HW3-7E5YKDH5XGSJ-55UCRAWZQIAB-OBXXM43YOYTD-6D6VPEQOBOZA-CDPL3L-TZGXJ3ROV5KKEA-DEPLOYMENT.TRIAL-5YTKZU.TRIAL.EXPIRES.31.AUG.2023");
        private bool isLicensed = IronXL.License.IsLicensed;

        private readonly IWaybillService waybillService;
        private readonly IVehicleService vehicleService;
        private readonly ITaskService taskService;

        public ExcelService(IWaybillService waybillService, IVehicleService vehicleService, ITaskService taskService)
        {
            this.waybillService = waybillService;
            this.vehicleService = vehicleService;
            this.taskService = taskService;
        }

        /// <summary>
        /// This method returns a byte[] with list of tasks
        /// </summary>
        /// <returns>byte[]</returns>
        public async Task<byte[]> GenerateExcelFileAllTasksAsync()
        {
            IsLicensed();

            var tasks = await taskService.GetAllActiveTasksAsync();
            WorkBook workbook = new WorkBook();
            WorkSheet worksheet = workbook.CreateWorkSheet("AllTasks");
            worksheet["A1"].Value = "Task Name";
            worksheet["B1"].Value = "Deadline";
            worksheet["C1"].Value = "Username of the creator";

            int counter = 1;
            foreach (var task in tasks)
            {
                counter++;
                worksheet[$"A{counter}"].StringValue = task.Name;
                worksheet[$"B{counter}"].StringValue = task.EndDate;
                worksheet[$"C{counter}"].StringValue = task.User;
            }

            worksheet.AutoSizeColumn(0);
            worksheet.AutoSizeColumn(1);
            worksheet.AutoSizeColumn(2);

            workbook.ToStream();
            return workbook.ToByteArray();
        }

        /// <summary>
        /// This method returns a byte[] with vehicle data
        /// </summary>
        /// <param name="regNumber">Tha registration number of the vehicle</param>
        /// <returns>byte[]</returns>
        public async Task<byte[]> GenerateExcelFileVehicleAsync(string regNumber)
        {
            IsLicensed();

            VehicleViewModelDetails vehicle = await vehicleService.GetVehicleByIdAsync(regNumber);

            WorkBook workbook = new WorkBook();
            WorkSheet worksheet = workbook.CreateWorkSheet("VehicleData");


            worksheet["A1"].Value = "Registration Number";
            worksheet["A2"].Value = "VIN";
            worksheet["A3"].Value = "Model";
            worksheet["A4"].Value = "Make";
            worksheet["A5"].Value = "Mileage";
            worksheet["A6"].Value = "Fuel Quantity";
            worksheet["A7"].Value = "Fuel Capacity";
            worksheet["A8"].Value = "Fuel Cost Rate";
            worksheet["A9"].Value = "Owner";
            worksheet["A10"].Value = "Deleted";
            worksheet["A11"].Value = "Count Of Waybills";
            worksheet["A12"].Value = "Count Of Mileage Checks";
            worksheet["A13"].Value = "Count Of Date Checks";
            worksheet["A14"].Value = "Image";


            worksheet["B1"].StringValue = vehicle.RegistrationNumber;
            worksheet["B2"].StringValue = vehicle.VIN;
            if (vehicle.Model == null)
            {
                worksheet["B3"].StringValue = "No info";
            }
            else
            {
                worksheet["B3"].StringValue = vehicle.Model;
            }
            if (vehicle.Make == null)
            {
                worksheet["B4"].Value = "No info";
            }
            else
            {
                worksheet["B4"].Value = vehicle.Make;
            }
            worksheet["B5"].Int32Value = vehicle.Mileage;
            worksheet["B6"].DecimalValue = vehicle.FuelQuantity;
            worksheet["B7"].DecimalValue = vehicle.FuelCapacity;
            worksheet["B8"].DecimalValue = vehicle.FuelCostRate;
            worksheet["B9"].StringValue = vehicle.Owner;
            if (vehicle.IsDeleted)
            {
                worksheet["B10"].StringValue = "Deleted";
            }
            else
            {
                worksheet["B10"].StringValue = "Active";

            }
            worksheet["B11"].Int32Value = vehicle.CountOfWaybills;
            worksheet["B12"].Int32Value = vehicle.CountOfMileageChecks;
            worksheet["B13"].Int32Value = vehicle.CountOfDateChecks;
            if (vehicle.ModelImgUrl == null)
            {
                worksheet["B14"].StringValue = "No image";
            }
            else
            {
                worksheet["B14"].StringValue = "Has image";
            }

            worksheet.AutoSizeColumn(0);
            worksheet.AutoSizeColumn(1);

            workbook.ToStream();
            return workbook.ToByteArray();
        }



        public async Task<byte[]> GenerateExcelFileWaybillAsync(int waybillId)
        {
            IsLicensed();

            WaybillViewModelDetails waybill = await waybillService.GetWaybillByIdAsync(waybillId);

            WorkBook workbook = new WorkBook();
            WorkSheet worksheet = workbook.CreateWorkSheet("WaybillData");

            worksheet["A1"].Value = "Id";
            worksheet["A2"].Value = "Date";
            worksheet["A3"].Value = "Vehicle Number";
            worksheet["A4"].Value = "Mileage Start";
            worksheet["A5"].Value = "Mileage End";
            worksheet["A6"].Value = "Mileage Traveled";
            worksheet["A7"].Value = "Route Traveled";
            worksheet["A8"].Value = "Fuel Quantity Start";
            worksheet["A9"].Value = "Fuel Quantity End";
            worksheet["A10"].Value = "Fuel Consumed";
            worksheet["A11"].Value = "Fuel Loaded";
            worksheet["A12"].Value = "Info";
            worksheet["A13"].Value = "Date Created";
            worksheet["A14"].Value = "Cost Center Name";
            worksheet["A15"].Value = "Creator";

            worksheet["B1"].Int32Value = waybill.Id;
            worksheet["B2"].DateTimeValue = DateTime.Parse(waybill.Date);
            worksheet["B3"].Value = waybill.VehicleNumber;
            worksheet["B4"].Int32Value = waybill.MileageStart;
            worksheet["B5"].Int32Value = waybill.MileageEnd;
            worksheet["B6"].Int32Value = waybill.MileageTraveled;
            worksheet["B7"].StringValue = waybill.RouteTraveled;
            worksheet["B8"].DecimalValue = decimal.Parse(waybill.FuelQuantityStart);
            worksheet["B9"].DecimalValue = decimal.Parse(waybill.FuelQuantityEnd);
            worksheet["B10"].DecimalValue = decimal.Parse(waybill.FuelConsumed);
            worksheet["B11"].DecimalValue = decimal.Parse(waybill.FuelConsumed);
            worksheet["B12"].Value = waybill.Info;
            worksheet["B13"].DateTimeValue = DateTime.Parse(waybill.DateCreated);
            worksheet["B14"].StringValue = waybill.CostCenter;
            worksheet["B15"].StringValue = waybill.Creator;

            worksheet.AutoSizeColumn(0);
            worksheet.AutoSizeColumn(1);
            workbook.ToStream();
            return workbook.ToByteArray();
        }

        /// <summary>
        /// This method check is the IronXL licenced
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void IsLicensed()
        {
            if (!isLicensed)
            {
                throw new Exception("Not licensed!");
            }
        }
    }
}
