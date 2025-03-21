using System.ComponentModel.DataAnnotations;

namespace RealEStateProject.ViewModels.Roles
{
    public class EditRoleViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Role name is required")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
