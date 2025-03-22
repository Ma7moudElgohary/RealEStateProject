using RealEstate.Models;

namespace RealEStateProject.Models
{
    public class Agent
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string LicenseNumber { get; set; }

        public string Agency { get; set; }

        public string Biography { get; set; }

        public int YearsOfExperience { get; set; }

        public string? ProfileImageUrl { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual List<Property>? Properties { get; set; } = new List<Property>();
    }
}
