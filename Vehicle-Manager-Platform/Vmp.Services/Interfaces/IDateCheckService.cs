namespace Vmp.Services.Interfaces
{
    using System.Collections.Generic;

    using Vmp.Web.ViewModels.DateCheckViewModels;

    public interface IDateCheckService
    {
        Task AddAsync(DateCheckViewModelAdd viewModel, string? myId);

        Task<string> CompleteCheckByIdAsync(int id, string myId);

        Task EditAsync(DateCheckViewModelEdit viewModel);

        Task<ICollection<DateCheckViewModelAll>> GetAllDateChecksAsync();

        Task<ICollection<DateCheckViewModelAll>> GetAllForVehicleAsync(string vehicleNumber);

        Task<ICollection<DateCheckViewModelAll>> GetAllMineAsync(string myId);

        Task<DateCheckViewModelEdit> GetCheckByIdForEditAsync(int id);

        Task<DateCheckViewModelDetails> GetDateCheckByIdAsync(int id);
    }
}
