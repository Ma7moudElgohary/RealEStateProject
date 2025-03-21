using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public override string? PhoneNumber { get; set; }  // Make PhoneNumber nullable

        public UserType? UserType { get; set; }

        public string? UserImageURL { get; set; }

        public ICollection<Property>? Properties { get; set; } = new List<Property>();

        public ICollection<Favorite>? Favorites { get; set; } = new List<Favorite>();

        public ICollection<PropertyRequest>? Requests { get; set; } = new List<PropertyRequest>();


    }
    public enum UserType
    {
        User,
        Agent
    }
}


