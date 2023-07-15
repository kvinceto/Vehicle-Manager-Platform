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

        public async Task EditVehicle(VehicleViewModelAdd model)
        {
            Vehicle? vehicle = await dbContext.Vehicles
                .FirstOrDefaultAsync(v => v.Number == model.Number);
            if (vehicle == null)
            {
                throw new NullReferenceException(nameof(vehicle));
            }

            vehicle.Number = model.Number;
            vehicle.Make = model.Make;
            vehicle.Model = model.Model;
            vehicle.Mileage = model.Mileage;
            vehicle.FuelCapacity = model.FuelCapacity;
            vehicle.FuelCostRate = model.FuelCostRate;
            vehicle.FuelQuantity = model.FuelQuantity;
            vehicle.ModelImgUrl = model.ModelImgUrl;
            vehicle.OwnerId = model.OwnerId;
            vehicle.VIN = model.VIN;

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

        public async Task<VehicleViewModelDetails> GetVehicleByIdAsync(string regNumber)
        {
            var vehicle = await dbContext.Vehicles
                .AsNoTracking()
                .Include(v => v.Waybills)
                .Include(v => v.DateChecks)
                .Include(v => v.MileageChecks)
                .Include(v => v.Owner)
                .Where(v => v.Number == regNumber)
                .Select(v => new VehicleViewModelDetails()
                {
                    RegistrationNumber = v.Number,
                    Model = v.Model,
                    Make = v.Make,
                    Mileage = v.Mileage,
                    FuelQuantity = v.FuelQuantity,
                    FuelCapacity = v.FuelCapacity,
                    FuelCostRate = v.FuelCostRate,
                    VIN = v.VIN,
                    Owner = v.Owner.Name,
                    IsDeleted = v.IsDeleted,
                    ModelImgUrl = v.ModelImgUrl,
                    CountOfDateChecks = v.DateChecks.Count,
                    CountOfMileageChecks = v.MileageChecks.Count,
                    CountOfWaybills = v.Waybills.Count
                })
                .FirstOrDefaultAsync();

            if(vehicle == null)
            {
                throw new NullReferenceException(nameof(vehicle));
            }
            else
            {
                return vehicle;
            }     
        }

        public async Task<VehicleViewModelAdd> GetVehicleByIdForEditAsync(string regNumber)
        {
            var v = await dbContext
                .Vehicles
                .Include(v => v.Owner)
                .FirstAsync(v => v.Number == regNumber);

            VehicleViewModelAdd model = new VehicleViewModelAdd()
            {
                Number = v.Number,
                Model = v.Model,
                Make = v.Make,
                Mileage = v.Mileage,
                FuelQuantity = v.FuelQuantity,
                FuelCapacity = v.FuelCapacity,
                FuelCostRate = v.FuelCostRate,
                VIN = v.VIN,
                OwnerId = v.Owner.Id,
                ModelImgUrl = v.ModelImgUrl
            };

            return model;
        }
    }
}
