namespace Vmp.Web.ViewModels.MileageCheckViewModels
{
    using System.Collections.Generic;

    using Vmp.Web.ViewModels.VehicleViewModels;

    public class MileageCheckViewModelAllDto
    {
        public MileageCheckViewModelAllDto()
        {
            this.Vehicles = new HashSet<VehicleViewModelShortInfo>();
            this.Checks = new HashSet<MileageCheckViewModelAll>();
        }

        public ICollection<VehicleViewModelShortInfo> Vehicles { get; set; }

        public ICollection<MileageCheckViewModelAll> Checks { get; set; }
    }
}
