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
    using Vmp.Web.ViewModels.MileageCheckViewModels;
    using Vmp.Web.ViewModels.VehicleViewModels;

    public class MileageCheckService : IMileageCheckService
    {
        private readonly VehicleManagerDbContext dbContext;

        public MileageCheckService(VehicleManagerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(MileageCheckViewModelAdd modelAdd, string myId)
        {
            MileageCheck mileageCheck = new MileageCheck()
            {
                Name = modelAdd.Name,
                VehicleNumber = modelAdd.VehicleNumber,
                ExpectedMileage = modelAdd.ExpectedMileage,
                IsCompleted = false,
                UserId = Guid.Parse(myId)
            };

            await dbContext.MileageChecks.AddAsync(mileageCheck);
            await dbContext.SaveChangesAsync();
        }

        public async Task<string> CompleteCheckByIdAsync(int id, string myId)
        {
            MileageCheck? mileageCheck = await dbContext.MileageChecks
                  .FirstOrDefaultAsync(mc => mc.Id == id);

            if(mileageCheck == null)
            {
                return "not changed";
            }

            if(mileageCheck.UserId.ToString() !=  myId)
            {
                return "not me";
            }

            mileageCheck.IsCompleted = true;

            await dbContext.SaveChangesAsync();

            return "changed";

        }

        public async Task EditAsync(MileageCheckViewModelEdit viewModel)
        {
            MileageCheck? mileageCheck = await dbContext.MileageChecks
                .FirstOrDefaultAsync(mc => mc.Id == viewModel.Id!.Value);

            if (mileageCheck == null)
            {
                throw new NullReferenceException(nameof(mileageCheck));
            }

            mileageCheck.Name = viewModel.Name;
            mileageCheck.ExpectedMileage = viewModel.ExpectedMileage;
            mileageCheck.VehicleNumber = viewModel.VehicleNumber;

            await dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<MileageCheckViewModelAll>> GetAllAsync()
        {
            return await dbContext.MileageChecks
                .AsNoTracking()
                .Where(mc => mc.IsCompleted == false)
                .Select(mc => new MileageCheckViewModelAll()
                {
                    Id = mc.Id,
                    Name = mc.Name,
                    VehicleNumber = mc.VehicleNumber,
                    UserId = mc.UserId.ToString()
                })
                .ToArrayAsync();
        }

        public async Task<ICollection<MileageCheckViewModelAll>> GetAllForVehicleAsync(string vehicleNumber)
        {
            return await dbContext.MileageChecks
               .AsNoTracking()
               .Where(mc => mc.IsCompleted == false && mc.VehicleNumber == vehicleNumber)
               .Select(mc => new MileageCheckViewModelAll()
               {
                   Id = mc.Id,
                   Name = mc.Name,
                   VehicleNumber = mc.VehicleNumber,
                   UserId = mc.UserId.ToString()
               })
               .ToArrayAsync();
        }

        public async Task<ICollection<MileageCheckViewModelAll>> GetAllMineAsync(string myId)
        {

            return await dbContext.MileageChecks
               .AsNoTracking()
               .Where(mc => mc.IsCompleted == false && mc.UserId.ToString() == myId)
               .Select(mc => new MileageCheckViewModelAll()
               {
                   Id = mc.Id,
                   Name = mc.Name,
                   VehicleNumber = mc.VehicleNumber,
                   UserId = mc.UserId.ToString()
               })
               .ToArrayAsync();
        }

        public async Task<MileageCheckViewModelDetails> GetChechByIdAsync(int id)
        {
            MileageCheck? mileageCheck = await dbContext.MileageChecks
                 .Include(mc => mc.User)
                 .FirstOrDefaultAsync(mc => mc.Id == id);

            if (mileageCheck == null)
            {
                throw new NullReferenceException(nameof(mileageCheck));
            }

            return new MileageCheckViewModelDetails()
            {
                Id = mileageCheck.Id,
                Name = mileageCheck.Name,
                IsCompleted = mileageCheck.IsCompleted,
                ExpectedMileage = mileageCheck.ExpectedMileage,
                User = mileageCheck.User.UserName,
                VehicleNumber = mileageCheck.VehicleNumber
            };
        }

        public async Task<MileageCheckViewModelEdit> GetCheckByIdForEditAsync(int id)
        {
            MileageCheck? mileageCheck = await dbContext.MileageChecks
                 .FirstOrDefaultAsync(mc => mc.Id == id);

            if (mileageCheck == null)
            {
                throw new NullReferenceException(nameof(mileageCheck));
            }

            MileageCheckViewModelEdit result = new MileageCheckViewModelEdit()
            {
                Id = mileageCheck.Id,
                Name = mileageCheck.Name,
                ExpectedMileage = mileageCheck.ExpectedMileage,
                UserId = mileageCheck.UserId.ToString(),
                VehicleNumber = mileageCheck.VehicleNumber,
                Vehicles = await dbContext.Vehicles
                                .Where(v => v.IsDeleted == false)
                                .Select(v => new VehicleViewModelShortInfo()
                                {
                                    Number = v.Number,
                                    Make = v.Make,
                                    Model = v.Model
                                }).ToArrayAsync()
            };

            return result;

        }
    }
}
