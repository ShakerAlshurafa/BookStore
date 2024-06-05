using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModel
{
    public class AuthorFormVM
    {
        public int Id { get; set; }
        [MaxLength(50,ErrorMessage ="the name field can't exceed 50 characters")]
        public string Name { get; set; } = null!;
    }
}
