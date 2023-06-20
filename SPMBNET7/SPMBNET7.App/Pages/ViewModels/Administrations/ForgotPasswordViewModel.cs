using System.ComponentModel.DataAnnotations;

namespace SPMBNET7.App.Pages.ViewModels.Administrations
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Emel diperlukan")]
        [EmailAddress]
        public string Emel { get; set; } = string.Empty;
    }
}
