namespace Vmp.Services
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

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

        /// <summary>
        /// This method creates a new entity for Owner in the Database
        /// </summary>
        /// <param name="ownerModel">View Model for the data</param>
        /// <returns>Void</returns>
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

        /// <summary>
        /// This method marks an Owner as deleted
        /// </summary>
        /// <param name="id">The Id of the Owner</param>
        /// <returns>True or False</returns>
        public async Task<bool> DeleteOwnerByIdAsync(int id)
        {
            Owner? owner = await dbContext.Owners.FirstOrDefaultAsync(o => o.Id == id);

            if (owner == null)
            {
                return false;
            }

            owner.IsInactive = true;

            await dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// This method Edits the Owner
        /// </summary>
        /// <param name="ownerModel">View Model for the new data</param>
        /// <returns>Void</returns>
        /// <exception cref="NullReferenceException">If owner does now exists throws exception</exception>
        public async Task EditOwnerAsync(OwnerViewModelEdit ownerModel)
        {
            Owner? owner = await dbContext.Owners
                .FirstOrDefaultAsync(o => o.Id ==  ownerModel.Id);

            if(owner == null)
            {
                throw new NullReferenceException(nameof(owner));
            }

            owner.Name = ownerModel.Name;
            owner.Info = ownerModel.Info;

            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// This method returns Collection of all active Owners Ordered by Name accending
        /// </summary>
        /// <returns>Collection of type OwnerViewModelAll</returns>
        public async Task<ICollection<OwnerViewModelAll>> GetAllActiveOwnersAsync()
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

        /// <summary>
        /// This method returns Owner with collection of his vehicles
        /// </summary>
        /// <param name="ownerId">The Id of the Owner</param>
        /// <returns>Owner of type OwnerViewModelInfo</returns>
        /// <exception cref="NullReferenceException">If owner does now exists throws exception</exception>
        public async Task<OwnerViewModelInfo> GetOwnerByIdAsync(int ownerId)
        {
            Owner? owner = await dbContext.Owners
                  .FirstOrDefaultAsync(o => o.Id == ownerId);

            if (owner == null)
            {
                throw new NullReferenceException(nameof(owner));
            }

            List<string> vehicles = await dbContext.Vehicles
                .Where(v => v.OwnerId == ownerId)
                .Select(v => v.Number)
                .ToListAsync();

            OwnerViewModelInfo result = new OwnerViewModelInfo()
            {
                Id = ownerId,
                Name = owner.Name,
                Info = owner.Info,
                Vehicles = vehicles
            };

            return result;
        }


        /// <summary>
        /// This method returns Owner
        /// </summary>
        /// <param name="ownerId">The Id of the owner</param>
        /// <returns>Owner of type OwnerViewModelEdit</returns>
        /// <exception cref="NullReferenceException">If owner does now exists throws exception</exception>
        public async Task<OwnerViewModelEdit> GetOwnerByIdForEditAsync(int ownerId)
        {
            Owner? owner = await dbContext.Owners
                .AsNoTracking()
                .FirstOrDefaultAsync (o => o.Id == ownerId);

            if (owner == null)
            {
                throw new NullReferenceException(nameof(owner));
            }

            return new OwnerViewModelEdit()
            {
                Id= owner.Id,
                Name = owner.Name,
                Info = owner.Info
            };
        }
    }
}
