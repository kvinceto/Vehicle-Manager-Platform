﻿namespace Vmp.Services
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

        public async Task EditOwner(OwnerViewModelEdit ownerModel)
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

        public async Task<OwnerViewModelEdit> GetOwnerByIdForEditAsync(int ownerId)
        {
            Owner? owner = await dbContext.Owners
                .AsNoTracking()
                .FirstOrDefaultAsync (o => o.Id == ownerId);
            if (owner == null)
            {
                return null;
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
