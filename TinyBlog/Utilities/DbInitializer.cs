using Microsoft.AspNetCore.Identity;
using TinyBlog.Data;
using TinyBlog.Models;

namespace TinyBlog.Utilities
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> rolemanager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = rolemanager;

        }
        public void Initialize()
        {
            
            if(!_roleManager.RoleExistsAsync(WebsiteRoles.WebsiteAdmin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.WebsiteAdmin));
            }
           
        }
    }
}
