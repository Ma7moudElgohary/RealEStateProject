using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Models
{
    public class PropertyRequest
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("PropertyId")]
        public int PropertyId { get; set; }

        public Property Property { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string Message { get; set; }

        public RequestStatus Status { get; set; } = RequestStatus.PENDING;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}