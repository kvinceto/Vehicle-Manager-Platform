namespace Vmp.Services
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
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

        /// <summary>
        /// This method creates a new CostCenter entity in the Database
        /// </summary>
        /// <param name="model">View Model for the data</param>
        /// <returns>Void</returns>
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

        /// <summary>
        /// This methos marks a cost center as deleted
        /// </summary>
        /// <param name="costCenterId">The Id of the Cost center</param>
        /// <returns>True or False</returns>
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

        /// <summary>
        /// This method edits a Cost center in the Database
        /// </summary>
        /// <param name="model">View Model for the data</param>
        /// <returns>Void</returns>
        public async Task EditCostCenterAsync(CostCenterViewModelEdit model)
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

        /// <summary>
        /// This method return Collection of all active cost centers Ordered by Name accending
        /// </summary>
        /// <returns>Collction of CostCenterViewModelAll</returns>
        public async Task<ICollection<CostCenterViewModelAll>> GetAllCostCentersAsync()
        {
            var models = await dbContext.CostCenters
                .Where(cc => cc.IsClosed == false)
                .OrderBy(cc => cc.Name)
                .Select(cc => new CostCenterViewModelAll
                {
                    Id = cc.Id,
                    Name = cc.Name
                })
                .ToArrayAsync();

            return models;
        }

        /// <summary>
        /// This method returns CostCenter
        /// </summary>
        /// <param name="costCenterid">Ths Id of the cost center</param>
        /// <returns>Cost Center of type CostCenterViewModelDetails</returns>
        public async Task<CostCenterViewModelDetails> GetCostCenterByIdAsync(int costCenterid)
        {
            CostCenterViewModelDetails? model = await dbContext.CostCenters
                .Where(cc => cc.Id == costCenterid)
                .Select(cc => new CostCenterViewModelDetails()
                {
                    Id = cc.Id,
                    Name = cc.Name,
                    IsClosed = cc.IsClosed,
                    WaybillsCount = cc.Waybills.Count()
                })
                .FirstOrDefaultAsync();

            if(model == null)
            {
                throw new NullReferenceException(nameof(model));
            }
                
            return model;
        }
    }
}
