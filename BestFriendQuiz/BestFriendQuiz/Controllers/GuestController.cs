using BestFriendQuiz.Context;
using BestFriendQuiz.EntityLayer.Concrete;
using BestFriendQuiz.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BestFriendQuiz.Controllers
{
    public class GuestController : Controller
    {
        private readonly BFQContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;

        public GuestController(BFQContext context, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

  
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(GuestKeyViewModel guestKeyViewModel)
        {
            string key = guestKeyViewModel.Url.Substring(35, guestKeyViewModel.Url.Length - 35);
            User user = _context.Users.FirstOrDefault(x => x.Key.ToString() == key);

            Response.Cookies.Append("Username", user.UserName);
            Response.Cookies.Append("UserId", user.Id.ToString());
            Response.Cookies.Append("RoleId", user.RoleId.ToString());
            Response.Cookies.Append("Firstname", user.Firstname);

            return RedirectToAction("Survey", "Survey");
        }



   
        [AllowAnonymous]
        public ActionResult Warning()
        {

            string message = "Bu kullanıcı maksimum anket cevabına ulaşmıştır.";
            ViewBag.WarningMessage = message;
            return View();
        }

      
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Create(User model)
        {

            var identityUser = new User();
            var user = _userManager.Users.OrderByDescending(x => x.Id).FirstOrDefault();
            identityUser = new User
            {
                UserName = "guest" + (user.Id + 1),
                Email = "guest" + (user.Id + 1) + "@hotmail.com",
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                RoleId = 3,
                Role = await _roleManager.FindByIdAsync("3")

            };

            var result = await _userManager.CreateAsync(identityUser, "Guest123-");
            var result2 = await _userManager.AddToRoleAsync(identityUser, identityUser.Role.Name); 

         
            if (result.Succeeded)
            {
                Response.Cookies.Append("Username", identityUser.UserName);
                Response.Cookies.Append("UserId", identityUser.Id.ToString());
                Response.Cookies.Append("RoleId", identityUser.RoleId.ToString());
                Response.Cookies.Append("Firstname", identityUser.Firstname);
             
                return RedirectToAction("Create", "Survey");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }
            }
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register(int id)
        {
            User user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            GenericViewModel userDto = new GenericViewModel();
            userDto.Firstname = user.Firstname;
            userDto.Lastname = user.Lastname;
            userDto.Id = id;


            return View(userDto);
        }

       
        [HttpPost]
        [AllowAnonymous]

        public async Task<ActionResult> Register(GenericViewModel model)
        {
            if( ModelState.IsValid )
            {

                if( model.Password == model.RePassword )
                {

                    User identityUser = _context.Users.FirstOrDefault(x => x.Id == model.Id);

                    identityUser.UserName = model.Email;
                    identityUser.Firstname = model.Firstname;
                    identityUser.Lastname = model.Lastname;
                    identityUser.Email = model.Email;
                    identityUser.Password = model.Password;
                    identityUser.RoleId = 2;
                    identityUser.Role = await _roleManager.FindByIdAsync("2");

                    var result = await _userManager.UpdateAsync(identityUser);
                    var result2 = await _userManager.AddToRoleAsync(identityUser, identityUser.Role.Name); 
                    var result3 = await _userManager.RemoveFromRoleAsync(identityUser, "Guest"); 


                    if( result.Succeeded )
                    {

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach( var error in result.Errors )
                        {
                            ModelState.AddModelError("Register", error.Description);
                        }
                    }


                }
                else
                {
                    ModelState.AddModelError("Register", "Passwords do not match.");
                }
            }
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

     
        public ActionResult Delete(int id)
        {
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
