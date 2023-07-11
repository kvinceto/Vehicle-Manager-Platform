namespace Vmp.Web.ViewModels.OwnerViewModels
{
    public class OwnerViewModelInfo
    {
        public int Id { get; set; }
       
        public string Name { get; set; } = null!;

        public string? Info { get; set; }

        public int VehiclesCount
        {
            get
            {
                return Vehicles.Count;
            }
        }
          

        public ICollection<string> Vehicles { get; set; } = new List<string>();
    }
}
