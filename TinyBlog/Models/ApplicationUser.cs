using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace TinyBlog.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string? FirstName { get; set; }  
        public string? LastName { get; set;}
        public List<post>? Posts { get; set; }

    }

}


