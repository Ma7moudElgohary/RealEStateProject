using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RealEStateProject.ViewModels.User
{
    public class UserProfileViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        public string? ProfilePicture { get; set; }

        [Display(Name = "Profile Image")]
        public IFormFile? ProfileImageFile { get; set; }

        // Agent-specific properties
        public bool IsAgent { get; set; }
        public int? AgentId { get; set; }

        [StringLength(20, ErrorMessage = "License number cannot exceed 20 characters")]
        [Display(Name = "License Number")]
        public string? LicenseNumber { get; set; }

        [StringLength(100, ErrorMessage = "Agency name cannot exceed 100 characters")]
        [Display(Name = "Agency Name")]
        public string? Agency { get; set; }

        [StringLength(1000, ErrorMessage = "Biography cannot exceed 1000 characters")]
        [Display(Name = "Professional Biography")]
        public string? Biography { get; set; }

        [Range(0, 100, ErrorMessage = "Experience must be between 0 and 100 years")]
        [Display(Name = "Years of Experience")]
        public int? YearsOfExperience { get; set; }
    }
}