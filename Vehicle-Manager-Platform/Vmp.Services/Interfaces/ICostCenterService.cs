namespace Vmp.Services.Interfaces
{
    using Vmp.Web.ViewModels.CostCenterViewModels;
    using Vmp.Web.ViewModels.TaskViewModels;

    public interface ICostCenterService
    {
        Task<ICollection<CostCenterViewModelAll>> GetAllCostCentersAsync();

        Task AddNewCostCenterAsync(CostCenterViewModelAdd model);

        Task<CostCenterViewModelDetails> GetCostCenterByIdAsync(int costCenterId);

        Task<bool> DeleteCostCenterByIdAsync(int costCenterId);

        Task EditCostCenter(CostCenterViewModelEdit model);
    }
}
