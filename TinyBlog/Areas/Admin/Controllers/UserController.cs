using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Text;
using TinyBlog.Models;
using TinyBlog.Utilities;
using TinyBlog.ViewModel;

namespace TinyBlog.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _SignInUser;
        private readonly INotyfService _notification;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> SignInUser, INotyfService notification)
        {
            _userManager = userManager;
            _SignInUser = SignInUser;
            _notification = notification;


        }
[Authorize(Roles = "Admin")]
[HttpGet]
public async Task<IActionResult> Index()
{
    var users = await _userManager.Users.ToListAsync();
    var vm = users.Select(x => new UserVMcs()
    {
        ID = x.Id,
        FirstName = x.FirstName,
        LastName = x.LastName,
        Username = x.UserName
    }).ToList();
        
    return View(vm);  
}

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterVM());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]

        public async Task<IActionResult>  Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
            return View(vm);

            }
          var checkemail = await  _userManager.FindByEmailAsync(vm.Email);
            if (checkemail != null)
            {
                _notification.Error("Email already exist");
                return View(vm);
            }
            var checkusername = await _userManager.FindByNameAsync(vm.UserName);
            if(checkusername != null)
            {
                _notification.Error($"User name: {vm.UserName} already taken");
                return View(vm);

            }
            var applicationuser = new ApplicationUser()
            {
                Email = vm.Email,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                UserName = vm.UserName
            };
            var result =await  _userManager.CreateAsync(applicationuser, vm.Password);
            if (result.Succeeded)
            {
                if (vm.IsAdmin)
                {
                    await _userManager.AddToRoleAsync(applicationuser, WebsiteRoles.WebsiteAdmin);
                }
                else
                {
                    await _userManager.AddToRoleAsync(applicationuser,WebsiteRoles.WebsiteAuthor);
                }
                _notification.Success("user registered succesfuly");
                return RedirectToAction("Index","User",new {area = "Admin"});
            }
            return View(vm);

        }
       
        

        [HttpGet("login")]
        public IActionResult Login()
        {
            if (!HttpContext.User.Identity!.IsAuthenticated)
            {
                return View(new loginVM());

            }
            return RedirectToAction("Index", "User", new { Area = "Admin" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(loginVM vm)
        {
            if (!ModelState.IsValid)
            {

                return View(vm);

            }
            var existingUSer = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == vm.username);
            if (existingUSer == null)
            {
                _notification.Error("Username not found");
                return View(vm);
            }
            var checkpassword = await _userManager.CheckPasswordAsync(existingUSer, vm.password);
            if (!checkpassword)
            {

                _notification.Error("Password not found");
                return View(vm);
            }
            await _SignInUser.PasswordSignInAsync(vm.username, vm.password, vm.RememberMe, true);
            _notification.Success("login succesful");
            return RedirectToAction("Index", "User", new { Area = "Admin" });

        }

        public IActionResult logout()
        {
            _SignInUser.SignOutAsync();
            return RedirectToAction("Index","Home",new {area = ""});
            _notification.Success("succesfully logout");
        }       


    }
}