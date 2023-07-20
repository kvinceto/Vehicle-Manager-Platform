namespace Vmp.Services.Interfaces
{
    using System.Collections.Generic;

    using Vmp.Web.ViewModels.WaybillViewModels;

    public interface IWaybillService
    {
        Task AddWaybill(WaybillViewModelAdd viewModel, string myId);

        Task EditWaybillAsync(WaybillViewModelEdit modelToEdit, string myId);

        Task<ICollection<WaybillViewModelAll>> GetAll();

        Task<WaybillViewModelShort> GetShortWaybillByIdAsync(int id);

        Task<WaybillViewModelDetails> GetWaybillByIdAsync(int id);

        Task<WaybillViewModelAdd> GetWaybillForAddingAsync(string regNumber);

        Task<WaybillViewModelEdit> GetWaybillForEditByIdAsync(int id);
    }
}
