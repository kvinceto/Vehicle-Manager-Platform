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

        /// <summary>
        /// This method creates new mileage check in the Database
        /// </summary>
        /// <param name="modelAdd">View Model for the data</param>
        /// <param name="myId">User Id</param>
        /// <returns>Void</returns>
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

        /// <summary>
        /// This method marks mileage check as completed
        /// </summary>
        /// <param name="id">The Id for the mileage check</param>
        /// <param name="myId">User Id</param>
        /// <returns>string</returns>
        public async Task<string> CompleteCheckByIdAsync(int id, string myId)
        {
            MileageCheck? mileageCheck = await dbContext.MileageChecks
                  .FirstOrDefaultAsync(mc => mc.Id == id);

            if (mileageCheck == null)
            {
                return "not changed";
            }

            if (mileageCheck.UserId.ToString() != myId)
            {
                return "not me";
            }

            mileageCheck.IsCompleted = true;

            await dbContext.SaveChangesAsync();

            return "changed";

        }

        /// <summary>
        /// This method edits a mileage check data
        /// </summary>
        /// <param name="viewModel">View Model for the data</param>
        /// <returns>Void</returns>
        /// <exception cref="NullReferenceException">If not found throws exception</exception>
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

        /// <summary>
        /// This methos returns all active mileage checks
        /// </summary>
        /// <returns>Collection of type MileageCheckViewModelAll</returns>
        public async Task<ICollection<MileageCheckViewModelAll>> GetAllActiveAsync()
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

        /// <summary>
        /// This methos returns all active mileage checks for vehicle
        /// </summary>
        /// <param name="vehicleNumber">The vehicle registration number</param>
        /// <returns>Collection of type MileageChecViewModelAll</returns>
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

        /// <summary>
        /// This methos returns all active mileage checks for user
        /// </summary>
        /// <param name="vehicleNumber">The vehicle registration number</param>
        /// <returns>Collection of type MileageChecViewModelAll</returns>
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

        /// <summary>
        /// This method returns mileage check data
        /// </summary>
        /// <param name="id">The Id of the mileage check</param>
        /// <returns>Check of type MileageCheckViewModelDetails</returns>
        /// <exception cref="NullReferenceException">If not found throws exception</exception>
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

        /// <summary>
        /// This methos returns mileage check for Edit
        /// </summary>
        /// <param name="id">The Id of the mileage check</param>
        /// <returns>Check of type MileageCheckViewModelEdit</returns>
        /// <exception cref="NullReferenceException">If not found throws exception</exception>
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
