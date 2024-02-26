using Microsoft.AspNetCore.Mvc;
using TinyBlog.ViewModel;
using Microsoft.AspNetCore.Authorization;
using TinyBlog.Data;
using Microsoft.AspNetCore.Identity;
using TinyBlog.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.Drawing;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TinyBlog.Utilities;
using System.Linq;
using AspNetCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Build.Logging;

namespace TinyBlog.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notification { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostController(ApplicationDbContext context,
                                INotyfService notyfService,
                                IWebHostEnvironment webHostEnvironment,
                                UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _notification = notyfService;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var listofposts = new List<post>();
            var LoggedInUser =await _userManager.Users.FirstOrDefaultAsync(x=>x.UserName==User.Identity!.Name); 
            if (LoggedInUser != null)
            {

            var LoggedInUserRole = await _userManager.GetRolesAsync(LoggedInUser!);
            if (LoggedInUserRole[0] == WebsiteRoles.WebsiteAdmin)
            {
                listofposts = await _context.PostTable!.Include(x=>x.ApplicationUser).ToListAsync();
            }
            else
            {
                listofposts = await _context.PostTable!.Include(x => x.ApplicationUser).Where(x => x.ApplicationUser!.Id == LoggedInUser!.Id).ToListAsync();
            }
            var listofPostsVM = listofposts.Select(x=>new PostVM()
            {
                Id = x.Id,
                Title = x.Title,
                CreatedDate = x.CreatedTime,
                ThumbnailUrl = x.ThumbnailUrl,
                AuthorName=x.ApplicationUser!.FirstName+" "+x.ApplicationUser!.LastName

                    
            }).OrderByDescending(x=>x.CreatedDate).ToList();
            return View(listofPostsVM);
            }
            else {

                _notification.Warning("user not found");
                return View(listofposts); 
            }
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostVMcs vm)
        {
            if (!ModelState.IsValid) { return View(vm); }

            
            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);

            var post = new post();

            post.Title = vm.Title;
            post.Description = vm.Description;
            post.ShortDescription = vm.ShortDescription;
            post.ApplicationUserID = loggedInUser!.Id;

            if (post.Title != null)
            {
                string slug = vm.Title!.Trim();
                slug = slug.Replace(" ", "-");
                post.slug = slug + "-" + Guid.NewGuid();
            }

            if (vm.Thumbnail != null)
            {
                post.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }

            await _context.PostTable!.AddAsync(post);
            await _context.SaveChangesAsync();
            _notification.Success("Post Created Successfully");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _context.PostTable!.FirstOrDefaultAsync(x=>x.Id==id);
             var LoggedInUSer=await _userManager.Users.FirstOrDefaultAsync(x=>x.UserName==User.Identity!.Name);
            var userrole = await _userManager.GetRolesAsync(LoggedInUSer!);
            if (userrole[0] == WebsiteRoles.WebsiteAdmin || LoggedInUSer?.Id == post?.ApplicationUserID) {
            if(post == null)
            {
                _notification.Error("User not found");
                return View();
            }   
            var vm = new CreatePostVMcs()
            {
               Id =post.Id,
               Title= post.Title,
               Description = post.Description,
               ShortDescription = post.ShortDescription,
               ApplicationUserID=post.ApplicationUserID,
               ThumbnailUrl =post.ThumbnailUrl
            };
            return View(vm);
            }
            _notification.Warning("Authority not given");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreatePostVMcs vm)
        {
            if (!ModelState.IsValid)
            {
            return View(vm);

            }
            var post =await _context.PostTable!.FirstOrDefaultAsync(x=>x.Id==vm.Id);
            if(post == null)
            {
                _notification.Error("User not found");
                return View(vm);
            }
            post.Title = vm.Title;
            post.Description = vm.Description;
            post.CreatedTime = DateTime.UtcNow;
            post.ShortDescription = vm.ShortDescription;
            if (vm.Thumbnail != null)
            {
                post.ThumbnailUrl = UploadImage(vm.Thumbnail);
            }
            await _context.SaveChangesAsync();
            _notification.Success("updated successfully");
            return RedirectToAction("Index","Post",new {area ="Admin"});
           
       }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.PostTable!.FirstOrDefaultAsync(x => x.Id == id);

            var loggedInUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);
            var loggedInUserRole = await _userManager.GetRolesAsync(loggedInUser!);

            if (loggedInUserRole[0] == WebsiteRoles.WebsiteAdmin || loggedInUser?.Id == post?.ApplicationUserID)
            {
                _context.PostTable!.Remove(post!);
                await _context.SaveChangesAsync();
                _notification.Success("Post Deleted Successfully");
                return RedirectToAction("Index", "Post", new { area = "Admin" });
            }
            return View();
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
