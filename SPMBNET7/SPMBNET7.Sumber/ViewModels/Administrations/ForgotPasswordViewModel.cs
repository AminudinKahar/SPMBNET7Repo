﻿using System.ComponentModel.DataAnnotations;

namespace SPMBNET7.Sumber.ViewModels.Administrations
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Emel diperlukan")]
        [EmailAddress]
        public string Emel { get; set; } = string.Empty;
    }
}
