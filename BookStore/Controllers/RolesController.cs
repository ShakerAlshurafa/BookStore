using BookStore.Data;
using BookStore.Models;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await roleManager.Roles.ToListAsync();
            var roleVM = roles.Select(role => new RoleVM
            {
                Name = role.Name
            }).ToList();
            return View(roleVM);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleVM roleVM)
        {
            if (!ModelState.IsValid)
            {
                return View(roleVM);
            }

            var result = await roleManager.CreateAsync(new IdentityRole(roleVM.Name));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var role = context.Roles.Find(id);
            if (role == null)
            {
                return NotFound();
            }
            var roleVM = new RoleVM
            {
                Name = role.Name
            };
            return View("Create",roleVM);
        }
        [HttpPost]
        public IActionResult Edit(RoleVM roleVM)
        {
			if (!ModelState.IsValid)
			{
				return View("Create", roleVM);
			}
            var role = context.Roles.Find(roleVM.Id);
            if (role == null)
            {
                return NotFound();
            }
            role.Name = roleVM.Name;
            context.SaveChanges();
			return RedirectToAction("Index");
        }
    }
}
