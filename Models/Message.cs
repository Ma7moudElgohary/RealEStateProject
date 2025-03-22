using RealEstate.Models;

namespace RealEStateProject.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public string AgentId { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string SenderPhone { get; set; }
        public string MessageText { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; }

        // Navigation properties
        public virtual Property Property { get; set; }

    }
}
