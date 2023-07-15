﻿namespace Vmp.Services.Interfaces
{
    using Vmp.Web.ViewModels.VehicleViewModels;

    public interface IVehicleService
    {
        Task<ICollection<VehicleViewModelShortInfo>> GetAllVehiclesAsync();

        Task AddNewVehicleAsync(VehicleViewModelAdd model);

        Task<VehicleViewModelDetails> GetVehicleByIdAsync(string regNumber);
    }
}