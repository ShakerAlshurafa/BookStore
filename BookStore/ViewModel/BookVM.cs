using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModel
{
	public class BookVM
	{
		public int Id { get; set; }
		[MaxLength(50)]
		public string Title { get; set; } = null!;
		public string Author { get; set; } = null!;
		public string Publisher { get; set; } = null!;
		[Display(Name = "Publish Date")]
		public DateTime publishDate { get; set; }
		[Display(Name = "Image")]
		public string? ImageUrl { get; set; }
		public string Description { get; set; } = null!;
		public List<string> Categories { get; set; } 
	}
}
