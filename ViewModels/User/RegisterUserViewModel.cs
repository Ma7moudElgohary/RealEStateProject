using System.ComponentModel.DataAnnotations;

namespace RealEStateProject.ViewModels.User
{
    public class RegisterUserViewModel
    {
        [Display(Name = "First Name")]

        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string Address { get; set; }

    }
}
