using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TinyBlog.Data;
using TinyBlog.Models;
using TinyBlog.ViewModel;

namespace TinyBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notification { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context,
                                INotyfService notyfService,
                                IWebHostEnvironment webHostEnvironment,
                                UserManager<ApplicationUser> userManager,
                                ILogger<HomeController> logger)
        {
            _context = context;
            _notification = notyfService;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _logger = logger;
        }

        
            
        public IActionResult Index()
        {
            var setting = _context.settingTable!.ToList();
            var vm = new HomeVM()
            {
            Title = setting[0].Title,
            ShortDescription = setting[0].ShortDescription,
            ThumbnailUrl = setting[0].ThumbnailUrl,
            Posts = _context.PostTable!.Include(x=>x.ApplicationUser).ToList(),

        };
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}