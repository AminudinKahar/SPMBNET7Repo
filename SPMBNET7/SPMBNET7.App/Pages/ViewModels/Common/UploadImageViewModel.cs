using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SPMBNET7.App.Pages.ViewModels.Common
{
    public class UploadImageViewModel
    {
        [Display(Name = "Logo")]
        public IFormFile? Gambar { get; set; }
    }
}