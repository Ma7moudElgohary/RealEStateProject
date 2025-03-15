using System.ComponentModel.DataAnnotations;
using RealEstate.Models;

namespace RealEStateProject.ViewModels.Property
{
    public class PropertyRequestViewModel
    {
        public int Id { get; set; }

        public PropertyViewModel Property { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }

        [Required]
        [StringLength(1000)]
        public string Message { get; set; }

        public RequestStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}