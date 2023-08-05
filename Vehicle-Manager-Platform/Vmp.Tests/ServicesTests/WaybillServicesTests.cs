namespace Vmp.Tests.ServicesTests
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    using Vmp.Data;
    using Vmp.Data.Models;
    using Vmp.Services;
    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.WaybillViewModels;

    public class WaybillServicesTests
    {
        private VehicleManagerDbContext dbContext;
        private IWaybillService waybillService;
        private string testUserId = "5c35b64b-b218-4548-ba50-5b75879a422f";
        private Waybill waybill;
        private Vehicle vehicle;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<VehicleManagerDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;
            dbContext = new VehicleManagerDbContext(options);
            waybillService = new WaybillService(dbContext);

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
                IsInactive = false,
                Name = "Test"
            };

            await dbContext.Owners.AddAsync(owner);

            CostCenter costCenter = new CostCenter()
            {
                Id = 1,
                Name = "Test",
                IsClosed = false
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
                IsDeleted = false,
                ModelImgUrl = "url",
                OwnerId = 1,
                VIN = "TEST123456789"
            };

            await dbContext.Vehicles.AddAsync(vehicle);
            await dbContext.SaveChangesAsync();

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
        }

        [Test]
        public void AddWaybillAsyncThrowsNullInvalidVehicle()
        {
            WaybillViewModelAdd viewModelAdd = new WaybillViewModelAdd()
            {
                Date = waybill.Date.ToString("dd/MM/yyyy"),
                FuelLoaded = waybill.FuelLoaded,
                FuelQuantityEnd = waybill.FuelQuantityEnd,
                FuelQuantityStart = waybill.FuelQuantityStart,
                CostCenterId = waybill.CostCenterId,
                Info = waybill.Info,
                MileageEnd = waybill.MileageEnd,
                MileageStart = waybill.MileageStart,
                RouteTraveled = waybill.RouteTraveled,
                VehicleNumber = "invalid"
            };

            Assert.ThrowsAsync<NullReferenceException>(async () => await waybillService.AddWaybillAsync(viewModelAdd, testUserId));
        }

        [Test]
        public async Task AddWaybillAsyncAddsWaybill()
        {
            WaybillViewModelAdd viewModelAdd = new WaybillViewModelAdd()
            {
                Date = waybill.Date.ToString("dd/MM/yyyy"),
                FuelLoaded = waybill.FuelLoaded,
                FuelQuantityEnd = waybill.FuelQuantityEnd,
                FuelQuantityStart = waybill.FuelQuantityStart,
                CostCenterId = waybill.CostCenterId,
                Info = waybill.Info,
                MileageEnd = waybill.MileageEnd,
                MileageStart = waybill.MileageStart,
                RouteTraveled = waybill.RouteTraveled,
                VehicleNumber = vehicle.Number
            };

            await waybillService.AddWaybillAsync(viewModelAdd, testUserId);

            var w = await dbContext.Waybills.FirstOrDefaultAsync(w => w.Id == waybill.Id);

            Assert.IsNotNull(w);
            Assert.That(w.Id, Is.EqualTo(waybill.Id));
            Assert.That(w.Info, Is.EqualTo(waybill.Info));
            Assert.That(w.Date, Is.EqualTo(waybill.Date));
            Assert.That(w.MileageStart, Is.EqualTo(waybill.MileageStart));
            Assert.That(w.MileageEnd, Is.EqualTo(waybill.MileageEnd));
            Assert.That(w.MileageTraveled, Is.EqualTo(waybill.MileageTraveled));
            Assert.That(w.FuelLoaded, Is.EqualTo(waybill.FuelLoaded));
            Assert.That(w.FuelQuantityStart, Is.EqualTo(waybill.FuelQuantityStart));
            Assert.That(w.FuelConsumed, Is.EqualTo(waybill.FuelConsumed));
            Assert.That(w.FuelQuantityEnd, Is.EqualTo(waybill.FuelQuantityEnd));
            Assert.That(w.CostCenterId, Is.EqualTo(waybill.CostCenterId));
            Assert.That(w.RouteTraveled, Is.EqualTo(waybill.RouteTraveled));
            Assert.That(w.UserId, Is.EqualTo(waybill.UserId));
            Assert.That(w.VehicleNumber, Is.EqualTo(waybill.VehicleNumber));
        }

        [Test]
        public void EditWaybillAsyncThrowsNullInvalidWaybill()
        {
            WaybillViewModelEdit viewModelEdit = new WaybillViewModelEdit()
            {
                Id = 115
            };

            Assert.ThrowsAsync<NullReferenceException>(async () => await waybillService.EditWaybillAsync(viewModelEdit, testUserId));
        }

        [Test]
        public async Task EditWaybillAsyncThrowsNullInvalidVehicle()
        {
            WaybillViewModelAdd viewModelAdd = new WaybillViewModelAdd()
            {
                Date = waybill.Date.ToString("dd/MM/yyyy"),
                FuelLoaded = waybill.FuelLoaded,
                FuelQuantityEnd = waybill.FuelQuantityEnd,
                FuelQuantityStart = waybill.FuelQuantityStart,
                CostCenterId = waybill.CostCenterId,
                Info = waybill.Info,
                MileageEnd = waybill.MileageEnd,
                MileageStart = waybill.MileageStart,
                RouteTraveled = waybill.RouteTraveled,
                VehicleNumber = vehicle.Number
            };

            await waybillService.AddWaybillAsync(viewModelAdd, testUserId);


            WaybillViewModelEdit viewModelEdit = new WaybillViewModelEdit()
            {
                Id = waybill.Id,
                VehicleNumber = "invalid"
            };

            Assert.ThrowsAsync<NullReferenceException>(async () => await waybillService.EditWaybillAsync(viewModelEdit, testUserId));
        }

        [Test]
        public async Task EditWaybillAsyncEdits()
        {
            WaybillViewModelAdd viewModelAdd = new WaybillViewModelAdd()
            {
                Date = waybill.Date.ToString("dd/MM/yyyy"),
                FuelLoaded = waybill.FuelLoaded,
                FuelQuantityEnd = waybill.FuelQuantityEnd,
                FuelQuantityStart = waybill.FuelQuantityStart,
                CostCenterId = waybill.CostCenterId,
                Info = waybill.Info,
                MileageEnd = waybill.MileageEnd,
                MileageStart = waybill.MileageStart,
                RouteTraveled = waybill.RouteTraveled,
                VehicleNumber = vehicle.Number
            };

            await waybillService.AddWaybillAsync(viewModelAdd, testUserId);

            WaybillViewModelEdit viewModelEdit = new WaybillViewModelEdit()
            {

                Id = waybill.Id,
                OldDate = viewModelAdd.Date,
                NewDate = viewModelAdd.Date,
                VehicleNumber = viewModelAdd.VehicleNumber,
                MileageStart = viewModelAdd.MileageStart,
                NewMileageEnd = viewModelAdd.MileageEnd,
                OldMileageEnd = viewModelAdd.MileageEnd,
                NewRouteTraveled = "new route",
                OldRouteTraveled = viewModelAdd.RouteTraveled,
                FuelQuantityStart = viewModelAdd.FuelQuantityStart,
                FuelQuantityEnd = viewModelAdd.FuelQuantityEnd,
                NewFuelLoaded = viewModelAdd.FuelLoaded,
                OldFuelLoaded = viewModelAdd.FuelLoaded,
                NewInfo = viewModelAdd.Info,
                OldInfo = viewModelAdd.Info,
                CostCenterId = viewModelAdd.CostCenterId,
                OldCostCenterName = "Test"
            };


            await waybillService.EditWaybillAsync(viewModelEdit, testUserId);

            var w = await waybillService.GetWaybillByIdAsync(waybill.Id);

            Assert.That(w.RouteTraveled == "new route");
        }

        [Test]
        public async Task GetAllAsyncReturns0()
        {
            var result = await waybillService.GetAllAsync();

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetAllAsyncReturnsAll()
        {
            await dbContext.Waybills.AddAsync(waybill);

            waybill = new Waybill()
            {
                Id = 2,
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
            await dbContext.SaveChangesAsync();

            var result = await waybillService.GetAllAsync();

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetAllForCostCenterAsyncReturns0()
        {
            var result = await waybillService.GetAllForCostCenterAsync(1);

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetAllForCostCenterAsyncReturnsAll()
        {
            await dbContext.Waybills.AddAsync(waybill);

            waybill = new Waybill()
            {
                Id = 2,
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
            await dbContext.SaveChangesAsync();

            var result = await waybillService.GetAllForCostCenterAsync(1);

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetAllForVehicleAsyncReturns0()
        {
            var result = await waybillService.GetAllForVehicleAsync(vehicle.Number);

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetAllForVehicleAsyncReturnsAll()
        {

            await dbContext.Waybills.AddAsync(waybill);

            waybill = new Waybill()
            {
                Id = 2,
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
            await dbContext.SaveChangesAsync();

            var result = await waybillService.GetAllForVehicleAsync(vehicle.Number);

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetShortWaybillByIdAsyncThrowsNullInvalidWaybill()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await waybillService.GetShortWaybillByIdAsync(125));
        }

        [Test]
        public async Task GetShortWaybillByIdAsyncReturnsAll()
        {
            await dbContext.Waybills.AddAsync(waybill);
            await dbContext.SaveChangesAsync();

            var w = await waybillService.GetShortWaybillByIdAsync(waybill.Id);

            Assert.IsNotNull(w);
            Assert.That(w.Id, Is.EqualTo(waybill.Id));
            Assert.That(w.UserId, Is.EqualTo(waybill.UserId.ToString()));
        }

        [Test]
        public void GetWaybillByIdAsyncThrowsNullInvalidWaybill()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await waybillService.GetWaybillByIdAsync(125));
        }

        [Test]
        public async Task GetWaybillByIdAsyncReturnsWaybill()
        {
            await dbContext.Waybills.AddAsync(waybill);
            await dbContext.SaveChangesAsync();

            var w = await waybillService.GetWaybillByIdAsync(waybill.Id);

            Assert.IsNotNull(w);
        }

        [Test]
        public void GetWaybillForAddingAsyncThrowsNullInvalidVehicle()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await waybillService.GetWaybillForAddingAsync("invalid"));
        }

        [Test]
        public async Task GetWaybillForAddingAsyncReturnsWaybill()
        {
            var w = await waybillService.GetWaybillForAddingAsync(vehicle.Number);

            Assert.IsNotNull(w);
            Assert.That(w.VehicleNumber, Is.EqualTo(vehicle.Number));
            Assert.That(w.FuelQuantityStart, Is.EqualTo(vehicle.FuelQuantity));
            Assert.That(w.MileageStart, Is.EqualTo(vehicle.Mileage));
        }

        [Test]
        public void GetWaybillForEditByIdAsyncThrowsNullInvalidWaybill()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await waybillService.GetWaybillForEditByIdAsync(121));
        }

        [Test]
        public async Task GetWaybillForEditByIdAsyncReturnsWaybill()
        {
            await dbContext.Waybills.AddAsync(waybill);
            await dbContext.SaveChangesAsync();

            var w = await waybillService.GetWaybillForEditByIdAsync(waybill.Id);

            Assert.IsNotNull(w);
            Assert.That(w.Id, Is.EqualTo(waybill.Id));
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeletedAsync();
        }
    }
}
