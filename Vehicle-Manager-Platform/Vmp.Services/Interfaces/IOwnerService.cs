namespace Vmp.Services.Interfaces
{
    using Vmp.Web.ViewModels.OwnerViewModels;

    public interface IOwnerService
    {
        Task<ICollection<OwnerViewModelAll>> GetAllOwnersAsync();

        Task AddNewOwnerAsync(OwnerViewModelAdd ownerModel);
    }
}
