namespace Vmp.Services
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Vmp.Data;
    using Vmp.Data.Models;
    using Vmp.Services.Interfaces;

    public class AdminService : IAdminService
    {
        private readonly VehicleManagerDbContext dbContext;

        public AdminService(VehicleManagerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<string> RestoreCostCenterByIdAsync(int costCenterId)
        {
            CostCenter? costCenter = await dbContext.CostCenters
                .FirstOrDefaultAsync(cc => cc.Id == costCenterId);

            if(costCenter == null )
            {
                return $"Cost Center with id: {costCenterId} not found!";
            }

            costCenter.IsClosed = true;
            await dbContext.SaveChangesAsync();
            return $"Cost Center with id: {costCenterId} restored!";
        }

        public async Task<string> RestoreDateCheckByIdAsync(int dateCheckId)
        {
            DateCheck? dateCheck = await dbContext.DateChecks
                .FirstOrDefaultAsync(dc => dc.Id == dateCheckId);

            if (dateCheck == null)
            {
                return $"Date Check with id: {dateCheckId} not found!";
            }

            dateCheck.IsCompleted = true;
            await dbContext.SaveChangesAsync();
            return $"Date Check with id: {dateCheckId} restored!";
        }

        public async Task<string> RestoreMileageCheckByIdAsync(int mileageCheckId)
        {
            MileageCheck? mileageCheck = await dbContext.MileageChecks
                 .FirstOrDefaultAsync(mc => mc.Id == mileageCheckId);

            if (mileageCheck == null)
            {
                return $"Mileage Check with id: {mileageCheckId} not found!";
            }

            mileageCheck.IsCompleted = true;
            await dbContext.SaveChangesAsync();
            return $"Mileage Check with id: {mileageCheckId} restored!";
        }

        public async Task<string> RestoreOwnerByIdAsync(int ownerId)
        {
            Owner? owner = await dbContext.Owners
                 .FirstOrDefaultAsync(o => o.Id == ownerId);

            if (owner == null)
            {
                return $"Owner with id: {ownerId} not found!";
            }

            owner.IsInactive = true;
            await dbContext.SaveChangesAsync();
            return $"Owner with id: {ownerId} restored!";
        }

        public async Task<string> RestoreTaskByIdAsync(int taskId)
        {
            TaskModel? task = await dbContext.Tasks
                  .FirstOrDefaultAsync(t => t.Id == taskId);

            if ( task == null)
            {
                return $"Task with id: {taskId} not found!";
            }

            task.IsCompleted = true;
            await dbContext.SaveChangesAsync();
            return $"Task with id: {taskId} restored!";
        }

        public async Task<string> RestoreVehicleByIdAsync(string vehicleNumber)
        {
            Vehicle? vehicle = await dbContext.Vehicles
                   .FirstOrDefaultAsync(v => v.Number == vehicleNumber);

            if (vehicle == null)
            {
                return $"Vehicle with registration number: {vehicleNumber} not found!";
            }

            vehicle.IsDeleted = true;
            await dbContext.SaveChangesAsync();
            return $"Vehicle with registration number: {vehicleNumber} restored!";
        }
    }
}
