using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.ViewModels;

namespace UserManagement.Controllers
{
    [Authorize(Roles ="Admin")]
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
            var users =await _userManager.Users.Select(user => new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                Roles = _userManager.GetRolesAsync(user).Result
            }).ToListAsync();

            return View(users);
        }

        public async Task<IActionResult> Add()
        {

            var roles =await _roleManager.Roles.ToListAsync();

            var model = new AddUserViewModel
            {
                Roles = roles.Select(r => new RoleViewModel
                {
                    Id = r.Id,
                    RoleName = r.Name
                }).ToList()
            };

            return View(model);
        }
       

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Add(AddUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if(await _userManager.FindByNameAsync(model.Username) is not null)
            {
                ModelState.AddModelError("Username", "Username is aleardy exists..");
                return View(model);
            }

            if (await _userManager.FindByEmailAsync(model.Email) is not null)
            {
                ModelState.AddModelError("Email", "Email is aleardy exists..");
                return View(model);
            }

            if(! model.Roles.Any(r => r.IsSelected))
            {
                ModelState.AddModelError("Roles", "please, select at least 1 Role..");
                return View(model);
            }

            ApplicationUser user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Username,
                Email = model.Username,
                
            };
 
            var result =  await _userManager.CreateAsync(user,model.Password);
            if(! result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Roles" , error.Description);
                }
            }

            await _userManager.AddToRolesAsync(user, model.Roles.Where(r => r.IsSelected).Select(r => r.RoleName));

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Modify(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return NotFound();

            var model = new ModifyUserViewModel
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                Email = user.UserName
            };

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Modify(ModifyUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            
            if (user is null)
                return NotFound();

            var UniqueEmail = await _userManager.FindByEmailAsync(model.Email);
            
            if((UniqueEmail is not null) && (UniqueEmail.Id != user.Id))
            {
                ModelState.AddModelError("Email", "Email is already exists..");
                return View(model);
            }

            var UniqueUsername = await _userManager.FindByEmailAsync(model.Username);

            if ((UniqueUsername is not null) && (UniqueUsername.Id != user.Id))
            {
                ModelState.AddModelError("Username", "Username is already exists..");
                return View(model);
            }

            user.FirstName = model.FirstName;
            user.LastName = model.FirstName;
            user.Email = model.Email;
            user.UserName = model.Username;
        
            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return NotFound();

            var roles = await _roleManager.Roles.ToListAsync();

            var viewModel = new UserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(role => new RoleViewModel
                {
                    Id = role.Id,
                    RoleName = role.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, role.Name).Result
                }).ToList()
            };

            return View(viewModel);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ManageRoles(UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in model.Roles)
            {
                if (userRoles.Any(r => r == role.RoleName) & !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);

                if (! userRoles.Any(r => r == role.RoleName) & role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.RoleName);
            }

            return RedirectToAction(nameof(Index));

        }
    }
}
