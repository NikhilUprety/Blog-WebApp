using Microsoft.AspNetCore.Identity;

namespace TinyBlog.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }  
        public string? LastName { get; set;}
        public List<post>? posts { get; set; }

    }

}


