namespace Vmp.Tests.ServicesTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    using Vmp.Data.Models;
    using Vmp.Data;
    using Vmp.Services;
    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.OwnerViewModels;

    [TestFixture]
    public class OwnerServicesTests
    {
        private VehicleManagerDbContext dbContext;
        private IOwnerService ownerService;
        private string testUserId = "5c35b64b-b218-4548-ba50-5b75879a422f";
        private Owner owner;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<VehicleManagerDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;
            dbContext = new VehicleManagerDbContext(options);
            ownerService = new OwnerService(dbContext);

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

            owner = new Owner()
            {
                Id = 1,
                Name = "Test",
                Info = "Test",
                IsInactive = false,
                Vehicles = new List<Vehicle>()
            };
        }

        [Test]
        public async Task AddNewOwnerAsyncShouldAddOwner()
        {
            OwnerViewModelAdd viewModelAdd = new OwnerViewModelAdd()
            {
                Info = owner.Info,
                Name = owner.Name
            };

            await ownerService.AddNewOwnerAsync(viewModelAdd);

            var owners = await dbContext.Owners.ToArrayAsync();

            Assert.IsTrue(owners.Any());
            Assert.That(owners.Count(), Is.EqualTo(1));

            var o = owners[0];

            Assert.That(o.Id, Is.EqualTo(owner.Id));
            Assert.That(o.Name, Is.EqualTo(owner.Name));
            Assert.That(o.Info, Is.EqualTo(owner.Info));
            Assert.That(o.IsInactive, Is.EqualTo(owner.IsInactive));
        }

        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        [TestCase(0)]
        [TestCase(15655)]
        public async Task DeleteOwnerByIdAsyncReturnsFalseInvalidOwner(int id)
        {
            await dbContext.Owners.AddAsync(owner);
            await dbContext.SaveChangesAsync();

            bool result = await ownerService.DeleteOwnerByIdAsync(id);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteOwnerByIdAsyncReturnsTrueAndDeleteOwner()
        {
            await dbContext.Owners.AddAsync(owner);
            await dbContext.SaveChangesAsync();

            bool result = await ownerService.DeleteOwnerByIdAsync(owner.Id);

            Assert.IsTrue(result);

            var o = await dbContext.Owners.FirstAsync();

            Assert.IsTrue(o.IsInactive);
        }

        [Test]
        public async Task EditOwnerAsyncThrowsNullInvalidOwner()
        {
            await dbContext.Owners.AddAsync(owner);
            await dbContext.SaveChangesAsync();

            OwnerViewModelEdit viewModelEdit = new OwnerViewModelEdit()
            {
                Id = 2656,
                Info = null,
                Name = "Test"
            };

            Assert.ThrowsAsync<NullReferenceException>(async () => await ownerService.EditOwnerAsync(viewModelEdit));
        }


        [Test]
        public async Task EditOwnerAsyncEditsOwner()
        {
            await dbContext.Owners.AddAsync(owner);
            await dbContext.SaveChangesAsync();

            OwnerViewModelEdit viewModelEdit = new OwnerViewModelEdit()
            {
                Id = owner.Id,
                Info = "New Info",
                Name = "New Name"
            };

            await ownerService.EditOwnerAsync(viewModelEdit);

            var o = await dbContext.Owners.FirstAsync(o => o.Id == owner.Id);

            Assert.That(o.Name, Is.EqualTo(viewModelEdit.Name));
            Assert.That(o.Info, Is.EqualTo(viewModelEdit.Info));
        }

        [Test]
        public async Task GetAllActiveOwnersAsync()
        {
            await dbContext.Owners.AddAsync(owner);
            await dbContext.SaveChangesAsync();
            owner = new Owner()
            {
                Id = 2,
                Name = "Test2",
                Info = "Test2",
                IsInactive = false,
                Vehicles = new List<Vehicle>()
            };
            await dbContext.Owners.AddAsync(owner);
            await dbContext.SaveChangesAsync();

            var owners = await ownerService.GetAllActiveOwnersAsync();

            Assert.IsNotNull(owners);
            Assert.That(owners.Count, Is.EqualTo(2));

            int n = 0;
            foreach (var owner in owners)
            {
                n++;
                Assert.That(owner.Id, Is.EqualTo(n));
            }
        }

        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        [TestCase(0)]
        [TestCase(26545)]
        public async Task GetOwnerByIdAsyncThrowsNullInvalidOwner(int id)
        {
            await dbContext.Owners.AddAsync(owner);
            await dbContext.SaveChangesAsync();

            Assert.ThrowsAsync<NullReferenceException>(async () => await ownerService.GetOwnerByIdAsync(id));
        }

        [Test]
        public async Task GetOwnerByIdAsyncReturnsOwner()
        {
            await dbContext.Owners.AddAsync(owner);
            await dbContext.SaveChangesAsync();

            var o = await ownerService.GetOwnerByIdAsync(owner.Id);

            Assert.IsNotNull(o);
            Assert.That(o.Id, Is.EqualTo(owner.Id));
            Assert.That(o.Name, Is.EqualTo(owner.Name));
            Assert.That(o.Info, Is.EqualTo(owner.Info));
            Assert.That(o.Vehicles.Count, Is.EqualTo(0));
        }

        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        [TestCase(0)]
        [TestCase(2545151)]
        public async Task GetOwnerByIdForEditAsyncThrowsNullInvalidId(int id)
        {
            await dbContext.Owners.AddAsync(owner);
            await dbContext.SaveChangesAsync();

            Assert.ThrowsAsync<NullReferenceException>(async () => await ownerService.GetOwnerByIdForEditAsync(id));
        }

        [Test]
        public async Task GetOwnerByIdForEditAsyncReturnOwner()
        {
            await dbContext.Owners.AddAsync(owner);
            await dbContext.SaveChangesAsync();

            var o = await ownerService.GetOwnerByIdForEditAsync(owner.Id);

            Assert.IsNotNull(o);
            Assert.That(o.Id, Is.EqualTo(owner.Id));
            Assert.That(o.Name, Is.EqualTo(owner.Name));
            Assert.That(o.Info, Is.EqualTo(owner.Info));
        }


        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeletedAsync();
        }
    }
}
