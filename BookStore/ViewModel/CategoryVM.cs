using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModel
{
    public class CategoryVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="plz insert name")]
        [MaxLength(30,ErrorMessage ="Max length is 30")]
        [Remote("checkName",null,ErrorMessage ="exists")]
        public string Name { get; set; } = null!;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}
