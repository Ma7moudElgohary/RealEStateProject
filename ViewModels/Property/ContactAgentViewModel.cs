using System.ComponentModel.DataAnnotations;

namespace RealEStateProject.ViewModels.Property
{
    public class ContactAgentViewModel
    {
        public int PropertyId { get; set; }

        public string? AgentId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string ContactEmail { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string ContactPhone { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(1000, ErrorMessage = "Message cannot exceed 1000 characters")]
        public string ContactMessage { get; set; }
    }
}
