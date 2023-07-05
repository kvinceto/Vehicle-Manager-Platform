namespace Vmp.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Vmp.Data;
    using Vmp.Data.Models;
    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.OwnerViewModels;

    public class OwnerService : IOwnerService
    {
        private readonly VehicleManagerDbContext dbContext;

        public OwnerService(VehicleManagerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddNewOwnerAsync(OwnerViewModelAdd ownerModel)
        {
            Owner newOwner = new Owner()
            {
                Name = ownerModel.Name,
                Info = ownerModel.Info != null ? ownerModel.Info : null,
                IsInactive = false
            };

            await dbContext.Owners.AddAsync(newOwner);
            await dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<OwnerViewModelAll>> GetAllOwnersAsync()
        {
            return await dbContext.Owners
                 .Where(o => o.IsInactive == false)
                 .AsNoTracking()
                 .OrderBy(o => o.Name)
                 .Select(o => new OwnerViewModelAll()
                 {
                     Id = o.Id,
                     Name = o.Name
                 })
                 .ToListAsync();
        }
    }
}
