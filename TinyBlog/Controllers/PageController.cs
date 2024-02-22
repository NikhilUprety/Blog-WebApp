using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TinyBlog.Data;
using TinyBlog.ViewModel;

namespace TinyBlog.Controllers
{
    public class PageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PageController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> About()
        {
            var about =await _context.PageTable!.FirstOrDefaultAsync(x => x.Slug == "about");
            var vm = new PageVMcs()
            {
                Tittle = about!.Tittle,
                ShortDescription= about!.ShortDescription,
                Description= about!.Description,
                ThumbnailUrl=about!.ThumbnailUrl,
                
            };
            return View(vm);
        }
        public async Task<IActionResult> Contact()
        {
            var contact= await _context.PageTable!.FirstOrDefaultAsync(x => x.Slug == "contact");
            var vm = new PageVMcs()
            {
                Tittle = contact!.Tittle,
                ShortDescription = contact.ShortDescription,
                Description = contact.Description,
                ThumbnailUrl = contact.ThumbnailUrl,

            };
            return View(vm);
            
        }

        public async Task<IActionResult> PrivacyPolicy()
        {
            var privacy = await _context.PageTable!.FirstOrDefaultAsync(x => x.Slug == "privacy");
            var vm = new PageVMcs()
            {
                Tittle = privacy!.Tittle,
                ShortDescription = privacy.ShortDescription,
                Description = privacy.Description,
                ThumbnailUrl = privacy.ThumbnailUrl,

            };
            return View(vm);

        }
    }
}
    