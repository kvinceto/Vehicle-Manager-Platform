namespace Vmp.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Vmp.Data;
    using Vmp.Data.Models;
    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.VehicleViewModels;

    public class VehicleService : IVehicleService
    {
        private readonly VehicleManagerDbContext dbContext;

        public VehicleService(VehicleManagerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddNewVehicleAsync(VehicleViewModelAdd model)
        {
            Vehicle vehicle = new Vehicle()
            {
                Number = model.Number,
                Make = model.Make,
                Model = model.Model,
                Mileage = model.Mileage,
                FuelCapacity = model.FuelCapacity,
                FuelQuantity = model.FuelQuantity,
                FuelCostRate = model.FuelCostRate,
                IsDeleted = false,
                ModelImgUrl = model.ModelImgUrl,
                OwnerId = model.OwnerId,
                VIN = model.VIN
            };

            await dbContext.Vehicles.AddAsync(vehicle);
            await dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<VehicleViewModelShortInfo>> GetAllVehiclesAsync()
        {
            return await dbContext.Vehicles
                .Where(v => v.IsDeleted == false)
                .OrderBy(v => v.Number)
                .Select(v => new VehicleViewModelShortInfo
                {
                    Number = v.Number,
                    Make = v.Make,
                    Model = v.Model
                })
                .ToArrayAsync();
        }
    }
}
