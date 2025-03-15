using System.ComponentModel.DataAnnotations;

namespace RealEStateProject.ViewModels.Common
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}