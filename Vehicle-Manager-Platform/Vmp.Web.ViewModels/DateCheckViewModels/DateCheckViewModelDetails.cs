namespace Vmp.Web.ViewModels.DateCheckViewModels
{
    public class DateCheckViewModelDetails
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string EndDate { get; set; } = null!;

        public string VehicleNumber { get; set; } = null!;

        public bool IsCompleted { get; set; }

        public string User { get; set; } = null!;
    }
}
