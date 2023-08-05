namespace Vmp.Tests.ServicesTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    using Vmp.Data;
    using Vmp.Data.Models;
    using Vmp.Services;
    using Vmp.Services.Interfaces;
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

        [Test]
        public async Task CompleteCheckByIdAsyncInvalidCheckId()
        {
            await dbContext.MileageChecks.AddAsync(mileageCheck);
            await dbContext.SaveChangesAsync();

            var result = await mileageCheckService.CompleteCheckByIdAsync(25, testUserId);

            Assert.IsNotNull(result);
            Assert.That(result, Is.EquivalentTo("not changed"));

        }

        [Test]
        public async Task CompleteCheckByIdAsyncInvalidUserId()
        {
            await dbContext.MileageChecks.AddAsync(mileageCheck);
            await dbContext.SaveChangesAsync();

            var result = await mileageCheckService.CompleteCheckByIdAsync(mileageCheck.Id, "invalid");

            Assert.IsNotNull(result);
            Assert.That(result, Is.EquivalentTo("not me"));
        }

        [Test]
        public async Task CompleteCheckByIdAsyncDeletesCheck()
        {
            await dbContext.MileageChecks.AddAsync(mileageCheck);
            await dbContext.SaveChangesAsync();

            var result = await mileageCheckService.CompleteCheckByIdAsync(mileageCheck.Id, testUserId);
            var mc = await dbContext.MileageChecks.FirstAsync(m => m.Id == mileageCheck.Id);

            Assert.IsNotNull(result);
            Assert.That(result, Is.EquivalentTo("changed"));
            Assert.IsTrue(mc.IsCompleted);
        }

        [Test]
        public async Task EditAsyncThrowsNullInvalidCheck()
        {
            await dbContext.MileageChecks.AddAsync(mileageCheck);
            await dbContext.SaveChangesAsync();

            MileageCheckViewModelEdit viewModelEdit = new MileageCheckViewModelEdit()
            {
                Id = 256,
                ExpectedMileage = 152,
                Name = "invalid",
                UserId = testUserId,
                VehicleNumber = mileageCheck.VehicleNumber,
                Vehicles = new List<VehicleViewModelShortInfo>()
            };

            Assert.ThrowsAsync<NullReferenceException>(async () => await mileageCheckService.EditAsync(viewModelEdit));
        }

        [Test]
        public async Task EditAsyncEdits()
        {
            await dbContext.MileageChecks.AddAsync(mileageCheck);
            await dbContext.SaveChangesAsync();

            MileageCheckViewModelEdit viewModelEdit = new MileageCheckViewModelEdit()
            {
                Id = mileageCheck.Id,
                ExpectedMileage = (mileageCheck.ExpectedMileage + 100),
                Name = "Edited",
                UserId = testUserId,
                VehicleNumber = mileageCheck.VehicleNumber,
                Vehicles = new List<VehicleViewModelShortInfo>()
            };

            await mileageCheckService.EditAsync(viewModelEdit);

            var mc = await dbContext.MileageChecks.FirstOrDefaultAsync(m => m.Id == mileageCheck.Id);

            Assert.IsNotNull(mc);
            Assert.That(mc.Id, Is.EqualTo(viewModelEdit.Id));
            Assert.That(mc.ExpectedMileage, Is.EqualTo(viewModelEdit.ExpectedMileage));
            Assert.That(mc.Name, Is.EqualTo(viewModelEdit.Name));
            Assert.That(mc.UserId, Is.EqualTo(Guid.Parse(viewModelEdit.UserId)));
            Assert.That(mc.VehicleNumber, Is.EqualTo(viewModelEdit.VehicleNumber));
        }

        [Test]
        public async Task GetAllActiveAsyncReturnsActive()
        {
            await dbContext.MileageChecks.AddAsync(mileageCheck);
            await dbContext.SaveChangesAsync();

            mileageCheck = new MileageCheck()
            {
                Id = 2,
                Name = "Test2",
                ExpectedMileage = 200002,
                IsCompleted = false,
                UserId = Guid.Parse(testUserId),
                VehicleNumber = vehicle.Number
            };

            await dbContext.MileageChecks.AddAsync(mileageCheck);
            await dbContext.SaveChangesAsync();

            mileageCheck = new MileageCheck()
            {
                Id = 3,
                Name= "Test3",
                ExpectedMileage = 200003,
                IsCompleted = true,
                UserId = Guid.Parse(testUserId),
                VehicleNumber = vehicle.Number
            };

            await dbContext.MileageChecks.AddAsync(mileageCheck);
            await dbContext.SaveChangesAsync();

            var result = await mileageCheckService.GetAllActiveAsync();

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            for (int i = 1; i <= 2; i++)
            {
                Assert.That(result.Any(r => r.Id == i), Is.True);
            }
        }

        [Test]
        public async Task GetAllForVehicleAsyncReturns0()
        {
            await dbContext.MileageChecks.AddAsync(mileageCheck);
            await dbContext.SaveChangesAsync();

            mileageCheck = new MileageCheck()
            {
                Id = 2,
                Name = "Test2",
                ExpectedMileage = 200002,
                IsCompleted = false,
                UserId = Guid.Parse(testUserId),
                VehicleNumber = vehicle.Number
            };

            await dbContext.MileageChecks.AddAsync(mileageCheck);
            await dbContext.SaveChangesAsync();

            var result = await mileageCheckService.GetAllForVehicleAsync("invalid");

            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task GetAllForVehicleAsyncReturnsCorrect()
        {
            await dbContext.MileageChecks.AddAsync(mileageCheck);
            await dbContext.SaveChangesAsync();

            mileageCheck = new MileageCheck()
            {
                Id = 2,
                Name = "Test2",
                ExpectedMileage = 200002,
                IsCompleted = false,
                UserId = Guid.Parse(testUserId),
                VehicleNumber = vehicle.Number
            };

            await dbContext.MileageChecks.AddAsync(mileageCheck);
            await dbContext.SaveChangesAsync();

            var result = await mileageCheckService.GetAllForVehicleAsync(vehicle.Number);

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetAllMineAsyncReturns0()
        {
            await dbContext.MileageChecks.AddAsync(mileageCheck);
            await dbContext.SaveChangesAsync();

            mileageCheck = new MileageCheck()
            {
                Id = 2,
                Name = "Test2",
                ExpectedMileage = 200002,
                IsCompleted = false,
                UserId = Guid.Parse(testUserId),
                VehicleNumber = vehicle.Number
            };

            await dbContext.MileageChecks.AddAsync(mileageCheck);
            await dbContext.SaveChangesAsync();

            var result = await mileageCheckService.GetAllMineAsync("invalid");

            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task GetAllMineAsyncReturnsMine()
        {
            await dbContext.MileageChecks.AddAsync(mileageCheck);
            await dbContext.SaveChangesAsync();

            mileageCheck = new MileageCheck()
            {
                Id = 2,
                Name = "Test2",
                ExpectedMileage = 200002,
                IsCompleted = false,
                UserId = Guid.Parse(testUserId),
                VehicleNumber = vehicle.Number
            };

            await dbContext.MileageChecks.AddAsync(mileageCheck);
            await dbContext.SaveChangesAsync();

            var result = await mileageCheckService.GetAllMineAsync(testUserId);

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        [TestCase(0)]
        [TestCase(-5)]
        [TestCase(26545)]
        public async Task GetChechByIdAsyncThrowsNullInvalidCheck(int id)
        {
            await dbContext.MileageChecks.AddAsync(mileageCheck);
            await dbContext.SaveChangesAsync();

            Assert.ThrowsAsync<NullReferenceException>(async () => await mileageCheckService.GetChechByIdAsync(id));
        }

        [Test]
        public async Task GetChechByIdAsyncReturnsCheck()
        {
            await dbContext.MileageChecks.AddAsync(mileageCheck);
            await dbContext.SaveChangesAsync();

            var mc = await mileageCheckService.GetChechByIdAsync(mileageCheck.Id);

            Assert.That(mc.Id, Is.EqualTo(mileageCheck.Id));
            Assert.That(mc.Name, Is.EqualTo(mileageCheck.Name));
            Assert.That(mc.IsCompleted, Is.EqualTo(mileageCheck.IsCompleted));
            Assert.That(mc.ExpectedMileage, Is.EqualTo(mileageCheck.ExpectedMileage));
            Assert.That(mc.User, Is.EquivalentTo("user@user.bg"));
            Assert.That(mc.VehicleNumber, Is.EquivalentTo(mileageCheck.VehicleNumber)); 
        }

        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        [TestCase(0)]
        [TestCase(-5)]
        [TestCase(26545)]
        public async Task GetCheckByIdForEditAsyncThrowsNullInvalidCheck(int id)
        {
            await dbContext.MileageChecks.AddAsync(mileageCheck);
            await dbContext.SaveChangesAsync();

            Assert.ThrowsAsync<NullReferenceException>(async () => await mileageCheckService.GetCheckByIdForEditAsync(id));
        }

        [Test]
        public async Task GetCheckByIdForEditAsyncReturnsCheck()
        {
            await dbContext.MileageChecks.AddAsync(mileageCheck);
            await dbContext.SaveChangesAsync();

            var mc = await mileageCheckService.GetCheckByIdForEditAsync(mileageCheck.Id);

            Assert.IsNotNull(mc);
            Assert.That(mc.Id, Is.EqualTo(mileageCheck.Id));
            Assert.That(mc.Name, Is.EqualTo(mileageCheck.Name));
            Assert.That(mc.ExpectedMileage, Is.EqualTo(mileageCheck.ExpectedMileage));
            Assert.That(mc.UserId, Is.EquivalentTo(mileageCheck.UserId.ToString()));
            Assert.That(mc.VehicleNumber, Is.EquivalentTo(mileageCheck.VehicleNumber));
            Assert.That(mc.Vehicles.Count, Is.EqualTo(1));
        }

        [TearDown]
        public async Task TearDown()
        {
            await dbContext.Database.EnsureDeletedAsync();
        }
    }
}
