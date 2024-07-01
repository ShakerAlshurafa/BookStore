using BookStore.Data;
using BookStore.Models;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace BookStore.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext context;
		private readonly IWebHostEnvironment webHostEnvironment;

		public BooksController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment) {
            this.context = context;
			this.webHostEnvironment = webHostEnvironment;
		}
        public IActionResult Index()
        {
            var books = context.Books.
                Include(book => book.Author).
                Include(book => book.Categories).
                ThenInclude(book => book.Category).
                ToList();

            var bookVMs = books.Select(book => new BookVM
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author.Name,
                Publisher = book.Publisher,
                publishDate = book.publishDate,
                ImageUrl = book.ImageUrl,
                Categories = book.Categories.Select(book=>book.Category.Name).ToList(),
            }).ToList();

            return View(bookVMs);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            var authors = context.Authors.OrderBy(author=>author.Name)
                .Select(author => new SelectListItem
                {
                    Value = author.Id.ToString(),
                    Text = author.Name,
                }).ToList();


            var categories = context.Categories.OrderBy(category => category.Name)
                .Select(category => new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name,
                }).ToList();
			
			var viewModel = new BookFormVM
            {
                Authors = authors,
                Categories = categories,
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(BookFormVM viewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(viewModel);
            }
            string ImageName = null;

			if (viewModel.ImageUrl != null)
            {
                ImageName = Path.GetFileName(viewModel.ImageUrl.FileName);
                var path = Path.Combine($"{webHostEnvironment.WebRootPath}/img/books", ImageName);
                var stream = System.IO.File.Create(path);
                viewModel.ImageUrl.CopyTo(stream);
            }

            var book = new Book
            {
                Title = viewModel.Title,
                AuthorId = viewModel.AuthorId,
                Publisher = viewModel.Publisher,
                publishDate = viewModel.publishDate,
                Description = viewModel.Description,
                ImageUrl = ImageName,
                Categories = viewModel.SelectedCategories.Select(id => new BookCategory
                {
                    CategoryId = id,
                }).ToList(),
            };
            context.Books.Add(book);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var book = context.Books.Find(id);
            if(book is null)
            {
                return NotFound();
            }
            if(book.ImageUrl != null)
            {
                var path = Path.Combine(webHostEnvironment.WebRootPath,"/img/books", book.ImageUrl);
                if(System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            context.Books.Remove(book);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
