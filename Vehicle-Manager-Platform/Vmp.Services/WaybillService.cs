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
    using Vmp.Web.ViewModels.WaybillViewModels;

    public class WaybillService : IWaybillService
    {
        private readonly VehicleManagerDbContext dbContext;

        public WaybillService(VehicleManagerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddWaybill(WaybillViewModelAdd viewModel, string myId)
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
                DateCreated = DateTime.UtcNow,
                CostCenterId = viewModel.CostCenterId,
                UserId = Guid.Parse(myId)
            };

            vehicle.FuelQuantity += viewModel.FuelLoaded;
            vehicle.FuelQuantity -= waybill.FuelConsumed;
            vehicle.Mileage += viewModel.MileageTraveled;

            await dbContext.Waybills.AddAsync(waybill);
            await dbContext.SaveChangesAsync();
        }

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
    }
}
