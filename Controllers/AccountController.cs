using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Rendering;
using RealEstate.Models;
using RealEStateProject.ViewModels.User;

namespace RealEStateProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        #region Register

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.UserTypes = Enum.GetValues(typeof(UserType))
                           .Cast<UserType>()
                           .Select(e => new SelectListItem
                           {
                               Value = ((int)e).ToString(),
                               Text = e.ToString()
                           }).ToList();
            return View("Register");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel userFromRequest)
        {
            if (ModelState.IsValid)
            {
                var userModel = new ApplicationUser();
                userModel.FirstName = userFromRequest.FirstName;
                userModel.LastName = userFromRequest.LastName;
                userModel.Email = userFromRequest.Email;
                userModel.UserName = userFromRequest.Username;
                userModel.UserType = userFromRequest.UserType;
                userModel.PhoneNumber = userFromRequest.PhoneNumber ?? "0000000";

                IdentityResult result = await userManager.CreateAsync(userModel, userFromRequest.Password);

                if (result.Succeeded)
                {
                    // Assign role based on UserType
                    if (userFromRequest.UserType == UserType.Agent)
                    {
                        await userManager.AddToRoleAsync(userModel, "Agent");
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(userModel, "User");
                    }

                    await signInManager.SignInAsync(userModel, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }

            // Reload UserTypes if model state is invalid
            ViewBag.UserTypes = Enum.GetValues(typeof(UserType))
                                    .Cast<UserType>()
                                    .Select(e => new SelectListItem
                                    {
                                        Value = ((int)e).ToString(),
                                        Text = e.ToString()
                                    }).ToList();

            return View("Register", userFromRequest);
        }
        #endregion

        #region Login

        [HttpGet]
        public IActionResult Login()
        {

            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel userFromRequest)
        {
            if (ModelState.IsValid)
            {
                var userFromDataBase = await userManager.FindByNameAsync(userFromRequest.Username);

                if (userFromDataBase != null)
                {
                    bool found = await userManager.CheckPasswordAsync(userFromDataBase, userFromRequest.Password);
                    if (found)
                    {
                        List<Claim> claims = new List<Claim>();
                        claims.Add(new Claim("UserType", userFromDataBase.UserType.ToString()));

                        await signInManager.SignInWithClaimsAsync(userFromDataBase, userFromRequest.RemeberMe, claims);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Invalid Username and Password");
            }
            return View("Login", userFromRequest);
        }

        #endregion

        #region Logout

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region External Login

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError("", $"Error: {remoteError}");
                return RedirectToAction("Login");
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("Login");
            }

            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (signInResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var email = info.Principal.FindFirstValue(System.Security.Claims.ClaimTypes.Email);
                if (email != null)
                {
                    var user = new ApplicationUser { UserName = email, Email = email };
                    var result = await userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        await userManager.AddLoginAsync(user, info);
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return RedirectToAction("Login");
        }

        #endregion


    }
}
