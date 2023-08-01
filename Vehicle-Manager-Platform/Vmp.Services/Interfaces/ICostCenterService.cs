namespace Vmp.Services.Interfaces
{
    using Vmp.Web.ViewModels.CostCenterViewModels;

    public interface ICostCenterService
    {
        Task<ICollection<CostCenterViewModelAll>> GetAllCostCentersAsync();

        Task AddNewCostCenterAsync(CostCenterViewModelAdd model);

        Task<CostCenterViewModelDetails> GetCostCenterByIdAsync(int costCenterId);

        Task<bool> DeleteCostCenterByIdAsync(int costCenterId);

        Task EditCostCenterAsync(CostCenterViewModelEdit model);
    }
}
