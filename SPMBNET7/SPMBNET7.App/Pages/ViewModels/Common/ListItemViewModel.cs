using System.ComponentModel.DataAnnotations.Schema;

namespace SPMBNET7.App.Pages.ViewModels.Common
{
    public class ListItemViewModel
    {
        [NotMapped]
        public int id { get; set; }
        [NotMapped]
        public int indek { get; set; }
        [NotMapped]
        public string perihal { get; set; }
        [NotMapped]
        public bool isGanda { get; set; }
    }
}
