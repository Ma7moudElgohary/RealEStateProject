using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Services;
using RealEStateProject.ViewModels.Roles;
using System.Threading.Tasks;

namespace RealEStateProject.Controllers
{
    [Authorize(Roles = "Admin")] 
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // GET: List all roles
        public async Task<IActionResult> Index()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return View(roles);
        }

        // GET: Add new role
        public IActionResult AddRole()
        {
            return View();
        }

        // POST: Add new role
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(RoleViewModel roleFromRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.CreateRoleAsync(roleFromRequest);
                if (result)
                {
                    TempData["SuccessMessage"] = $"Role '{roleFromRequest.RoleName}' created successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", $"Role '{roleFromRequest.RoleName}' already exists or couldn't be created.");
                }
            }
            return View(roleFromRequest);
        }

        // GET: Manage user roles
        public async Task<IActionResult> ManageUserRoles()
        {
            var model = await _roleService.GetUsersWithRolesAsync();
            return View(model);
        }

        // GET: Edit user roles
        public async Task<IActionResult> EditUserRoles(string userId)
        {
            var model = await _roleService.GetUserRolesForEditAsync(userId);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Edit user roles
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserRoles(string UserId, string UserName, string SelectedRoleId)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                return NotFound();
            }

            // Get the current model to update
            var model = await _roleService.GetUserRolesForEditAsync(UserId);
            if (model == null)
            {
                return NotFound();
            }

            // Reset all selections
            foreach (var role in model.UserRoles)
            {
                role.IsSelected = (role.RoleId == SelectedRoleId);
            }

            // Update the roles
            var result = await _roleService.UpdateUserRolesAsync(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Role updated successfully!";
                return RedirectToAction("ManageUserRoles");
            }

            ModelState.AddModelError("", "Error updating role.");
            return View(model);
        }


      


        // GET: Delete role (with onfirmation)
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
            {
                TempData["ErrorMessage"] = "Role not found.";
                return RedirectToAction("Index");
            }

            // Check if any users are assigned to this role
            var usersInRole = await _roleService.GetUsersInRoleAsync(role.Name);
            if (usersInRole.Any())
            {
                TempData["ErrorMessage"] = $"Cannot delete role '{role.Name}' because it's assigned to {usersInRole.Count} user(s). Remove all users from this role first.";
                return RedirectToAction("Index");
            }

            var result = await _roleService.DeleteRoleAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Role deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Error deleting role. It might be a system role or in use.";
            }
            return RedirectToAction("Index");
        }



        // GET: Delete user
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _roleService.DeleteUserAsync(userId);
            if (result)
            {
                TempData["SuccessMessage"] = "User deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Error deleting user.";
            }
            return RedirectToAction("ManageUserRoles");
        }
    }
}