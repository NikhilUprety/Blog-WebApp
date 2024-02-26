using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TinyBlog.Models;

namespace TinyBlog.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)

        {
            
        }
        public DbSet<ApplicationUser>? ApplicationUsersTable { get; set; }
        public DbSet<page>? PageTable { get; set; }
        public DbSet<post>? PostTable { get; set; }
        public DbSet<Setting>? settingTable { get; set; }
    }

}
