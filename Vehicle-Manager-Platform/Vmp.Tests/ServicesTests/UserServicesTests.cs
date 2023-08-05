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

    [TestFixture]
    public class UserServicesTests
    {
        private VehicleManagerDbContext dbContext;
        private IUserService userService;
        private string testUserId = "5c35b64b-b218-4548-ba50-5b75879a422f";
        private ApplicationUser user;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<VehicleManagerDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;
            dbContext = new VehicleManagerDbContext(options);
            userService = new UserService(dbContext);

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            user = new ApplicationUser()
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
        }


        [Test]
        public void DeleteDataAsyncThrowsNullInvalidUser()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await userService.DeleteDataAsync("invalid"));
        }

        [Test]
        public async Task DeleteDataAsyncDeletesUser()
        {
            await userService.DeleteDataAsync(testUserId);

            var u = await dbContext.AspNetUsers.FirstAsync(u => u.Id == Guid.Parse(testUserId));

            Assert.IsNull(u.UserName);
            Assert.IsNull(u.NormalizedUserName);
            Assert.IsNull(u.Email);
            Assert.IsNull(u.NormalizedEmail);
            Assert.IsNull(u.PasswordHash);
            Assert.IsNull(u.SecurityStamp);
            Assert.IsNull(u.ConcurrencyStamp);
            Assert.IsNull(u.PhoneNumber);
            Assert.IsNull(u.LockoutEnd);            
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Database.EnsureDeletedAsync();
        }
    }
}
