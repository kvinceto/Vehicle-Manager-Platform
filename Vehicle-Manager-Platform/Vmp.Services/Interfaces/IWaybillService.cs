namespace Vmp.Services.Interfaces
{
    using Vmp.Web.ViewModels.WaybillViewModels;

    public interface IWaybillService
    {
        Task AddWaybill(WaybillViewModelAdd viewModel, string myId);

        Task<WaybillViewModelAdd> GetWaybillForAddingAsync(string regNumber);
    }
}
