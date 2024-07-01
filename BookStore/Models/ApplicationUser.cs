using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(50)]
        public string Address { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn {  get; set; } = DateTime.Now;
        public DateTime UpdateOn { get; set;} = DateTime.Now;
    }
}
