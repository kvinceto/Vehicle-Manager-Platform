namespace Vmp.Services.Interfaces
{
    using Vmp.Web.ViewModels.OwnerViewModels;

    public interface IOwnerService
    {
        Task<ICollection<OwnerViewModelAll>> GetAllOwnersAsync();

        Task AddNewOwnerAsync(OwnerViewModelAdd ownerModel);

        Task<OwnerViewModelInfo> GetOwnerByIdAsync(int ownerId);

        Task<OwnerViewModelEdit> GetOwnerByIdForEditAsync(int ownerId);

        Task<bool> DeleteOwnerByIdAsync(int ownerId);

        Task EditOwner(OwnerViewModelEdit ownerModel);
    }
}
