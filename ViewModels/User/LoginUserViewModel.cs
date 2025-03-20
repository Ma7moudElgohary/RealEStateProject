using System.ComponentModel.DataAnnotations;

namespace RealEStateProject.ViewModels.User
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[Required(ErrorMessage = "Email is required")]
        //[EmailAddress(ErrorMessage = "Invalid email format")]
        //public string Email { get; set; }

        public bool RemeberMe { get; set; }
    }
}
