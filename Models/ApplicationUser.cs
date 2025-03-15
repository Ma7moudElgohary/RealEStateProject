using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Key]
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public ICollection<Property>? Properties { get; set; } = new List<Property>();

        public ICollection<Favorite>? Favorites { get; set; } = new List<Favorite>();

        public ICollection<PropertyRequest>? Requests { get; set; } = new List<PropertyRequest>();
    }
}