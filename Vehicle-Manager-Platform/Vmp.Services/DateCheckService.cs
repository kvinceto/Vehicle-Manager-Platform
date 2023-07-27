namespace Vmp.Services
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using Vmp.Data;
    using Vmp.Data.Models;
    using Vmp.Services.Interfaces;
    using Vmp.Web.ViewModels.DateCheckViewModels;

    public class DateCheckService : IDateCheckService
    {
        private readonly VehicleManagerDbContext dbContext;

        public DateCheckService(VehicleManagerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(DateCheckViewModelAdd viewModel, string? myId)
        {
            DateCheck dateCheck = new DateCheck()
            {
                Name = viewModel.Name,
                EndDate = DateTime.Parse(viewModel.EndDate),
                IsCompleted = false,
                VehicleNumber = viewModel.VehicleNumber,
                UserId = Guid.Parse(myId!)
            };

            await dbContext.DateChecks.AddAsync(dateCheck);
            await dbContext.SaveChangesAsync();
        }

        public async Task<string> CompleteCheckByIdAsync(int id, string myId)
        {
            DateCheck? dateCheck = await dbContext.DateChecks
                  .FirstOrDefaultAsync(dc => dc.Id == id);

            if (dateCheck == null)
            {
                return "not changed";
            }

            if (dateCheck.UserId.ToString() != myId)
            {
                return "not me";
            }

            dateCheck.IsCompleted = true;

            await dbContext.SaveChangesAsync();

            return "changed";

        }

        public async Task EditAsync(DateCheckViewModelEdit viewModel)
        {
            DateCheck? dateCheck = await dbContext.DateChecks
                .FirstOrDefaultAsync(dc => dc.Id == viewModel.Id);

            if (dateCheck == null)
            {
                throw new NullReferenceException(nameof(dateCheck));
            }

            dateCheck!.EndDate = DateTime.Parse(viewModel.EndDate);
            dateCheck.VehicleNumber = viewModel.VehicleNumber;
            dateCheck.UserId = Guid.Parse(viewModel.UserId!);
            dateCheck.Name = viewModel.Name;

            await dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<DateCheckViewModelAll>> GetAllDateChecksAsync()
        {
            var result = await dbContext.DateChecks
                .Where(dc => dc.IsCompleted == false)
                .OrderBy(dc => dc.EndDate)
                .Select(dc => new DateCheckViewModelAll()
                {
                    Id = dc.Id,
                    Name = dc.Name,
                    EndDate = dc.EndDate.ToString("dd/MM/yyyy"),
                    UserId = dc.UserId.ToString(),
                    VehicleNumber = dc.VehicleNumber
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<ICollection<DateCheckViewModelAll>> GetAllForVehicleAsync(string vehicleNumber)
        {
            return await dbContext.DateChecks
              .AsNoTracking()
              .Where(dc => dc.IsCompleted == false && dc.VehicleNumber == vehicleNumber)
              .Select(dc => new DateCheckViewModelAll()
              {
                  Id = dc.Id,
                  Name = dc.Name,
                  VehicleNumber = dc.VehicleNumber,
                  EndDate = dc.EndDate.ToString("dd/MM/yyyy"),
                  UserId = dc.UserId.ToString()
              })
              .ToArrayAsync();
        }

        public async Task<ICollection<DateCheckViewModelAll>> GetAllMineAsync(string myId)
        {

            return await dbContext.DateChecks
               .AsNoTracking()
               .Where(dc => dc.IsCompleted == false && dc.UserId.ToString() == myId)
               .Select(dc => new DateCheckViewModelAll()
               {
                   Id = dc.Id,
                   Name = dc.Name,
                   VehicleNumber = dc.VehicleNumber,
                   UserId = dc.UserId.ToString(),
                   EndDate = dc.EndDate.ToString("dd/MM/yyyy")
               })
               .ToArrayAsync();
        }

        public async Task<DateCheckViewModelEdit> GetCheckByIdForEditAsync(int id)
        {
            var dateCheck = await dbContext.DateChecks
                 .Include(dc => dc.User)
                 .Include(dc => dc.Vehicle)
                 .FirstOrDefaultAsync(dc => dc.Id == id);

            if (dateCheck == null)
            {
                throw new NullReferenceException(nameof(dateCheck));
            }

            var result = new DateCheckViewModelEdit()
            {
                Id = dateCheck.Id,
                EndDate = dateCheck.EndDate.ToString("dd/MM/yyyy"),
                Name = dateCheck.Name,
                UserId = dateCheck.UserId.ToString(),
                VehicleNumber = dateCheck.VehicleNumber
            };

            return result;
        }

        public async Task<DateCheckViewModelDetails> GetDateCheckByIdAsync(int id)
        {
            DateCheck? dateCheck = await dbContext.DateChecks
                .Include(dc => dc.User)
                .Include(dc => dc.Vehicle)
                .FirstOrDefaultAsync(dc => dc.Id == id);

            if (dateCheck == null)
            {
                throw new NullReferenceException(nameof(dateCheck));
            }

            DateCheckViewModelDetails model = new DateCheckViewModelDetails()
            {
                Id = dateCheck.Id,
                Name = dateCheck.Name,
                EndDate = dateCheck.EndDate.ToString("dd/MM/yyyy"),
                IsCompleted = dateCheck.IsCompleted,
                VehicleNumber = dateCheck.VehicleNumber,
                User = dateCheck.User.UserName
            };

            return model;
        }
    }
}
