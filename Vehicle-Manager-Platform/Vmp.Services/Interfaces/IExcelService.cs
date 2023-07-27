namespace Vmp.Services.Interfaces
{
    public interface IExcelService
    {
        Task<byte[]> GenerateExcelFileWaybillAsync(int waybillId);

        Task<byte[]> GenerateExcelFileVehicleAsync(string regNumber);
    }
}
