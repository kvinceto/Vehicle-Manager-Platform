namespace Vmp.Tests.ServicesTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    using Vmp.Data.Models;
    using Vmp.Data;
    using Vmp.Services.Interfaces;
    using Vmp.Services;
    using Vmp.Web.ViewModels.VehicleViewModels;
    using Vmp.Web.ViewModels.OwnerViewModels;

    [TestFixture]
    public class VehicleServicesTests
    {
        private VehicleManagerDbContext dbContext;
        private IVehicleService vehicleService;
        private string testUserId = "5c35b64b-b218-4548-ba50-5b75879a422f";
        private Vehicle vehicle;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<VehicleManagerDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;
            dbContext = new VehicleManagerDbContext(options);
            vehicleService = new VehicleService(dbContext);

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
            await dbContext.SaveChangesAsync();

            Owner owner = new Owner()
            {
                Id = 1,
                Info = "info",
                Name = "name",
                IsInactive = false,
                Vehicles = new List<Vehicle>()
            };

            await dbContext.Owners.AddAsync(owner);
            await dbContext.SaveChangesAsync();

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
        }

        [Test]
        public async Task AddNewVehicleAsync()
        {
            VehicleViewModelAdd viewModelAdd = new VehicleViewModelAdd()
            {
                Number = vehicle.Number,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Mileage = vehicle.Mileage,
                FuelCapacity = vehicle.FuelCapacity,
                FuelQuantity = vehicle.FuelQuantity,
                FuelCostRate = vehicle.FuelCostRate,
                ModelImgUrl = vehicle.ModelImgUrl,
                OwnerId = vehicle.OwnerId,
                VIN = vehicle.VIN,
            };

            await vehicleService.AddNewVehicleAsync(viewModelAdd);

            var result = await dbContext.Vehicles.FirstAsync(v => v.Number == viewModelAdd.Number);

            Assert.IsNotNull(result);
            Assert.That(result.Number, Is.EqualTo(viewModelAdd.Number));
            Assert.That(result.Make, Is.EqualTo(viewModelAdd.Make));
            Assert.That(result.Model, Is.EqualTo(viewModelAdd.Model));
            Assert.That(result.Mileage, Is.EqualTo(viewModelAdd.Mileage));
            Assert.That(result.FuelCapacity, Is.EqualTo(viewModelAdd.FuelCapacity));
            Assert.That(result.FuelQuantity, Is.EqualTo(viewModelAdd.FuelQuantity));
            Assert.That(result.FuelCostRate, Is.EqualTo(viewModelAdd.FuelCostRate));
            Assert.That(result.ModelImgUrl, Is.EqualTo(viewModelAdd.ModelImgUrl));
            Assert.That(result.OwnerId, Is.EqualTo(viewModelAdd.OwnerId));
            Assert.That(result.VIN, Is.EqualTo(viewModelAdd.VIN));
            Assert.IsFalse(result.IsDeleted);
        }

        [Test]
        public async Task DeleteVehicleByIdAsyncReturnFalseInvalidNumber()
        {
            await dbContext.Vehicles.AddAsync(vehicle);
            await dbContext.SaveChangesAsync();

            bool result = await vehicleService.DeleteVehicleByIdAsync("invalid");

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteVehicleByIdAsyncReturnTrue()
        {
            await dbContext.Vehicles.AddAsync(vehicle);
            await dbContext.SaveChangesAsync();

            bool result = await vehicleService.DeleteVehicleByIdAsync(vehicle.Number);
            var v = await dbContext.Vehicles.FirstAsync(v => v.Number == vehicle.Number);

            Assert.IsTrue(result);
            Assert.IsTrue(v.IsDeleted);
        }

        [Test]
        public async Task EditVehicleAsyncThrowsNullInvalidVehicle()
        {
            await dbContext.Vehicles.AddAsync(vehicle);
            await dbContext.SaveChangesAsync();

            VehicleViewModelAdd viewModelAdd = new VehicleViewModelAdd()
            {
                Number = "invalid",
                Make = "TEST",
                Model = "TEST",
                Mileage = 200000,
                FuelCapacity = 60.00M,
                FuelQuantity = 20M,
                FuelCostRate = 0.07M,
                ModelImgUrl = "url",
                OwnerId = 1,
                VIN = "TEST123456789",
                Owners = new List<OwnerViewModelAll>()
            };

            Assert.ThrowsAsync<NullReferenceException>(async () => await vehicleService.EditVehicleAsync(viewModelAdd));
        }

        [Test]
        public async Task EditVehicleAsyncEditsVehicle()
        {
            await dbContext.Vehicles.AddAsync(vehicle);
            await dbContext.SaveChangesAsync();

            VehicleViewModelAdd viewModelAdd = new VehicleViewModelAdd()
            {
                Number = "ТХ0000ХТ",
                Make = "TEST2",
                Model = "TEST2",
                Mileage = 300000,
                FuelCapacity = 62.00M,
                FuelQuantity = 22M,
                FuelCostRate = 0.08M,
                ModelImgUrl = "url2",
                OwnerId = 1,
                VIN = "TEST12345678l",
                Owners = new List<OwnerViewModelAll>()
            };

            await vehicleService.EditVehicleAsync(viewModelAdd);

            var v = await dbContext.Vehicles.FirstAsync(v => v.Number == v.Number);

            Assert.IsNotNull(v);
            Assert.That(v.Number, Is.EqualTo(vehicle.Number));
            Assert.That(v.Make, Is.EqualTo(vehicle.Make));
            Assert.That(v.Model, Is.EqualTo(vehicle.Model));
            Assert.That(v.Mileage, Is.EqualTo(vehicle.Mileage));
            Assert.That(v.FuelCapacity, Is.EqualTo(vehicle.FuelCapacity));
            Assert.That(v.FuelQuantity, Is.EqualTo(vehicle.FuelQuantity));
            Assert.That(v.FuelCostRate, Is.EqualTo(vehicle.FuelCostRate));
            Assert.That(v.ModelImgUrl, Is.EqualTo(vehicle.ModelImgUrl));
            Assert.That(v.OwnerId, Is.EqualTo(vehicle.OwnerId));
            Assert.That(v.VIN, Is.EqualTo(vehicle.VIN));
        }

        [Test]
        public async Task GetAllAsyncReturnsAll()
        {
            await dbContext.Vehicles.AddAsync(vehicle);
            await dbContext.SaveChangesAsync();

            vehicle = new Vehicle()
            {
                Number = "ТХ0001ХТ",
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
            await dbContext.SaveChangesAsync();

            var v = await vehicleService.GetAllAsync();

            Assert.IsNotNull(v);
            Assert.That(v.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetAllVehiclesAsyncReturnsActive()
        {
            await dbContext.Vehicles.AddAsync(vehicle);
            await dbContext.SaveChangesAsync();

            vehicle = new Vehicle()
            {
                Number = "ТХ0001ХТ",
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
            await dbContext.SaveChangesAsync();

            vehicle = new Vehicle()
            {
                Number = "ТХ0002ХТ",
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

            var veh = await vehicleService.GetAllVehiclesAsync();

            Assert.NotNull(veh);
            Assert.That(veh.Count, Is.EqualTo(2));

            int n = 0;
            foreach (var v in veh)
            {
                n++;
                if (n == 1)
                {
                    Assert.That(v.Number == "ТХ0000ХТ");
                }
                else if (n == 2)
                {
                    Assert.That(v.Number == "ТХ0002ХТ");
                }
            }
        }

        [Test]
        public async Task GetVehicleByIdAsyncThrowsNullInvalidVehicle()
        {
            await dbContext.Vehicles.AddAsync(vehicle);
            await dbContext.SaveChangesAsync();

            Assert.ThrowsAsync<NullReferenceException>(async () => await vehicleService.GetVehicleByIdAsync("invalid"));
        }

        [Test]
        public async Task GetVehicleByIdAsyncReturnsVehicle()
        {
            await dbContext.Vehicles.AddAsync(vehicle);
            await dbContext.SaveChangesAsync();

            var v = await vehicleService.GetVehicleByIdAsync(vehicle.Number);

            Assert.NotNull(v);
            Assert.That(v.RegistrationNumber, Is.EqualTo(vehicle.Number));
            Assert.That(v.Model, Is.EqualTo(vehicle.Model));
            Assert.That(v.Make, Is.EqualTo(vehicle.Make));
            Assert.That(v.Mileage, Is.EqualTo(vehicle.Mileage));
            Assert.That(v.FuelQuantity, Is.EqualTo(vehicle.FuelQuantity));
            Assert.That(v.FuelCapacity, Is.EqualTo(vehicle.FuelCapacity));
            Assert.That(v.FuelCostRate, Is.EqualTo(vehicle.FuelCostRate));
            Assert.That(v.VIN, Is.EqualTo(vehicle.VIN));
            Assert.That(v.Owner, Is.EqualTo("name"));
            Assert.That(v.IsDeleted, Is.EqualTo(vehicle.IsDeleted));
            Assert.That(v.ModelImgUrl, Is.EqualTo(vehicle.ModelImgUrl));
            Assert.That(v.CountOfDateChecks, Is.EqualTo(0));
            Assert.That(v.CountOfMileageChecks, Is.EqualTo(0));
            Assert.That(v.CountOfWaybills, Is.EqualTo(0));
        }

        [Test]
        public async Task GetVehicleByIdForEditAsyncThrowsNullInvalidVehicle()
        {
            await dbContext.Vehicles.AddAsync(vehicle);
            await dbContext.SaveChangesAsync();

            Assert.ThrowsAsync<NullReferenceException>(async () => await vehicleService.GetVehicleByIdForEditAsync("invalid"));
        }

        [Test]
        public async Task GetVehicleByIdForEditAsyncReturnsVehicle()
        {
            await dbContext.Vehicles.AddAsync(vehicle);
            await dbContext.SaveChangesAsync();

            var v = await vehicleService.GetVehicleByIdForEditAsync(vehicle.Number);

            Assert.IsNotNull(v);
            Assert.That(v.Number , Is.EqualTo(vehicle.Number));
            Assert.That(v.Model , Is.EqualTo(vehicle.Model));
            Assert.That(v.Make , Is.EqualTo(vehicle.Make));
            Assert.That(v.Mileage , Is.EqualTo(vehicle.Mileage));
            Assert.That(v.FuelQuantity , Is.EqualTo(vehicle.FuelQuantity));
            Assert.That(v.FuelCapacity , Is.EqualTo(vehicle.FuelCapacity));
            Assert.That(v.FuelCostRate , Is.EqualTo(vehicle.FuelCostRate));
            Assert.That(v.VIN , Is.EqualTo(vehicle.VIN));
            Assert.That(v.OwnerId , Is.EqualTo(1));
            Assert.That(v.ModelImgUrl , Is.EqualTo(vehicle.ModelImgUrl));
        }

        [TearDown]
        public async Task TearDown()
        {
            await dbContext.Database.EnsureDeletedAsync();
        }
    }
}
