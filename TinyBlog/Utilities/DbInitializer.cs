using Microsoft.AspNetCore.Identity;
using System.Linq;
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
                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.WebsiteAdmin)).GetAwaiter();
                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.WebsiteAuthor)).GetAwaiter();
                _userManager.CreateAsync(new ApplicationUser()
                {
                    UserName = "admin@gmail.com",
                    Email= "admin@gmail.com",
                    FirstName = "Super",
                    LastName = "Admin"
                },"Admin@0011").GetAwaiter();
                var appUser = _context.ApplicationUsersTable.FirstOrDefault(x => x.Email == "admin@gmail.com");

                if (appUser != null)
                {
                    _userManager.AddToRoleAsync(appUser,WebsiteRoles.WebsiteAdmin).GetAwaiter().GetResult();


                }

            }
           
        }
    }
}
