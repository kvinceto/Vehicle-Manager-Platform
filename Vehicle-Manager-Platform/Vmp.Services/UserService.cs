namespace Vmp.Services
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Vmp.Data;
    using Vmp.Data.Models;
    using Vmp.Services.Interfaces;


    public class UserService : IUserService
    {
        private readonly VehicleManagerDbContext dbContext;

        public UserService(VehicleManagerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task DeleteDataAsync(string? id)
        {
            ApplicationUser? user = await dbContext.AspNetUsers
                .FirstOrDefaultAsync(u => u.Id.ToString() == id);

            if (user == null)
            {
                throw new NullReferenceException(nameof(user));
            }

            user.UserName = null;
            user.NormalizedUserName = null;
            user.Email = null;
            user.NormalizedEmail = null;
            user.PasswordHash = null;
            user.SecurityStamp = null;
            user.ConcurrencyStamp = null;
            user.PhoneNumber = null;
            user.LockoutEnd = null;

            await dbContext.SaveChangesAsync();
        }
    }
}
