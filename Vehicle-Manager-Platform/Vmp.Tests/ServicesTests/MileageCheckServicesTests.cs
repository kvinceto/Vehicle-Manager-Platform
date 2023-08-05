namespace Vmp.Tests.ServicesTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Vmp.Data.Models;
    using Vmp.Data;
    using Vmp.Services.Interfaces;
    using Vmp.Services;
    using Vmp.Web.ViewModels.MileageCheckViewModels;
    using Vmp.Web.ViewModels.VehicleViewModels;

    [TestFixture]
    public class MileageCheckServicesTests
    {
        private VehicleManagerDbContext dbContext;
        private IMileageCheckService mileageCheckService;
        private string testUserId = "5c35b64b-b218-4548-ba50-5b75879a422f";
        private MileageCheck mileageCheck;
        private Vehicle vehicle;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<VehicleManagerDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;
            dbContext = new VehicleManagerDbContext(options);
            mileageCheckService = new MileageCheckService(dbContext);

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

            mileageCheck = new MileageCheck()
            {
                Id = 1,
                Name = "Test",
                ExpectedMileage = 200000,
                IsCompleted = false,
                UserId = user.Id,
                VehicleNumber = vehicle.Number
            };
        }

        [Test]
        public async Task AddAsyncAddsCheck()
        {
            MileageCheckViewModelAdd viewModelAdd = new MileageCheckViewModelAdd()
            {
                Name = mileageCheck.Name,
                VehicleNumber = mileageCheck.VehicleNumber,
                ExpectedMileage = mileageCheck.ExpectedMileage,
                Vehicles = new List<VehicleViewModelShortInfo>()
            };

            await mileageCheckService.AddAsync(viewModelAdd, testUserId);

            var mc = await dbContext.MileageChecks.FirstAsync(m => m.Id == mileageCheck.Id);

            Assert.IsNotNull(mc);
            Assert.That(mc.Id, Is.EqualTo(mileageCheck.Id));
            Assert.That(mc.Name, Is.EqualTo(mileageCheck.Name));
            Assert.That(mc.VehicleNumber, Is.EqualTo(mileageCheck.VehicleNumber));
            Assert.That(mc.ExpectedMileage, Is.EqualTo(mileageCheck.ExpectedMileage));
            Assert.That(mc.IsCompleted, Is.EqualTo(mileageCheck.IsCompleted));
            Assert.That(mc.UserId, Is.EqualTo(mileageCheck.UserId));          
        }







        [TearDown]
        public async Task TearDown()
        {
            await dbContext.Database.EnsureDeletedAsync();
        }
    }
}
