using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SPMBNET7.CoreBusiness.Models.Administrations
{
    public class ApplicationUser : IdentityUser
    {
        public string Nama { get; set; } = string.Empty;
        [DisplayName("Tandatangan")]
        public string Tandatangan { get; set; } = string.Empty;
        [NotMapped]
        public string RoleId { get; set; } = string.Empty;
        [NotMapped]
        public string Role { get; set; } = string.Empty;
        [NotMapped]
        public List<string>? UserRoles { get; set; }
        [NotMapped]
        public List<IdentityUserRole<string>>? SelectedRoleList { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? RoleList { get; set; }

        //relationship
        public int? SuPekerjaId { get; set; }
        public SuPekerja? SuPekerja { get; set; }
        [DisplayName("Bahagian")] 
        public string JBahagianList { get; set; } = string.Empty;
        [NotMapped]
        public List<int>? SelectedJBahagianList { get; set; }
    }
}
