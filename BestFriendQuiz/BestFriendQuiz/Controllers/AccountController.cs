using BestFriendQuiz.Context;
using BestFriendQuiz.EntityLayer.Concrete;
using BestFriendQuiz.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Net;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BestFriendQuiz.Controllers
{

    public class AccountController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if( ModelState.IsValid )
            {

            }
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(GenericViewModel model)
         {
            if( ModelState.IsValid )
            {
                var user = await _signInManager.UserManager.FindByNameAsync(model.Username);

                if( user != null )
                {
                    try
                    {
                        if (user.Password == model.Password)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                        }
                        else
                        {
                            ModelState.AddModelError("Login", "Kullanıcı adı veya şifre yanlış.");
                            return View(model);
                        }
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("Login", "Kullanıcı adı veya şifre yanlış.");
                        return View(model);
                    }
                    
                 
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim("Username", user.UserName),
                            new Claim("RoleId", user.RoleId.ToString())
    };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        Response.Cookies.Append("Username", user.UserName);
                        Response.Cookies.Append("UserId", user.Id.ToString());
                        Response.Cookies.Append("RoleId", user.RoleId.ToString());
                        Response.Cookies.Append("Firstname", user.Firstname);

                        TempData["RoleId"] = user.RoleId;
                        TempData["User"] = user.Firstname;

                        if ( user.RoleId == 2 )
                        {
                            return RedirectToAction("Survey", "Survey");
                        }
                        else if(user.RoleId==1)
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {
                            ModelState.AddModelError("Login", "Kayıt yok.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Login", "Kullanıcı adı veya şifre yanlış.");
                        return View(model);
                    }
                
              
                  ;
               
            }

            return View(model);
        }




        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(GenericViewModel model)
        {
            if ( ModelState.IsValid )
            {
             
                if ( model.Password == model.RePassword )
                {
                    bool agreeToTerms = Request.Form["AgreeToTerms"] == "on"; // "on" değeri, checkbox seçili olduğunda gelir
                    var identityUser =new User();
                    if (agreeToTerms==true)
                    {
                         identityUser = new User
                        {
                            UserName = model.Email,
                            Firstname = model.Firstname,
                            Lastname = model.Lastname,
                            Email = model.Email,
                            Password = model.Password,
                            RoleId = 1,
                            Role = await _roleManager.FindByIdAsync("1")

                        };

                    }
                    else
                    {
                        identityUser = new User
                        {
                            UserName = model.Email,
                            Firstname = model.Firstname,
                            Lastname = model.Lastname,
                            Email = model.Email,
                            Password = model.Password,
                            RoleId = 2,
                            Role = await _roleManager.FindByIdAsync("2")

                        };
                    }
                   
                    
                    var result = await _userManager.CreateAsync(identityUser, model.Password);
                    var result2 = await _userManager.AddToRoleAsync(identityUser,identityUser.Role.Name); 


                    if ( result.Succeeded )
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

        public async Task< IActionResult> Logout()
        {
            HttpContext.Response.Cookies.Delete("Username");
            HttpContext.Response.Cookies.Delete("RoleId");
            HttpContext.Response.Cookies.Delete("Firstname");
            HttpContext.Response.Cookies.Delete("UserId");
          
            await _signInManager.SignOutAsync(); ;
            return RedirectToAction("Index", "Home");
        }



    }
}
