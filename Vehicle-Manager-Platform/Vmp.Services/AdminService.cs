namespace Vmp.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Vmp.Data;
    using Vmp.Data.Models;
    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.AdminViewModels;

    public class AdminService : IAdminService
    {
        private readonly VehicleManagerDbContext dbContext;

        public AdminService(VehicleManagerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<CostCenterRestoreViewModel>> GetAllCostCenteresAsync()
        {
            return await dbContext.CostCenters
                 .Where(c => c.IsClosed == true)
                 .Select(c => new CostCenterRestoreViewModel()
                 {
                     Id = c.Id,
                     Name = c.Name
                 })
                 .ToArrayAsync();
        }

        public async Task<ICollection<DateCheckRestoreViewModel>> GetAllDateChecksAsync()
        {
            return await dbContext.DateChecks
                .Include(d => d.Vehicle)
                .Where(d => d.IsCompleted == true)
                .Select(d => new DateCheckRestoreViewModel()
                {
                    Id = d.Id,
                    Name = d.Name,
                    VehicleNumber = d.VehicleNumber
                })
                .ToArrayAsync();
        }

        public async Task<ICollection<MileageCheckRestoreViewModel>> GetAllMileageChecksAsync()
        {
            return await dbContext.MileageChecks
                .Include(m => m.Vehicle)
                .Where(m => m.IsCompleted == true)
                .Select(m => new MileageCheckRestoreViewModel()
                {
                    Id = m.Id,
                    Name = m.Name,
                    VehicleNumber = m.VehicleNumber
                })
                .ToArrayAsync();
        }

        public async Task<ICollection<OwnerRestoreViewModel>> GetAllOwnersAsync()
        {
            return await dbContext.Owners
                   .Where(o => o.IsInactive == true)
                   .Select(o => new OwnerRestoreViewModel()
                   {
                       Id = o.Id,
                       Name = o.Name
                   })
                   .ToArrayAsync();
        }

        public async Task<ICollection<TaskRestoreViewModel>> GetAllTasksAsync()
        {
            return await dbContext.Tasks
                .Where(t => t.IsCompleted == true)
                .Select(t => new TaskRestoreViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    EndDate = t.EndDate.ToString("dd/MM/yyyy")
                })
                .ToArrayAsync();
        }

        public async Task<ICollection<UserViewModel>> GetAllUsers()
        {
            return await dbContext.AspNetUsers
                .Where(u => u.UserName != null)
                .Select(u => new UserViewModel()
                {
                    Id = u.Id.ToString(),
                    UserName = u.UserName
                })
                .ToListAsync();
        }

        public async Task<ICollection<VehicleRestoreViewModel>> GetAllVehiclesAsync()
        {
            return await dbContext.Vehicles
                .Where(v => v.IsDeleted == true)
                .Select(v => new VehicleRestoreViewModel()
                {
                    RegistrationNumber = v.Number
                })
                .ToArrayAsync();
        }

        public async Task<string> RestoreCostCenterByIdAsync(int costCenterId)
        {
            CostCenter? costCenter = await dbContext.CostCenters
                .FirstOrDefaultAsync(cc => cc.Id == costCenterId);

            if (costCenter == null)
            {
                return $"Cost Center with id: {costCenterId} not found!";
            }

            costCenter.IsClosed = false;
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

            dateCheck.IsCompleted = false;
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

            mileageCheck.IsCompleted = false;
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

            owner.IsInactive = false;
            await dbContext.SaveChangesAsync();
            return $"Owner with id: {ownerId} restored!";
        }

        public async Task<string> RestoreTaskByIdAsync(int taskId)
        {
            TaskModel? task = await dbContext.Tasks
                  .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
            {
                return $"Task with id: {taskId} not found!";
            }

            task.IsCompleted = false;
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

            vehicle.IsDeleted = false;
            await dbContext.SaveChangesAsync();
            return $"Vehicle with registration number: {vehicleNumber} restored!";
        }
    }
}
