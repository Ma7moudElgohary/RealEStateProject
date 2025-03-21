using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Models;
using RealEStateProject.ViewModels.Common;
using RealEStateProject.ViewModels.Roles;

[Authorize(Roles = "Admin")] // Only admins can access role management
public class RoleController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    // GET: List all roles
    public IActionResult Index()
    {
        var roles = _roleManager.Roles;
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
            // Check if role already exists
            var roleExists = await _roleManager.RoleExistsAsync(roleFromRequest.RoleName);
            if (!roleExists)
            {
                var role = new IdentityRole(roleFromRequest.RoleName);
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = $"Role '{roleFromRequest.RoleName}' created successfully!";
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            else
            {
                ModelState.AddModelError("", $"Role '{roleFromRequest.RoleName}' already exists.");
            }
        }
        return View(roleFromRequest);
    }

    // Add a new method to assign roles to users
    public async Task<IActionResult> ManageUserRoles()
    {
        var users = _userManager.Users.ToList();
        var model = new List<UserRolesViewModel>();

        foreach (var user in users)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            model.Add(new UserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = string.Join(", ", userRoles)
            });
        }

        return View(model);
    }

    public async Task<IActionResult> EditUserRoles(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        var allRoles = _roleManager.Roles.ToList();

        var model = new EditUserRolesViewModel
        {
            UserId = user.Id,
            UserName = user.UserName,
            UserRoles = allRoles.Select(r => new RoleSelection
            {
                RoleId = r.Id,
                RoleName = r.Name,
                IsSelected = userRoles.Contains(r.Name)
            }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUserRoles(EditUserRolesViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null)
        {
            return NotFound();
        }

        var userRoles = await _userManager.GetRolesAsync(user);

        // Remove all existing roles
        await _userManager.RemoveFromRolesAsync(user, userRoles);

        // Add selected roles
        var selectedRoles = model.UserRoles.Where(r => r.IsSelected).Select(r => r.RoleName).ToList();
        await _userManager.AddToRolesAsync(user, selectedRoles);

        TempData["SuccessMessage"] = $"Roles for user '{user.UserName}' updated successfully!";
        return RedirectToAction("ManageUserRoles");
    }
}