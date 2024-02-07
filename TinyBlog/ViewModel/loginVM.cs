using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;

namespace TinyBlog.ViewModel
{
    public class loginVM
    {
        [Required]
        public string? username { get; set; }

        [Required]
        public string? password { get; set; }

        public bool RememberMe { get; set; }
    }
}
