namespace Vmp.Tests.ServicesTests
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    using Vmp.Data;
    using Vmp.Data.Models;
    using Vmp.Services;
    using Vmp.Services.Interfaces;

    [TestFixture]
    public class AdminServicesTests
    {
        private VehicleManagerDbContext dbContext;
        private IAdminService adminService;
        private string testUserId = "5c35b64b-b218-4548-ba50-5b75879a422f";
        private Waybill waybill;
        private Vehicle vehicle;
        private CostCenter costCenter;
        private DateCheck dateCheck;
        private MileageCheck mileageCheck;
        private Owner owner;
        private TaskModel taskModel;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<VehicleManagerDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;
            dbContext = new VehicleManagerDbContext(options);
            adminService = new AdminService(dbContext);

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

            taskModel = new TaskModel()
            {
                Id = 1,
                Name = "Test",
                Description = "Test",
                EndDate = DateTime.Parse("31/12/2023"),
                IsCompleted = true,
                UserId = Guid.Parse(testUserId)
            };

            await dbContext.Tasks.AddAsync(taskModel);

            owner = new Owner()
            {
                Id = 1,
                Info = null,
                IsInactive = true,
                Name = "Test"
            };

            await dbContext.Owners.AddAsync(owner);

            costCenter = new CostCenter()
            {
                Id = 1,
                Name = "Test",
                IsClosed = true
            };

            await dbContext.CostCenters.AddAsync(costCenter);

            vehicle = new Vehicle()
            {
                Number = "ТХ0000ХТ",
                Make = "TEST",
                Model = "TEST",
                Mileage = 200000,
                FuelCapacity = 60.00M,
                FuelQuantity = 20M,
                FuelCostRate = 0.07M,
                IsDeleted = true,
                ModelImgUrl = "url",
                OwnerId = 1,
                VIN = "TEST123456789"
            };

            await dbContext.Vehicles.AddAsync(vehicle);

            waybill = new Waybill()
            {
                Id = 1,
                Info = "Info",
                Date = DateTime.Parse("31/12/2023"),
                MileageStart = vehicle.Mileage,
                MileageEnd = (vehicle.Mileage + 100),
                DateCreated = DateTime.UtcNow.Date,
                MileageTraveled = 100,
                FuelLoaded = 0,
                FuelQuantityStart = vehicle.FuelQuantity,
                FuelConsumed = 7,
                FuelQuantityEnd = (vehicle.FuelQuantity - 7),
                CostCenterId = 1,
                RouteTraveled = "test",
                UserId = Guid.Parse(testUserId),
                VehicleNumber = vehicle.Number
            };

            await dbContext.Waybills.AddAsync(waybill);

            dateCheck = new DateCheck()
            {
                Id = 1,
                EndDate = DateTime.Parse("31/12/2023"),
                IsCompleted = true,
                Name = "Test",
                UserId = Guid.Parse(testUserId),
                VehicleNumber = vehicle.Number
            };

            await dbContext.DateChecks.AddAsync(dateCheck);

            mileageCheck = new MileageCheck()
            {
                Id = 1,
                ExpectedMileage = 1,
                IsCompleted = true,
                Name = "Test",
                UserId = Guid.Parse(testUserId),
                VehicleNumber = vehicle.Number
            };

            await dbContext.MileageChecks.AddAsync(mileageCheck);

            await dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task GetAllCostCenteresAsyncReturnsAll()
        {
            var result = await adminService.GetAllCostCenteresAsync();

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetAllDateChecksAsyncReturnsAll()
        {
            var result = await adminService.GetAllDateChecksAsync();

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetAllMileageChecksAsyncReturnsAll()
        {
            var result = await adminService.GetAllMileageChecksAsync();

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetAllOwnersAsyncReturnsAll()
        {
            var result = await adminService.GetAllOwnersAsync();

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetAllTasksAsyncReturnsAll()
        {
            var result = await adminService.GetAllTasksAsync();

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetAllUsersReturnsAll()
        {
            var result = await adminService.GetAllUsers();

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetAllVehiclesAsyncReturnsAll()
        {
            var result = await adminService.GetAllVehiclesAsync();

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task RestoreCostCenterByIdAsyncNotFound()
        {
            var result = await adminService.RestoreCostCenterByIdAsync(1515);
            string expected = $"Cost Center with id: {1515} not found!";
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public async Task RestoreCostCenterByIdAsyncReturns()
        {
            var result = await adminService.RestoreCostCenterByIdAsync(costCenter.Id);
            string expected = $"Cost Center with id: {costCenter.Id} restored!";
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public async Task RestoreDateCheckByIdAsyncNotFound()
        {
            var result = await adminService.RestoreDateCheckByIdAsync(1515);
            string expected = $"Date Check with id: {1515} not found!";
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public async Task RestoreDateCheckByIdAsyncReturns()
        {
            var result = await adminService.RestoreDateCheckByIdAsync(dateCheck.Id);
            string expected = $"Date Check with id: {dateCheck.Id} restored!";
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public async Task RestoreMileageCheckByIdAsyncNotFound()
        {
            var result = await adminService.RestoreMileageCheckByIdAsync(1515);
            string expected = $"Mileage Check with id: {1515} not found!";
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public async Task RestoreMileageCheckByIdAsyncReturns()
        {
            var result = await adminService.RestoreMileageCheckByIdAsync(mileageCheck.Id);
            string expected = $"Mileage Check with id: {mileageCheck.Id} restored!";
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public async Task RestoreOwnerByIdAsyncNotFound()
        {
            var result = await adminService.RestoreOwnerByIdAsync(1515);
            string expected = $"Owner with id: {1515} not found!";
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public async Task RestoreOwnerByIdAsyncReturns()
        {
            var result = await adminService.RestoreOwnerByIdAsync(owner.Id);
            string expected = $"Owner with id: {owner.Id} restored!";
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public async Task RestoreTaskByIdAsyncNotFound()
        {
            var result = await adminService.RestoreTaskByIdAsync(1515);
            string expected = $"Task with id: {1515} not found!";
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public async Task RestoreTaskByIdAsyncReturns()
        {
            var result = await adminService.RestoreTaskByIdAsync(taskModel.Id);
            string expected = $"Task with id: {taskModel.Id} restored!";
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public async Task RestoreVehicleByIdAsyncNotFound()
        {
            var result = await adminService.RestoreVehicleByIdAsync("invalid");
            string expected = $"Vehicle with registration number: invalid not found!";
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public async Task RestoreVehicleByIdAsyncReturns()
        {
            var result = await adminService.RestoreVehicleByIdAsync(vehicle.Number);
            string expected = $"Vehicle with registration number: {vehicle.Number} restored!";
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeletedAsync();
        }
    }
}
