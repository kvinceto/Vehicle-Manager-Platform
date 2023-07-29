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
    }
}
