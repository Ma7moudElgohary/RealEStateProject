using System.ComponentModel.DataAnnotations;

namespace RealEStateProject.ViewModels.Common
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
