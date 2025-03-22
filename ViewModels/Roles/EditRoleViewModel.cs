using System.ComponentModel.DataAnnotations;

namespace RealEStateProject.ViewModels.Roles
{
    public class EditRoleViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Role name is required")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        // Keep track of original name for better feedback
        public string OriginalRoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
