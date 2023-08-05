namespace Vmp.Tests.ServicesTests
{
    using Microsoft.EntityFrameworkCore;

    using Vmp.Data;
    using Vmp.Data.Models;
    using Vmp.Services;
    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.TaskViewModels;
    using Vmp.Web.ViewModels.VehicleViewModels;
    using Vmp.Web.ViewModels.WaybillViewModels;

    [TestFixture]
    public class ExcelServicesTests
    {
        private VehicleManagerDbContext dbContext;
        private IWaybillService waybillService;
        private IVehicleService vehicleService;
        private ITaskService taskService;
        private IExcelService excelService;
        private string testUserId = "5c35b64b-b218-4548-ba50-5b75879a422f";

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<VehicleManagerDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;
            dbContext = new VehicleManagerDbContext(options);
            waybillService = new WaybillService(dbContext);
            vehicleService = new VehicleService(dbContext);
            taskService = new TaskService(dbContext);
            excelService = new ExcelService(waybillService, vehicleService, taskService);

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            ApplicationUser user = new ApplicationUser()
            {
                Id = Guid.Parse(testUserId),
                UserName = "user@user.bg",
                NormalizedUserName = "user@user.bg".ToUpper(),
                Email = "user@user.bg",
                NormalizedEmail = "user@user.bg".ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = "user@user.bg".ToUpper(),
            };

            await dbContext.AspNetUsers.AddAsync(user);

            Owner owner = new Owner()
            {
                Id = 1,
                Info = null,
                Name = "Test",
                IsInactive = false
            };

            await dbContext.Owners.AddAsync(owner);
            await dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task GenerateExcelFileAllTasksAsyncReturnsData()
        {
            await taskService.AddNewTaskAsync(new TaskViewModelAdd()
            {
                Id = 1,
                Deadline = "31/12/2023",
                Description = "Test",
                Name = "Test"
            }, testUserId);

            var result = await excelService.GenerateExcelFileAllTasksAsync();

            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GenerateExcelFileVehicleAsyncReturnsData()
        {
            await vehicleService.AddNewVehicleAsync(new VehicleViewModelAdd()
            {
                Number = "tx12345ff",
                FuelCapacity = 1,
                FuelCostRate = 1,
                FuelQuantity = 1,
                Make = "test",
                Mileage = 1212,
                Model = "test",
                ModelImgUrl = null,
                OwnerId = 1,
                VIN = "1234565789155"
            });

            var data = await excelService.GenerateExcelFileVehicleAsync("tx12345ff");

            Assert.IsNotNull(data);
        }

        [Test]
        public async Task GenerateExcelFileWaybillAsyncReturnsData()
        {
            await dbContext.Vehicles.AddAsync(new Vehicle()
            {
                Number = "th1235hh",
                FuelCapacity = 60,
                FuelCostRate = 1,
                FuelQuantity = 1,
                IsDeleted = false,
                Make = null,
                Model = null,
                Mileage = 1212,
                ModelImgUrl = null,
                OwnerId = 1,
                VIN = "jhgfdfg2h5gf545"
            });
            await dbContext.CostCenters.AddAsync(new CostCenter()
            {
                Id = 1,
                Name = "Test",
                IsClosed = false
            });

            await dbContext.SaveChangesAsync();

            await waybillService.AddWaybillAsync(new WaybillViewModelAdd()
            {
                Info = null,
                FuelLoaded = 20,
                FuelQuantityEnd = 20,
                FuelQuantityStart = 20,
                CostCenterId = 1,
                Date = "31/12/2023",
                MileageEnd = 120,
                MileageStart = 100,
                RouteTraveled = "test",
                VehicleNumber = "th1235hh"
            }, testUserId);

            var data = await excelService.GenerateExcelFileWaybillAsync(1);

            Assert.IsNotNull(data);
        }

        [Test]
        public async Task GenerateExcelFileForAllWaybillsAsyncReturnsData()
        {
            await dbContext.Vehicles.AddAsync(new Vehicle()
            {
                Number = "th1235hh",
                FuelCapacity = 60,
                FuelCostRate = 1,
                FuelQuantity = 1,
                IsDeleted = false,
                Make = null,
                Model = null,
                Mileage = 1212,
                ModelImgUrl = null,
                OwnerId = 1,
                VIN = "jhgfdfg2h5gf545"
            });
            await dbContext.CostCenters.AddAsync(new CostCenter()
            {
                Id = 1,
                Name = "Test",
                IsClosed = false
            });

            await dbContext.SaveChangesAsync();

            await waybillService.AddWaybillAsync(new WaybillViewModelAdd()
            {
                Info = null,
                FuelLoaded = 20,
                FuelQuantityEnd = 20,
                FuelQuantityStart = 20,
                CostCenterId = 1,
                Date = "30/12/2023",
                MileageEnd = 120,
                MileageStart = 100,
                RouteTraveled = "test",
                VehicleNumber = "th1235hh"
            }, testUserId);

            var data = await excelService.GenerateExcelFileForAllWaybillsAsync("th1235hh", "29/12/2023", "31/12/2023");

            Assert.IsNotNull(data);
        }

        [TearDown]
        public async Task TearDown()
        {
            await dbContext.Database.EnsureDeletedAsync();
        }
    }
}
