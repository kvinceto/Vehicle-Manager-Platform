namespace Vmp.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using Vmp.Data;
    using Vmp.Data.Models;
    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.CostCenterViewModels;
    using Vmp.Web.ViewModels.WaybillViewModels;

    public class WaybillService : IWaybillService
    {
        private readonly VehicleManagerDbContext dbContext;

        public WaybillService(VehicleManagerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// This methos adds a waybill to the Database
        /// </summary>
        /// <param name="viewModel">View Model for the data</param>
        /// <param name="myId">User Id</param>
        /// <returns>Void</returns>
        public async Task AddWaybillAsync(WaybillViewModelAdd viewModel, string myId)
        {
            Vehicle? vehicle = await dbContext.Vehicles
                .FirstOrDefaultAsync(v => v.Number == viewModel.VehicleNumber);

            if (vehicle == null)
            {
                throw new NullReferenceException(nameof(vehicle));
            }

            Waybill waybill = new Waybill()
            {
                Date = DateTime.Parse(viewModel.Date),
                VehicleNumber = viewModel.VehicleNumber,
                MileageStart = viewModel.MileageStart,
                MileageEnd = viewModel.MileageEnd,
                MileageTraveled = viewModel.MileageTraveled,
                RouteTraveled = viewModel.RouteTraveled,
                FuelQuantityStart = vehicle.FuelQuantity,
                FuelQuantityEnd = (vehicle.FuelQuantity + viewModel.FuelLoaded - (vehicle.FuelCostRate * viewModel.MileageTraveled)),
                FuelConsumed = (vehicle.FuelCostRate * viewModel.MileageTraveled),
                FuelLoaded = viewModel.FuelLoaded,
                Info = viewModel.Info,
                DateCreated = DateTime.UtcNow.Date,
                CostCenterId = viewModel.CostCenterId,
                UserId = Guid.Parse(myId)
            };

            vehicle.FuelQuantity += viewModel.FuelLoaded;
            vehicle.FuelQuantity -= waybill.FuelConsumed;
            vehicle.Mileage += viewModel.MileageTraveled;

            await dbContext.Waybills.AddAsync(waybill);
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// This method edits waybill in the Database
        /// </summary>
        /// <param name="modelToEdit">View Model for the data</param>
        /// <param name="myId">User Id</param>
        /// <returns>Void</returns>
        public async Task EditWaybillAsync(WaybillViewModelEdit modelToEdit, string myId)
        {
            Waybill? waybillToEdit = await dbContext.Waybills
                .FirstOrDefaultAsync(w => w.Id == modelToEdit.Id);

            if (waybillToEdit == null)
            {
                throw new NullReferenceException(nameof(waybillToEdit));
            }

            Vehicle? vehicle = await dbContext.Vehicles
                .FirstOrDefaultAsync(v => v.Number == modelToEdit.VehicleNumber);

            if (vehicle == null)
            {
                throw new NullReferenceException(nameof(vehicle));
            }

            vehicle.FuelQuantity -= waybillToEdit.FuelLoaded;
            vehicle.FuelQuantity += waybillToEdit.FuelConsumed;
            vehicle.Mileage -= waybillToEdit.MileageTraveled;

            waybillToEdit.Date = DateTime.Parse(modelToEdit.NewDate);
            waybillToEdit.VehicleNumber = modelToEdit.VehicleNumber;
            waybillToEdit.MileageStart = modelToEdit.MileageStart;
            waybillToEdit.MileageEnd = modelToEdit.NewMileageEnd;
            waybillToEdit.MileageTraveled = modelToEdit.NewMileageTraveled;
            waybillToEdit.RouteTraveled = modelToEdit.NewRouteTraveled;
            waybillToEdit.FuelQuantityEnd = (vehicle.FuelQuantity + modelToEdit.NewFuelLoaded - (vehicle.FuelCostRate * modelToEdit.NewMileageTraveled));
            waybillToEdit.FuelConsumed = (vehicle.FuelCostRate * modelToEdit.NewMileageTraveled);
            waybillToEdit.FuelLoaded = modelToEdit.NewFuelLoaded;
            waybillToEdit.Info = modelToEdit.NewInfo;
            waybillToEdit.CostCenterId = modelToEdit.CostCenterId;
            waybillToEdit.UserId = Guid.Parse(myId);

            vehicle.FuelQuantity += waybillToEdit.FuelLoaded;
            vehicle.FuelQuantity -= waybillToEdit.FuelConsumed;
            vehicle.Mileage += waybillToEdit.MileageTraveled;

            await dbContext.SaveChangesAsync();

        }

        /// <summary>
        /// This method returns all waybills in the Database
        /// </summary>
        /// <returns>Collection of type WaybillViewModelAll</returns>
        public async Task<ICollection<WaybillViewModelAll>> GetAllAsync()
        {
            var result = await dbContext.Waybills
                .OrderBy(w => w.Date)
                .ThenBy(w => w.VehicleNumber)
                .AsNoTracking()
                .Select(w => new WaybillViewModelAll()
                {
                    Id = w.Id,
                    VehicleNumber = w.VehicleNumber,
                    Date = w.Date.ToString("dd/MM/yyyy"),
                    Info = w.Info
                })
                .ToArrayAsync();

            return result;

        }

        /// <summary>
        /// This method returns all waybills in the Database for cast center
        /// </summary>
        /// <param name="id">The Id of the cost center</param>
        /// <returns>Collection of type WaybillViewModelAll</returns>
        public async Task<ICollection<WaybillViewModelAll>> GetAllForCostCenterAsync(int id)
        {
            return await dbContext.Waybills
                 .AsNoTracking()
                 .Include(w => w.Vehicle)
                 .Where(w => w.CostCenterId == id)
                 .Select(w => new WaybillViewModelAll()
                 {
                     Id = w.Id,
                     Info = w.Info,
                     Date = w.Date.ToString("dd/MM/yyyy"),
                     VehicleNumber = w.VehicleNumber
                 })
                 .ToArrayAsync();
        }

        /// <summary>
        /// This method returns all waybills in the Database for a vehicle
        /// </summary>
        /// <param name="id">The Id of the vehicle</param>
        /// <returns>Collection of type WaybillViewModelAll</returns>
        public async Task<ICollection<WaybillViewModelAll>> GetAllForVehicleAsync(string regNumber)
        {
            return await dbContext.Waybills
                .Include(w => w.Vehicle)
                .Where(w => w.VehicleNumber == regNumber)
                .Select(w => new WaybillViewModelAll()
                {
                    Id = w.Id,
                    Date = w.Date.ToString("dd/MM/yyyy"),
                    Info = w.Info,
                    VehicleNumber = w.VehicleNumber.ToString()
                })
                .ToArrayAsync();
        }

        /// <summary>
        /// This method returns Collection of waybills for a vehicle for a period
        /// </summary>
        /// <param name="vehicleNumber">Registration Number of the vehicle</param>
        /// <param name="startDate">Start date of the period</param>
        /// <param name="endDate">End date of the period</param>
        /// <returns>Collection of type WaybillViewModelAll</returns>
        public async Task<ICollection<WaybillViewModelAll>> GetAllForVehicleForPeriod(string vehicleNumber, string startDate, string endDate)
        {
            DateTime start = DateTime.Parse(startDate);
            DateTime end = DateTime.Parse(endDate);

            return await dbContext.Waybills
                 .Include(w => w.Vehicle)
                 .Where(w => w.VehicleNumber == vehicleNumber && w.Date >= start && w.Date <= end)
                 .OrderByDescending(w => w.Date)
                 .Select(w => new WaybillViewModelAll()
                 {
                     Id = w.Id,
                     Date = w.Date.ToString("dd/MM/yyyy"),
                     Info = w.Info,
                     VehicleNumber = w.VehicleNumber
                 })
                 .ToArrayAsync();
        }

        /// <summary>
        /// This method returns short info for waybill
        /// </summary>
        /// <param name="id">The Id of the waybill</param>
        /// <returns>Waybill of type WaybillViewModelShort</returns>
        public async Task<WaybillViewModelShort> GetShortWaybillByIdAsync(int id)
        {
            var waybill = await dbContext.Waybills
                 .FirstOrDefaultAsync(w => w.Id == id);

            if (waybill == null)
            {
                throw new NullReferenceException(nameof(waybill));
            }

            return new WaybillViewModelShort()
            {
                Id = waybill.Id,
                UserId = waybill.UserId.ToString()
            };
        }

        /// <summary>
        /// This method returns data for waybill
        /// </summary>
        /// <param name="id">The Id of the waybill</param>
        /// <returns>Waybill of type WaybillViewModelDetails</returns>
        public async Task<WaybillViewModelDetails> GetWaybillByIdAsync(int id)
        {
            Waybill? waybill = await dbContext.Waybills
                .Include(w => w.CostCenter)
                .Include(w => w.User)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (waybill == null)
            {
                throw new NullReferenceException(nameof(waybill));
            }

            return new WaybillViewModelDetails()
            {
                Id = waybill.Id,
                Date = waybill.Date.ToString("dd/MM/yyyy"),
                Info = waybill.Info,
                DateCreated = waybill.Date.ToString("dd/MM/yyyy"),
                VehicleNumber = waybill.VehicleNumber,
                MileageStart = waybill.MileageStart,
                MileageEnd = waybill.MileageEnd,
                MileageTraveled = waybill.MileageTraveled,
                FuelQuantityStart = $"{waybill.FuelQuantityStart:f3}",
                FuelQuantityEnd = $"{waybill.FuelQuantityEnd:f3}",
                FuelConsumed = $"{waybill.FuelConsumed:f3}",
                FuelLoaded = $"{waybill.FuelLoaded:f3}",
                RouteTraveled = waybill.RouteTraveled,
                Creator = waybill.User.UserName,
                CostCenter = waybill.CostCenter.Name
            };
        }

        /// <summary>
        /// This method returns data for waybill
        /// </summary>
        /// <param name="regNumber">Vehicle registration number</param>
        /// <returns>Waybill of type WaybillViewModelAdd</returns>
        public async Task<WaybillViewModelAdd> GetWaybillForAddingAsync(string regNumber)
        {
            WaybillViewModelAdd model = new WaybillViewModelAdd();
            Vehicle? vehicle = await dbContext.Vehicles
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Number == regNumber);
            if (vehicle == null)
            {
                throw new NullReferenceException(nameof(vehicle));
            }

            model.VehicleNumber = vehicle.Number;
            model.FuelQuantityStart = vehicle.FuelQuantity;
            model.MileageStart = vehicle.Mileage;

            return model;
        }

        /// <summary>
        /// This methos return waybill for edit
        /// </summary>
        /// <param name="id">The Id of the Waybill</param>
        /// <returns>Waybill of type WaybillViewModelEdit</returns>
        public async Task<WaybillViewModelEdit> GetWaybillForEditByIdAsync(int id)
        {
            Waybill? waybill = await dbContext.Waybills
                .Include(w => w.CostCenter)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (waybill == null)
            {
                throw new NullReferenceException(nameof(waybill));
            }

            return new WaybillViewModelEdit()
            {
                Id = waybill.Id,
                VehicleNumber = waybill.VehicleNumber,
                OldDate = waybill.Date.ToString("dd/MM/yyyy"),
                OldCostCenterName = waybill.CostCenter.Name,
                OldInfo = waybill.Info,
                OldFuelLoaded = waybill.FuelLoaded,
                OldMileageEnd = waybill.MileageEnd,
                OldRouteTraveled = waybill.RouteTraveled,
                MileageStart = waybill.MileageStart,
                CostCenters = await dbContext.CostCenters
                                    .Where(c => c.IsClosed == false)
                                    .Select(c => new CostCenterViewModelAll()
                                    {
                                        Id = c.Id,
                                        Name = c.Name,
                                    })
                                    .ToArrayAsync()
            };
        }
    }
}
