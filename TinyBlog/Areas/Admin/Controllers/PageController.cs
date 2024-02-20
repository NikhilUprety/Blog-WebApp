using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TinyBlog.Data;
using TinyBlog.Models;
using TinyBlog.ViewModel;

namespace TinyBlog.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PageController : Controller

    {


        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notification;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PageController(ApplicationDbContext context, INotyfService notification, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _notification = notification;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> About()
        {
            var page = await _context.PageTable!.FirstOrDefaultAsync(x => x.Slug == "about");
            if (page == null) {
                _notification.Error("no page found for a user");
                 return View();
            }
            var vm = new PageVMcs()
            {
                Id = page.Id,
                Tittle = page.Tittle,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ThumbnailUrl = page.ThumbnailUrl,

            };
            return View(vm);

        }
        [HttpPost]
        public async Task<IActionResult> About(PageVMcs vm)
        {
            var page = await _context.PageTable!.FirstOrDefaultAsync(x => x.Slug == "about");
            if (page == null)
            {
                _notification.Error("no page found for a user");
                return View();
            }


            page.Tittle = vm.Tittle;
            page.ShortDescription = vm.ShortDescription;
            page.Description = vm.Description;

            if(vm.ThumbnailUrl != null) {

                page.ThumbnailUrl = UploadImage(vm.Thumbnail!);
            }
            await _context.SaveChangesAsync();
            _notification.Success("About page updated sucessfully");
            return RedirectToAction("Index", "Post", new { area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> Contact()
        {
            var page = await _context.PageTable!.FirstOrDefaultAsync(x => x.Slug == "contact");
            if (page == null)
            {
                _notification.Error("no page found for a user");
                return View();
            }
            var vm = new PageVMcs()
            {
                Id = page.Id,
                Tittle = page.Tittle,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ThumbnailUrl = page.ThumbnailUrl,

            };
            return View(vm);

        }
        [HttpPost]
        public async Task<IActionResult> Contact(PageVMcs vm)
        {
            var page = await _context.PageTable!.FirstOrDefaultAsync(x => x.Slug == "contact");
            if (page == null)
            {
                _notification.Error("no page found for a user");
                return View();
            }


            page.Tittle = vm.Tittle;
            page.ShortDescription = vm.ShortDescription;
            page.Description = vm.Description;

            if (vm.ThumbnailUrl != null)
            {

                page.ThumbnailUrl = UploadImage(vm.Thumbnail!);
            }
            await _context.SaveChangesAsync();
            _notification.Success("About page updated sucessfully");
            return RedirectToAction("Index", "Post", new { area = "Admin" });
        }
        [HttpGet]
        public async Task<IActionResult> Privacy()
        {
            var page = await _context.PageTable!.FirstOrDefaultAsync(x => x.Slug == "privacy");
            if (page == null)
            {
                _notification.Error("no page found for a user");
                return View();
            }
            var vm = new PageVMcs()
            {
                Id = page.Id,
                Tittle = page.Tittle,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ThumbnailUrl = page.ThumbnailUrl,

            };
            return View(vm);

        }
        [HttpPost]
        public async Task<IActionResult> Privacy(PageVMcs vm)
        {
            var page = await _context.PageTable!.FirstOrDefaultAsync(x => x.Slug == "privacy");
            if (page == null)
            {
                _notification.Error("no page found for a user");
                return View();
            }


            page.Tittle = vm.Tittle;
            page.ShortDescription = vm.ShortDescription;
            page.Description = vm.Description;

            if (vm.ThumbnailUrl != null)
            {

                page.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }
            await _context.SaveChangesAsync();
            _notification.Success("About page updated sucessfully");
            return RedirectToAction("Index", "Post", new { area = "Admin" });
        }
        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = "";
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "thumbnails");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(folderPath, uniqueFileName);
            using (FileStream fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);
            }
            return uniqueFileName;
        }

    }
}
