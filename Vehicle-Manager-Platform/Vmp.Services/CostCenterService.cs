namespace Vmp.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Vmp.Data;
    using Vmp.Data.Models;
    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.CostCenterViewModels;

    public class CostCenterService : ICostCenterService
    {
        private readonly VehicleManagerDbContext dbContext;

        public CostCenterService(VehicleManagerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddNewCostCenterAsync(CostCenterViewModelAdd model)
        {
            CostCenter costCenter = new CostCenter()
            {
                Name = model.Name,
                IsClosed = false
            };

            await dbContext.CostCenters.AddAsync(costCenter);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteCostCenterByIdAsync(int costCenterId)
        {
           CostCenter? costCenter = await dbContext.CostCenters
                .FirstOrDefaultAsync(cc => cc.Id == costCenterId);
            
            if (costCenter == null)
            {
                return false;
            }

            costCenter.IsClosed = true;
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task EditCostCenter(CostCenterViewModelEdit model)
        {
            var costCenter = await dbContext.CostCenters
                .FirstOrDefaultAsync(cc => cc.Id == model.Id);

            if (costCenter == null)
            {
                throw new NullReferenceException(nameof(costCenter));
            }

            costCenter.Name = model.Name;
            await dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<CostCenterViewModelAll>> GetAllCostCentersAsync()
        {
            var models = await dbContext.CostCenters
                .Where(cc => cc.IsClosed == false)
                .OrderBy(cc => cc.Id)
                .Select(cc => new CostCenterViewModelAll
                {
                    Id = cc.Id,
                    Name = cc.Name
                })
                .OrderBy(cc => cc.Id)
                .ToArrayAsync();

            return models;
        }

        public async Task<CostCenterViewModelDetails> GetCostCenterByIdAsync(int costCenterid)
        {
            CostCenterViewModelDetails model = await dbContext.CostCenters
                .Where(cc => cc.Id == costCenterid)
                .Select(cc => new CostCenterViewModelDetails()
                {
                    Id = cc.Id,
                    Name = cc.Name,
                    IsClosed = cc.IsClosed,
                    WaybillsCount = cc.Waybills.Count()
                })
                .FirstAsync();
                
            return model;
        }
    }
}
