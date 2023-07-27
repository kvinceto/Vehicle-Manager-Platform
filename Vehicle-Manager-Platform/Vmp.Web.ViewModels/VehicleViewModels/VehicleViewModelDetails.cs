namespace Vmp.Web.ViewModels.VehicleViewModels
{
    public class VehicleViewModelDetails
    {
        public string RegistrationNumber { get; set; } = null!; 

        public string VIN { get; set; } = null!;

        public string? Model { get; set; }

        public string? Make { get; set; }

        public int Mileage { get; set; }

        public decimal FuelQuantity { get; set; }

        public decimal FuelCapacity { get; set; }

        public string Owner { get; set; } = null!;

        public decimal FuelCostRate { get; set; }

        public bool IsDeleted { get; set; }

        public int CountOfWaybills { get; set; }

        public string? ModelImgUrl { get; set; }

        public int CountOfMileageChecks { get; set; }

        public int CountOfDateChecks { get; set; }
    }
}
