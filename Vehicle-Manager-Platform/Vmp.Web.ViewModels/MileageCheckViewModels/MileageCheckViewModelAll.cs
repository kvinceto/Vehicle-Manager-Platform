using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vmp.Web.ViewModels.MileageCheckViewModels
{
    public class MileageCheckViewModelAll
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string VehicleNumber { get; set; } = null!;

        public string? UserId { get; set; }
    }
}
