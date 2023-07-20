namespace Vmp.Web.ViewModels.WaybillViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Vmp.Web.ViewModels.CostCenterViewModels;

    using static Vmp.Common.EntityValidationConstants.Waybill;

    public class WaybillViewModelEdit
    {
        public WaybillViewModelEdit()
        {
            this.CostCenters = new HashSet<CostCenterViewModelAll>();
        }

        public int? Id { get; set; }

        public string OldDate { get; set; } = null!;

        public string NewDate { get; set; } = null!;

        public string VehicleNumber { get; set; } = null!;

        public int MileageStart { get; set; }

        [Range(ExpectedMileageMinValue, ExpectedMileageMaxValue)]
        public int NewMileageEnd { get; set; }

        public int OldMileageEnd { get; set; }

        public int OldMileageTraveled => this.OldMileageEnd - this.MileageStart;

        public int NewMileageTraveled => this.NewMileageEnd - this.MileageStart;

        [Required(AllowEmptyStrings = false)]
        [StringLength(RouteTraveledMaxLength, MinimumLength = RouteTraveledMinLength)]
        public string NewRouteTraveled { get; set; } = null!;

        public string OldRouteTraveled { get; set; } = null!;

        public decimal? FuelQuantityStart { get; set; }

        public decimal? FuelQuantityEnd { get; set; }

        public decimal? FuelConsumed => this.FuelQuantityEnd - this.FuelQuantityStart;

        [Range(double.MinValue, double.MaxValue)]
        public decimal NewFuelLoaded { get; set; }

        public decimal OldFuelLoaded { get; set; }


        [MaxLength(InfoMaxLength)]
        public string? NewInfo { get; set; }

        public string? OldInfo { get; set; }

        [Range(1, int.MaxValue)]
        public int CostCenterId { get; set; }

        public string OldCostCenterName { get; set; } = null!;

        public ICollection<CostCenterViewModelAll> CostCenters { get; set; }
    }
}
