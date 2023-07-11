namespace Vmp.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
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

        public async Task AddNewCostCenter(CostCenterViewModelAdd model)
        {
            CostCenter costCenter = new CostCenter()
            {
                Name = model.Name,
                IsClosed = false
            };

            await dbContext.CostCenters.AddAsync(costCenter);
            await dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<CostCenterViewModelAll>> GetAllCostCentersAsync()
        {
            var models = await dbContext.CostCenters
                .Where(cc => cc.IsClosed == false)
                .Select(cc => new CostCenterViewModelAll
                {
                    Id = cc.Id,
                    Name = cc.Name
                })
                .OrderBy(cc => cc.Id)
                .ToArrayAsync();

            return models;
        }


    }
}
