using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml.Linq;
using SPMBNET7.CoreBusiness.Models.Modules._03_Sumber;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SPMBNET7.Sumber.ViewModels.Administrations
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Emel Diperlukan.")]
        [EmailAddress]
        [Display(Name = "Emel")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Katalaluan Diperlukan.")]
        [StringLength(100, ErrorMessage = "{0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Katalaluan")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Pengesahan Katalaluan")]
        [Compare("Password", ErrorMessage = "Katalaluan dan pengesahan katalaluan tidak sama")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Display(Name = "Nama Penuh")]
        public string Nama { get; set; } = string.Empty;

        public IEnumerable<SelectListItem> RoleList { get; set; } = new List<SelectListItem>();
        [Display(Name = "Peranan")]
        [Required(ErrorMessage = "Peranan Diperlukan.")]
        public string RoleSelected { get; set; } = string.Empty;
        [DisplayName("Anggota")]
        [Required(ErrorMessage = "Anggota Diperlukan.")]
        public int? SuPekerjaId { get; set; }
        public SuPekerja SuPekerja { get; set; } = new SuPekerja();
        [DisplayName("Bahagian")]
        public string JBahagianList { get; set; } = string.Empty;
        [Required(ErrorMessage = "Bahagian Diperlukan.")]
        public List<int> SelectedJBahagianList { get; set; } = new List<int> { 0 };
    }
}
