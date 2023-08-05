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
    using Vmp.Web.ViewModels.CostCenterViewModels;

    [TestFixture]
    public class CostCenterServicesTests
    {
        private VehicleManagerDbContext dbContext;
        private ICostCenterService costCenterService;
        private string testUserId = "5c35b64b-b218-4548-ba50-5b75879a422f";
        private CostCenter costCenter;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<VehicleManagerDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;
            dbContext = new VehicleManagerDbContext(options);
            costCenterService = new CostCenterService(dbContext);

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

            costCenter = new CostCenter()
            {
                Id = 1,
                Name = "Test",
                IsClosed = false,
                Waybills = new List<Waybill>()
            };
        }

        [Test]
        public async Task AddNewCostCenterAsyncAddsCostCenter()
        {
            CostCenterViewModelAdd viewModelAdd = new CostCenterViewModelAdd()
            {
                Name = costCenter.Name
            };

            await costCenterService.AddNewCostCenterAsync(viewModelAdd);

            var cc = await dbContext.CostCenters.FirstAsync(cc => cc.Id == costCenter.Id);

            Assert.IsNotNull(cc);
            Assert.That(cc.Id, Is.EqualTo(costCenter.Id));
            Assert.That(cc.Name, Is.EqualTo(costCenter.Name));
            Assert.That(cc.IsClosed, Is.EqualTo(costCenter.IsClosed));
            Assert.That(cc.Waybills.Count, Is.EqualTo(0));
        }

        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        [TestCase(0)]
        [TestCase(455)]
        public async Task DeleteCostCenterByIdAsyncReturnFalseInvalidId(int id)
        {
            await dbContext.CostCenters.AddAsync(costCenter);
            await dbContext.SaveChangesAsync();

            bool result = await costCenterService.DeleteCostCenterByIdAsync(id);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteCostCenterByIdAsyncDeletesCostCenter()
        {
            await dbContext.CostCenters.AddAsync(costCenter);
            await dbContext.SaveChangesAsync();

            bool result = await costCenterService.DeleteCostCenterByIdAsync(costCenter.Id);
            var cc = await dbContext.CostCenters.FirstAsync(c => c.Id == costCenter.Id);

            Assert.IsTrue(result);
            Assert.IsTrue(cc.IsClosed);
        }

        [Test]
        public async Task EditCostCenterAsyncThrowsNullInvalidId()
        {
            await dbContext.CostCenters.AddAsync(costCenter);
            await dbContext.SaveChangesAsync();

            CostCenterViewModelEdit viewModelEdit = new CostCenterViewModelEdit()
            {
                Id = 125,
                Name = "Name"
            };

            Assert.ThrowsAsync<NullReferenceException>(async () => await costCenterService.EditCostCenterAsync(viewModelEdit));
        }

        [Test]
        public async Task EditCostCenterAsyncEdits()
        {
            await dbContext.CostCenters.AddAsync(costCenter);
            await dbContext.SaveChangesAsync();

            CostCenterViewModelEdit viewModelEdit = new CostCenterViewModelEdit()
            {
                Id = costCenter.Id,
                Name = "New Name"
            };

            await costCenterService.EditCostCenterAsync(viewModelEdit);

            var cc = await dbContext.CostCenters.FirstAsync(c => c.Id == costCenter.Id);

            Assert.IsNotNull(cc);
            Assert.That(cc.Name, Is.EqualTo(viewModelEdit.Name));
            Assert.That(cc.Id, Is.EqualTo(viewModelEdit.Id));
        }

        [Test]
        public async Task GetAllCostCentersAsyncReturnsList()
        {
            await dbContext.CostCenters.AddAsync(costCenter);
            await dbContext.SaveChangesAsync();

            costCenter = new CostCenter()
            {
                Id = 2,
                Name = "Test2",
                IsClosed = false,
                Waybills = new List<Waybill>()
            };

            await dbContext.CostCenters.AddAsync(costCenter);
            await dbContext.SaveChangesAsync();

            var cc = await costCenterService.GetAllCostCentersAsync();

            Assert.IsNotNull(cc);
            Assert.That(cc.Count, Is.EqualTo(2));

            int n = 0;
            foreach (var c in cc)
            {
                n++;
                Assert.That(c.Id, Is.EqualTo(n));
            }
        }

        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        [TestCase(0)]
        [TestCase(2548)]
        public async Task GetCostCenterByIdAsyncThrowsNullInvalidId(int id)
        {
            await dbContext.CostCenters.AddAsync(costCenter);
            await dbContext.SaveChangesAsync();

            Assert.ThrowsAsync<NullReferenceException>(async () => await costCenterService.GetCostCenterByIdAsync(id));
        }

        [Test]
        public async Task GetCostCenterByIdAsyncReturns()
        {
            await dbContext.CostCenters.AddAsync(costCenter);
            await dbContext.SaveChangesAsync();

            var cc = await costCenterService.GetCostCenterByIdAsync(costCenter.Id);

            Assert.IsNotNull(cc);
            Assert.That(cc.Id, Is.EqualTo(costCenter.Id));
            Assert.That(cc.Name, Is.EqualTo(costCenter.Name));
            Assert.That(cc.IsClosed, Is.EqualTo(costCenter.IsClosed));
            Assert.That(cc.WaybillsCount, Is.EqualTo(costCenter.Waybills.Count));
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeletedAsync();
        }
    }
}
