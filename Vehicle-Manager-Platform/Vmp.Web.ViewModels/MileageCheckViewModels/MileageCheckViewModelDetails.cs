namespace Vmp.Web.ViewModels.MileageCheckViewModels
{
    public class MileageCheckViewModelDetails
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int ExpectedMileage { get; set; }

        public string VehicleNumber { get; set; } = null!;

        public bool IsCompleted { get; set; }

        public string User { get; set; } = null!;
    }
}
