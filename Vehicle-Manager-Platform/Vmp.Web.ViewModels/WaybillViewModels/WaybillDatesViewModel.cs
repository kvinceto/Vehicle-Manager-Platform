namespace Vmp.Web.ViewModels.WaybillViewModels
{
    using Vmp.Web.ViewModels.VehicleViewModels;

    public class WaybillDatesViewModel
    {
        public WaybillDatesViewModel()
        {
            this.Vehicles = new HashSet<VehicleViewModelShortInfo>();
        }
        public string StartDate { get; set; } = null!;

        public string EndDate { get; set; } = null!;

        public string VehicleNumber { get; set; } = null!;

        public ICollection<VehicleViewModelShortInfo> Vehicles { get; set; }
    }
}
