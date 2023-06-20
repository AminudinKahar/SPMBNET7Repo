using System.ComponentModel;
using SPMBNET7.App.Pages.ViewModels.Common;

namespace SPMBNET7.App.Pages.ViewModels.Administrations
{
    public class ApplicationUserViewModel : EditSignViewModel
    {
        public string id { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        [DisplayName("Tandatangan")]
        public string Tandatangan { get; set; } = string.Empty;
    }
}
