using Final_Project.Areas.Identity.Data;
using Final_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Final_Project.Controllers
{
    
    public class UsersController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch all users from the database
            var users = _userManager.Users.ToList();
            var userList = new List<UserListViewModel>();

            foreach (var user in users)
            {
                // Get roles for the current user
                var roles = await _userManager.GetRolesAsync(user);

                userList.Add(new UserListViewModel
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Roles = string.Join(", ", roles)
                });
            }

            return View(userList);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(UserRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 1. Check if role exists, if not create it
                if (!await _roleManager.RoleExistsAsync(model.Role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(model.Role));
                }

                // 2. Map ViewModel to ApplicationUser
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailConfirmed = true // Bypassing email confirmation for manual adds
                };

                // 3. Create the user
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // 4. Assign the Role
                    await _userManager.AddToRoleAsync(user, model.Role);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        // GET: Users/AddAdmin
        public IActionResult AddAdmin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAdmin(string email, string password, string firstName, string lastName)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    EmailConfirmed = true // Auto-confirm for internal admins
                };

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    // Ensure the Admin role exists
                    if (!await _roleManager.RoleExistsAsync("Admin"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    }

                    // Assign the Role
                    await _userManager.AddToRoleAsync(user, "Admin");

                    return RedirectToAction("Index", "Home"); // Or wherever your user list is
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }



    }
}
