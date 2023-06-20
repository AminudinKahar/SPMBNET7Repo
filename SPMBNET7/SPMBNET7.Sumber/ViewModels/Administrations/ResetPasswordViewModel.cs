using System.ComponentModel.DataAnnotations;

namespace SPMBNET7.Sumber.ViewModels.Administrations
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Emel Diperlukan.")]
        [EmailAddress]
        [Display(Name = "Emel")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Katalaluan Diperlukan.")]
        [StringLength(100, ErrorMessage = "Minimum {2} aksara diperlukan untuk {0}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Katalaluan")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Pengesahan Katalaluan")]
        [Required(ErrorMessage = "Pengesahan Katalaluan Diperlukan.")]
        [Compare("Password", ErrorMessage = "Katalaluan dan pengesahan katalaluan tidak sama")]
        public string ConfirmedPassword { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;
    }
}
