namespace Vmp.Web.ViewModels.WaybillViewModels
{
    public class WaybillViewModelDetails
    {

        public int Id { get; set; }

        public string Date { get; set; } = null!;

        public string VehicleNumber { get; set; } = null!;

        public int MileageStart { get; set; }

        public int MileageEnd { get; set; }

        public int MileageTraveled { get; set; }

        public string RouteTraveled { get; set; } = null!;

        public string FuelQuantityStart { get; set; } = null!;

        public string FuelQuantityEnd { get; set; } = null!;

        public string FuelConsumed { get; set; } = null!;

        public string FuelLoaded { get; set; } = null!;

        public string? Info { get; set; }

        public string DateCreated { get; set; } = null!;

        public string CostCenter { get; set; } = null!;

        public string Creator { get; set; } = null!;
    } 
}
