namespace Vmp.Web.ViewModels.CostCenterViewModels
{
    public class CostCenterViewModelDetails
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public bool IsClosed { get; set; }

        public int WaybillsCount { get; set; }
    }
}
