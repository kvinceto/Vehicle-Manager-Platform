namespace Vmp.Services.Interfaces
{
    using Vmp.Web.ViewModels.CostCenterViewModels;

    public interface ICostCenterService
    {
        Task<ICollection<CostCenterViewModelAll>> GetAllCostCentersAsync();

        Task AddNewCostCenter(CostCenterViewModelAdd model);
    }
}
