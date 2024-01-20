using Microsoft.AspNetCore.Identity;
using TinyBlog.Data;
using TinyBlog.Models;

namespace TinyBlog.Utilities
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public DbInitializer()
        {
            
        }
        public void Initialize()
        {
            

            throw new NotImplementedException();
        }
    }
}
