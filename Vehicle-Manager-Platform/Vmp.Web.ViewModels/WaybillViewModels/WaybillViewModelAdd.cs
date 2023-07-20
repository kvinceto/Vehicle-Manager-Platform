namespace Vmp.Web.ViewModels.WaybillViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Vmp.Web.ViewModels.CostCenterViewModels;
    using Vmp.Web.ViewModels.VehicleViewModels;

    using static Vmp.Common.EntityValidationConstants.Waybill;

    public class WaybillViewModelAdd
    {
        public WaybillViewModelAdd()
        {
            this.CostCenters = new HashSet<CostCenterViewModelAll>();           
        }

        public string Date { get; set; } = null!;

        public string VehicleNumber { get; set; } = null!;

        public int MileageStart { get; set; }

        [Range(ExpectedMileageMinValue, ExpectedMileageMaxValue)]
        public int MileageEnd { get; set; }

        public int MileageTraveled => this.MileageEnd - this.MileageStart;

        [Required(AllowEmptyStrings = false)]
        [StringLength(RouteTraveledMaxLength, MinimumLength = RouteTraveledMinLength)]
        public string RouteTraveled { get; set; } = null!;

        public decimal? FuelQuantityStart { get; set; }
        
        public decimal? FuelQuantityEnd { get; set; }

        public decimal? FuelConsumed => this.FuelQuantityEnd - this.FuelQuantityStart;

        [Range(double.MinValue, double.MaxValue)]
        public decimal FuelLoaded { get; set; }


        [MaxLength(InfoMaxLength)]
        public string? Info { get; set; }

        [Range(1, int.MaxValue)]
        public int CostCenterId { get; set; }

        public ICollection<CostCenterViewModelAll> CostCenters { get; set; }
    }
}
