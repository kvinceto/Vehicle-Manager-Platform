namespace Vmp.Services.Interfaces
{
    using System.Collections.Generic;

    using Vmp.Web.ViewModels.MileageCheckViewModels;


    public interface IMileageCheckService
    {
        Task AddAsync(MileageCheckViewModelAdd modelAdd, string myId);

        Task<string> CompleteCheckByIdAsync(int id, string myId);

        Task EditAsync(MileageCheckViewModelEdit viewModel);

        Task<ICollection<MileageCheckViewModelAll>> GetAllActiveAsync();

        Task<ICollection<MileageCheckViewModelAll>> GetAllForVehicleAsync(string vehicleNumber);

        Task<ICollection<MileageCheckViewModelAll>> GetAllMineAsync(string myId);

        Task<MileageCheckViewModelDetails> GetChechByIdAsync(int id);

        Task<MileageCheckViewModelEdit> GetCheckByIdForEditAsync(int id);
    }
}
