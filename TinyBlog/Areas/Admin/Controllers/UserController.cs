using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using NuGet.Protocol.Plugins;
using System.Drawing.Text;
using System.Threading.Tasks;
using TinyBlog.Data;
using TinyBlog.Models;
using TinyBlog.Utilities;
using TinyBlog.ViewModel;



namespace TinyBlog.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _SignInUser;
        private readonly INotyfService _notification;
        private object user;

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> SignInUser, INotyfService notification)
        {
            _context = context;
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
        Username = x.UserName,
        Email = x.Email,
    }).ToList();

    foreach (var user in vm)
            {
                var singleuser =await _userManager.FindByIdAsync(user.ID);
                var role = await _userManager.GetRolesAsync(singleuser);
                user.Role = role.FirstOrDefault();
            }

        
    return View(vm);  
}


        [Authorize(Roles = "Admin")]
        [HttpGet]
         public  async Task<IActionResult>  ResetPassword(string id)
        {
            var existing_user =await _userManager.FindByIdAsync(id);
            if (existing_user == null)
            {
                _notification.Error("user not found");
                return View();
            }
            var vm = new ResetPasswordVM()
            {
                Id = existing_user.Id,
                Username=existing_user.UserName
                    
            };
            
            return View(vm);
   
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var existingUser = await _userManager.FindByIdAsync(vm.Id);
            if (existingUser == null)
            {
                _notification.Error("User doesnot exist");
                return View(vm);
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
            var result = await _userManager.ResetPasswordAsync(existingUser, token, vm.NewPassword);
            if (result.Succeeded)
            {
                _notification.Success("Password reset sucessful");
                return RedirectToAction(nameof(Index));
            }
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
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var checkUserByEmail = await _userManager.FindByEmailAsync(vm.Email);
            if (checkUserByEmail != null)
            {
                _notification.Error("Email already exists");
                return View(vm);
            }
            var checkUserByUsername = await _userManager.FindByNameAsync(vm.UserName);
            if (checkUserByUsername != null)
            {
                _notification.Error("Username already exists");
                return View(vm);
            }

            var applicationUser = new ApplicationUser()
            {
                Email = vm.Email,
                UserName = vm.UserName,
                FirstName = vm.FirstName,
                LastName = vm.LastName
            };

            var result = await _userManager.CreateAsync(applicationUser, vm.Password);
            if (result.Succeeded)
            {
                if (vm.IsAdmin)
                {
                    await _userManager.AddToRoleAsync(applicationUser, WebsiteRoles.WebsiteAdmin);
                }
                else
                {
                    await _userManager.AddToRoleAsync(applicationUser, WebsiteRoles.WebsiteAuthor);
                }
                _notification.Success("User registered successfully");
                return RedirectToAction("Index", "User", new { area = "Admin" });
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
            return RedirectToAction("Index", "Post", new { Area = "Admin" });
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
            return RedirectToAction("Index", "Post", new { area = "Admin" });

        }

        [Authorize]
        public IActionResult logout()
        {
            _SignInUser.SignOutAsync();
            _notification.Success("succesfully logout");
            return RedirectToAction("Index","Home",new {area = ""});
        }


        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
      

    }
}