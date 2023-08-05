namespace Vmp.Tests.ServicesTests
{
    using Microsoft.EntityFrameworkCore;

    using Vmp.Data;
    using Vmp.Data.Models;
    using Vmp.Services;
    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.DateCheckViewModels;
    using Vmp.Web.ViewModels.VehicleViewModels;

    [TestFixture]
    public class DateCheckServicesTests
    {
        private VehicleManagerDbContext dbContext;
        private IDateCheckService dateCheckService;
        private string testUserId = "5c35b64b-b218-4548-ba50-5b75879a422f";
        private DateCheck dateCheck;
        private Vehicle vehicle;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<VehicleManagerDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;
            dbContext = new VehicleManagerDbContext(options);
            dateCheckService = new DateCheckService(dbContext);

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

            dateCheck = new DateCheck()
            {
                Id = 1,
                Name = "Test",
                EndDate = DateTime.Parse("31/12/2023"),
                IsCompleted = false,
                UserId = user.Id,
                VehicleNumber = vehicle.Number
            };
        }

        [Test]
        public async Task AddAsyncAddsCheck()
        {
            DateCheckViewModelAdd viewModelAdd = new DateCheckViewModelAdd()
            {
                Name = dateCheck.Name,
                EndDate = dateCheck.EndDate.ToString("dd/MM/yyyy"),
                VehicleNumber = dateCheck.VehicleNumber,
                Vehicles = new List<VehicleViewModelShortInfo>()
            };

            await dateCheckService.AddAsync(viewModelAdd, testUserId);

            var dc = await dbContext.DateChecks.FirstOrDefaultAsync(d => d.Id == dateCheck.Id);

            Assert.IsNotNull(dc);
            Assert.That(dc.Id, Is.EqualTo(dateCheck.Id));
            Assert.That(dc.Name, Is.EqualTo(dateCheck.Name));
            Assert.That(dc.EndDate, Is.EqualTo(dateCheck.EndDate));
            Assert.That(dc.VehicleNumber, Is.EqualTo(dateCheck.VehicleNumber));
            Assert.That(dc.UserId, Is.EqualTo(Guid.Parse(testUserId)));
            Assert.IsFalse(dc.IsCompleted);
        }

        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        [TestCase(0)]
        [TestCase(51)]
        public async Task CompleteCheckByIdAsyncInvalidCheck(int id)
        {
            await dbContext.DateChecks.AddAsync(dateCheck);
            await dbContext.SaveChangesAsync();

            var result = await dateCheckService.CompleteCheckByIdAsync(id, testUserId);

            Assert.IsNotNull(result);
            Assert.That(result, Is.EquivalentTo("not changed"));
        }

        [Test]
        public async Task CompleteCheckByIdAsyncInvalidUser()
        {
            await dbContext.DateChecks.AddAsync(dateCheck);
            await dbContext.SaveChangesAsync();

            var result = await dateCheckService.CompleteCheckByIdAsync(dateCheck.Id, "invalid");

            Assert.IsNotNull(result);
            Assert.That(result, Is.EquivalentTo("not me"));
        }

        [Test]
        public async Task CompleteCheckByIdAsyncDeletesCheck()
        {
            await dbContext.DateChecks.AddAsync(dateCheck);
            await dbContext.SaveChangesAsync();

            var result = await dateCheckService.CompleteCheckByIdAsync(dateCheck.Id, testUserId);
            var dc = await dbContext.DateChecks.FirstAsync(d => d.Id == dateCheck.Id);

            Assert.IsNotNull(result);
            Assert.That(result, Is.EquivalentTo("changed"));
            Assert.IsTrue(dc.IsCompleted);
        }

        [Test]
        public async Task EditAsyncThrowsNullInvalidCheck()
        {
            await dbContext.DateChecks.AddAsync(dateCheck);
            await dbContext.SaveChangesAsync();

            DateCheckViewModelEdit viewModelEdit = new DateCheckViewModelEdit()
            {
                Id = 22,
                EndDate = dateCheck.EndDate.ToString("dd/MM/yyyy"),
                Name = dateCheck.Name,
                UserId = dateCheck.UserId.ToString(),
                VehicleNumber = dateCheck.VehicleNumber,
                Vehicles = new List<VehicleViewModelShortInfo>()
            };

            Assert.ThrowsAsync<NullReferenceException>(async () => await dateCheckService.EditAsync(viewModelEdit));
        }

        [Test]
        public async Task EditAsyncEdits()
        {
            await dbContext.DateChecks.AddAsync(dateCheck);
            await dbContext.SaveChangesAsync();

            DateCheckViewModelEdit viewModelEdit = new DateCheckViewModelEdit()
            {
                Id = dateCheck.Id,
                EndDate = dateCheck.EndDate.ToString("dd/MM/yyyy"),
                Name = dateCheck.Name,
                UserId = dateCheck.UserId.ToString(),
                VehicleNumber = dateCheck.VehicleNumber,
                Vehicles = new List<VehicleViewModelShortInfo>()
            };

            await dateCheckService.EditAsync(viewModelEdit);

            var dc = await dbContext.DateChecks.FirstAsync(d => d.Id == dateCheck.Id);

            Assert.IsNotNull(dc);
            Assert.That(dc.Id, Is.EqualTo(dateCheck.Id));
            Assert.That(dc.EndDate, Is.EqualTo(dateCheck.EndDate));
            Assert.That(dc.VehicleNumber, Is.EqualTo(dateCheck.VehicleNumber));
            Assert.That(dc.UserId, Is.EqualTo(dateCheck.UserId));
            Assert.That(dc.Name, Is.EqualTo(dateCheck.Name));
        }

        [Test]
        public async Task GetAllForVehicleAsyncReturns0()
        {
            await dbContext.DateChecks.AddAsync(dateCheck);
            await dbContext.SaveChangesAsync();

            var result = await dateCheckService.GetAllForVehicleAsync("invalid");

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetAllForVehicleAsyncReturnsAll()
        {
            await dbContext.DateChecks.AddAsync(dateCheck);
            await dbContext.SaveChangesAsync();

            dateCheck = new DateCheck()
            {
                Id = 2,
                Name = "Test2",
                EndDate = DateTime.Parse("31/12/2023"),
                IsCompleted = false,
                UserId = Guid.Parse(testUserId),
                VehicleNumber = vehicle.Number
            };

            await dbContext.DateChecks.AddAsync(dateCheck);
            await dbContext.SaveChangesAsync();

            var result = await dateCheckService.GetAllForVehicleAsync(vehicle.Number);

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetAllMineAsyncReturns0()
        {
            await dbContext.DateChecks.AddAsync(dateCheck);
            await dbContext.SaveChangesAsync();

            var result = await dateCheckService.GetAllMineAsync("invalid");

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetAllMineAsyncReturnsAll()
        {
            await dbContext.DateChecks.AddAsync(dateCheck);
            await dbContext.SaveChangesAsync();

            dateCheck = new DateCheck()
            {
                Id = 2,
                Name = "Test2",
                EndDate = DateTime.Parse("31/12/2023"),
                IsCompleted = false,
                UserId = Guid.Parse(testUserId),
                VehicleNumber = vehicle.Number
            };

            await dbContext.DateChecks.AddAsync(dateCheck);
            await dbContext.SaveChangesAsync();

            var result = await dateCheckService.GetAllMineAsync(testUserId);

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        [TestCase(0)]
        [TestCase(51)]
        public async Task GetCheckByIdForEditAsyncThrowsNullInvalidCheck(int id)
        {
            await dbContext.DateChecks.AddAsync(dateCheck);
            await dbContext.SaveChangesAsync();

            Assert.ThrowsAsync<NullReferenceException>(async () => await dateCheckService.GetCheckByIdForEditAsync(id));
        }

        [Test]
        public async Task GetCheckByIdForEditAsyncReturnsCheck()
        {
            await dbContext.DateChecks.AddAsync(dateCheck);
            await dbContext.SaveChangesAsync();

            var dc = await dateCheckService.GetCheckByIdForEditAsync(dateCheck.Id);

            Assert.IsNotNull(dc);
            Assert.That(dc.Id, Is.EqualTo(dateCheck.Id));
            Assert.That(dc.EndDate, Is.EqualTo(dateCheck.EndDate.ToString("dd/MM/yyyy")));
            Assert.That(dc.Name, Is.EqualTo(dateCheck.Name));
            Assert.That(dc.UserId, Is.EqualTo(dateCheck.UserId.ToString()));
            Assert.That(dc.VehicleNumber, Is.EqualTo(dateCheck.VehicleNumber));
        }

        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        [TestCase(0)]
        [TestCase(51)]
        public async Task GetDateCheckByIdAsyncThrowsNullInvalidCheck(int id)
        {
            await dbContext.DateChecks.AddAsync(dateCheck);
            await dbContext.SaveChangesAsync();

            Assert.ThrowsAsync<NullReferenceException>(async () => await dateCheckService.GetDateCheckByIdAsync(id));
        }

        [Test]
        public async Task GetDateCheckByIdAsyncReturnsCheck()
        {
            await dbContext.DateChecks.AddAsync(dateCheck);
            await dbContext.SaveChangesAsync();

            var dc = await dateCheckService.GetDateCheckByIdAsync(dateCheck.Id);

            Assert.IsNotNull(dc);
            Assert.That(dc.Id, Is.EqualTo(dateCheck.Id));
            Assert.That(dc.Name, Is.EqualTo(dateCheck.Name));
            Assert.That(dc.EndDate, Is.EqualTo(dateCheck.EndDate.ToString("dd/MM/yyyy")));
            Assert.That(dc.User, Is.EquivalentTo("user@user.bg"));
            Assert.That(dc.VehicleNumber, Is.EqualTo(dateCheck.VehicleNumber));
            Assert.IsFalse(dc.IsCompleted);
        }

        [Test]
        public async Task GetAllDateChecksAsyncReturns0()
        {
            await dbContext.DateChecks.AddAsync(dateCheck);
            await dbContext.SaveChangesAsync();
            var r = await dateCheckService.CompleteCheckByIdAsync(dateCheck.Id, testUserId);

            var result = await dateCheckService.GetAllDateChecksAsync();

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetAllDateChecksAsyncReturnsAll()
        {
            await dbContext.DateChecks.AddAsync(dateCheck);
            await dbContext.SaveChangesAsync();
            
            var result = await dateCheckService.GetAllDateChecksAsync();

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [TearDown]
        public async Task TearDown()
        {
            await dbContext.Database.EnsureDeletedAsync();
        }
    }
}
