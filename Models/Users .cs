using Microsoft.AspNetCore.Identity;

namespace RealEStateProject.Models
{

    public class Users : IdentityUser
    {
        public string FullName { get; set; }
    }

}
