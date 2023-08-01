namespace Vmp.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

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

        /// <summary>
        /// This method creates a new Vehicle entry in tha Database
        /// </summary>
        /// <param name="model">Veiw Model for the data</param>
        /// <returns>Void</returns>
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

        /// <summary>
        /// This methos marks a vehicle as deleted
        /// </summary>
        /// <param name="regNumber">The registration number of the vehicle</param>
        /// <returns>True or False</returns>
        public async Task<bool> DeleteVehicleByIdAsync(string regNumber)
        {
            Vehicle? vehicle= await dbContext.Vehicles.FirstOrDefaultAsync(v => v.Number == regNumber);

            if (vehicle == null)
            {
                return false;
            }

            vehicle.IsDeleted = true;

            await dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// This method edits a vehicle
        /// </summary>
        /// <param name="model">The registration number of the vehicle</param>
        /// <returns>Void</returns>
        /// <exception cref="NullReferenceException">If the vehicle does not exists throws exception</exception>
        public async Task EditVehicleAsync(VehicleViewModelAdd model)
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


        public async Task<ICollection<VehicleViewModelShortInfo>> GetAllAsync()
        {
            return await dbContext.Vehicles
                .OrderBy(v => v.Number)
                .Select(v => new VehicleViewModelShortInfo
                {
                    Number = v.Number,
                    Make = null,
                    Model = null
                })
                .ToArrayAsync();
        }

        /// <summary>
        /// This method return Collection of all active vehicles Ordered by Number accending
        /// </summary>
        /// <returns>Collection of type VehicleViewModelShortInfo</returns>
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

        /// <summary>
        /// This method return a vehicle
        /// </summary>
        /// <param name="regNumber">The registration number of the vehicle</param>
        /// <returns>Vehicle of type VehicleViewModelDetails</returns>
        /// <exception cref="NullReferenceException">If a vehicle does not exists throws exception</exception>
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

        /// <summary>
        /// This method return a vehicle for Edit
        /// </summary>
        /// <param name="regNumber">The registration number of the vehicle</param>
        /// <returns>Vehicle of type VehicleViewModelAdd</returns>
        public async Task<VehicleViewModelAdd> GetVehicleByIdForEditAsync(string regNumber)
        {
            var vehicle = await dbContext
                .Vehicles
                .Include(v => v.Owner)
                .FirstOrDefaultAsync(v => v.Number == regNumber);

            if(vehicle == null)
            {
                throw new NullReferenceException(nameof(vehicle));
            }

            VehicleViewModelAdd model = new VehicleViewModelAdd()
            {
                Number = vehicle.Number,
                Model = vehicle.Model,
                Make = vehicle.Make,
                Mileage = vehicle.Mileage,
                FuelQuantity = vehicle.FuelQuantity,
                FuelCapacity = vehicle.FuelCapacity,
                FuelCostRate = vehicle.FuelCostRate,
                VIN = vehicle.VIN,
                OwnerId = vehicle.Owner.Id,
                ModelImgUrl = vehicle.ModelImgUrl
            };

            return model;
        }
    }
}
