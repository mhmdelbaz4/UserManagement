using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UserManagement.ViewModels;

namespace UserManagement.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var Roles = await _roleManager.Roles.ToListAsync();

            return View(Roles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Add(RoleViewModel model)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index), await _roleManager.Roles.ToListAsync());

            bool IsRoleExist = await _roleManager.RoleExistsAsync(model.Name);

            if(IsRoleExist)
            {
                ModelState.AddModelError("Name","Role is already exist");
                return View(nameof(Index), await _roleManager.Roles.ToListAsync());
            }

            await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));

            return RedirectToAction(nameof(Index));
        }
    }
}
