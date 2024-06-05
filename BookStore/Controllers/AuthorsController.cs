using BookStore.Data;
using BookStore.Models;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext context;

        public AuthorsController(ApplicationDbContext context) 
        { 
            this.context = context;
        }
        public IActionResult Index()
        {
            var authors = context.Authors
                .Select(author => new AuthorVM
                {
                    Id = author.Id,
                    Name = author.Name,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                }).ToList();

            return View(authors);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Form");
        }

        [HttpPost]
        public IActionResult Create(AuthorFormVM authorFormVM)
        {
            if(!ModelState.IsValid)
            {
                return View("Form", authorFormVM);
            }
            var author = new Author
            {
                Name = authorFormVM.Name,
            };
			try
			{
				context.Authors.Add(author);
				context.SaveChanges();
				return RedirectToAction("Index");
			}
			catch
			{
				ModelState.AddModelError("Name", "Author name already exists");
				return View("Form",authorFormVM);
			}
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var author = context.Authors.Find(id);
            if(author is null)
            {
                return NotFound();
            }
            var authorVM = new AuthorFormVM()
            {
                Id = author.Id, 
                Name = author.Name,
            };
            return View("Form",authorVM);
        }

        [HttpPost]
        public IActionResult Edit(AuthorFormVM authorFormVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", authorFormVM);
            }
            var author = context.Authors.Find(authorFormVM.Id);
            if( author is null)
            {
                return NotFound();
            }
            author.Name = authorFormVM.Name;
            context.SaveChanges();
            return RedirectToAction("Index");
		}

        public IActionResult Details(int id)
        {
            var author = context.Authors.Find(id);
            if(author is null)
            {
                return NotFound();
            }
            var authorVM = new AuthorVM()
            {
                Id = author.Id,
                Name = author.Name,
                CreatedOn = author.CreatedOn,
                UpdatedOn = author.UpdatedOn,
            };
            return View(authorVM);
        }

        public IActionResult Delete(int id)
        {
            var author = context.Authors.Find(id);
            if(author is null) 
            { 
                return NotFound();
            }
            context.Remove(author);
            context.SaveChanges();
            return Ok();
        }
    }
}
