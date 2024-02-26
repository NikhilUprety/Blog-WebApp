using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinyBlog.Data;
using TinyBlog.Models;
using TinyBlog.ViewModel;

namespace TinyBlog.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class SettingController : Controller
    {
        private readonly ApplicationDbContext _context;
        public INotyfService _notification { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SettingController(ApplicationDbContext context, INotyfService notification, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _notification = notification;
            _webHostEnvironment = webHostEnvironment;
        }

        public async  Task<IActionResult> Index()
        {
            var setting = _context.settingTable!.ToList();
            if(setting.Count > 0)
            {
                var settingvm = new SettingVM()
                {
                    Id = setting[0].Id,
                    SiteName = setting[0].SiteName,
                    ShortDescription = setting[0].ShortDescription,
                    ThumbnailUrl = setting[0].ThumbnailUrl
                };
            }
           return View();
        }
    }
}
