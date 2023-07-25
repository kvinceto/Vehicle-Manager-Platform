namespace Vmp.Web.ViewModels.DateCheckViewModels
{
    using System.Collections.Generic;

    using Vmp.Web.ViewModels.VehicleViewModels;

    public class DateCheckViewModelAllDto
    {
        public DateCheckViewModelAllDto()
        {
            this.Vehicles = new HashSet<VehicleViewModelShortInfo>();
            this.Checks = new HashSet<DateCheckViewModelAll>();
        }

        public ICollection<VehicleViewModelShortInfo> Vehicles { get; set; }

        public ICollection<DateCheckViewModelAll> Checks { get; set; }
    }
}
