using System.ComponentModel.DataAnnotations;

namespace RealEStateProject.ViewModels.Login_Register
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
