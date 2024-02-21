using TinyBlog.Data;
using TinyBlog.Models;
using Microsoft.AspNetCore.Identity;

using TinyBlog.Utilities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FineBlog.Utilites
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DbInitializer(ApplicationDbContext context,
                            UserManager<ApplicationUser> userManager,
                            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            if (!_roleManager.RoleExistsAsync(WebsiteRoles.WebsiteAdmin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.WebsiteAdmin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.WebsiteAuthor)).GetAwaiter().GetResult();
                _userManager.CreateAsync(new ApplicationUser()
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    FirstName = "Super",
                    LastName = "Admin"
                }, "Admin@0011").Wait();

                var appUser = _context.ApplicationUsersTable!.FirstOrDefault(x => x.Email == "admin@gmail.com");
                if (appUser != null)
                {
                    _userManager.AddToRoleAsync(appUser, WebsiteRoles.WebsiteAdmin).GetAwaiter().GetResult();
                }

                
                var listOfPages = new List<page>()
                {
                    new page()
                    {
                         Tittle = "About Us",
                        Slug = "about"
                    },
                    new page()
                    {
                        Tittle = "Contact Us",
                        Slug = "contact"
                    },
                    new page()
                    {
                        Tittle = "Privacy Policy",
                        Slug = "privacy"
                    }
                 };

                _context.PageTable!.AddRange(listOfPages);

                //var setting = new Setting
                //{
                //    SiteName = "Site Name",
                //    Title = "Site Title",
                //    ShortDescription = "Short Description of site"
                //};

                //_context.Settings!.Add(setting);
                _context.SaveChanges();

            }
        }
    }
}
