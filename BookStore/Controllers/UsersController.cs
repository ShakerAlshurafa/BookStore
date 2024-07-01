using BookStore.Data;
using BookStore.Models;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

		public UsersController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager) 
        {
            this.context = context;
            this.roleManager = roleManager;
            this.userManager = userManager;
		}
        public async Task<IActionResult> Index()
        {
            var users = await userManager.Users.ToListAsync();
            var userVM = new List<ApplicationUserVM>();
            foreach(var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                var userData = new ApplicationUserVM
                {
                    UserName = user.UserName,
                    Address = user.Address,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles.ToList(),
                };
                userVM.Add(userData);
            }
            
            return View(userVM);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roles = await roleManager.Roles.ToListAsync();
            var viewModel = new ApplicationUserCreateVM
            {
                Roles = roles.Select(role => new SelectListItem
                {
                    Value = role.Name,
                    Text = role.Name
                }).ToList(),
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUserCreateVM userVM)
        {
            if(!ModelState.IsValid)
            {
                return View(userVM);
            }
            var user = new ApplicationUser
            {
                UserName = userVM.UserName,
                Email = userVM.Email,
                Address = userVM.Address,
                PhoneNumber = userVM.PhoneNumber,
            };
			var result = await userManager.CreateAsync(user,userVM.PasswordHash);
            if (!result.Succeeded)
            {
                return View(userVM);
            }
            await userManager.AddToRolesAsync(user,userVM.SelectedRoles);
            return RedirectToAction("Index");
        }
    }
}
