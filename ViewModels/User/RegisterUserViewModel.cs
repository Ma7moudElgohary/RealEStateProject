using System.ComponentModel.DataAnnotations;
using RealEstate.Models;

namespace RealEStateProject.ViewModels.User
{
    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters", MinimumLength = 8)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
        public string? PhoneNumber { get; set; } 

        //[Required(ErrorMessage = "Address is required")]
        //[Display(Name = "Address")]
        //[StringLength(255, ErrorMessage = "Address cannot exceed 255 characters")]
        //public string Address { get; set; }

        [Display(Name = "User Type")]
        [Required(ErrorMessage = "Please select a user type")]
        public UserType? UserType { get; set; }
    }
}
