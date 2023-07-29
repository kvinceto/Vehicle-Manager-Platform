using Vmp.Web.ViewModels.AdminViewModels;

namespace Vmp.Services.Interfaces
{
    public interface IAdminService
    {
        Task<string> RestoreTaskByIdAsync(int taskId);

        Task<string> RestoreOwnerByIdAsync(int ownerId);

        Task<string> RestoreCostCenterByIdAsync(int costCenterId);

        Task<string> RestoreVehicleByIdAsync(string vehicleNumber);

        Task<string> RestoreDateCheckByIdAsync(int dateCheckId);

        Task<string> RestoreMileageCheckByIdAsync(int mileageCheckId);

        Task<ICollection<TaskRestoreViewModel>> GetAllTasksAsync();

        Task<ICollection<OwnerRestoreViewModel>> GetAllOwnersAsync();

        Task<ICollection<VehicleRestoreViewModel>> GetAllVehiclesAsync();

        Task<ICollection<CostCenterRestoreViewModel>> GetAllCostCenteresAsync();

        Task<ICollection<MileageCheckRestoreViewModel>> GetAllMileageChecksAsync();

        Task<ICollection<DateCheckRestoreViewModel>> GetAllDateChecksAsync();

        Task<ICollection<UserViewModel>> GetAllUsers();
    }
}
