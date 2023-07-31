namespace Vmp.Services.Interfaces
{
    using System.Collections.Generic;

    using Vmp.Web.ViewModels.WaybillViewModels;

    public interface IWaybillService
    {
        Task AddWaybillAsync(WaybillViewModelAdd viewModel, string myId);

        Task EditWaybillAsync(WaybillViewModelEdit modelToEdit, string myId);

        Task<ICollection<WaybillViewModelAll>> GetAllAsync();

        Task<ICollection<WaybillViewModelAll>> GetAllForCostCenterAsync(int id);

        Task<ICollection<WaybillViewModelAll>> GetAllForVehicleAsync(string regNumber);

        Task<ICollection<WaybillViewModelAll>> GetAllForVehicleForPeriod(string vehicleNumber, string startDate, string endDate);

        Task<WaybillViewModelShort> GetShortWaybillByIdAsync(int id);

        Task<WaybillViewModelDetails> GetWaybillByIdAsync(int id);

        Task<WaybillViewModelAdd> GetWaybillForAddingAsync(string regNumber);

        Task<WaybillViewModelEdit> GetWaybillForEditByIdAsync(int id);
    }
}
