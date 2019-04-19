using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ActivityCenter.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ActivityCenter.Controllers
{
    public class AuthenticationController : Controller
    {
        private Context dbContext;
        
        public AuthenticationController(Context context)
        {
            dbContext = context;
        }

        [HttpPost("Register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
            	if(!dbContext.Users.Any(u => u.Email == newUser.Email))
            	{        		
	                PasswordHasher<User> Hasher = new PasswordHasher<User>();
	                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
	                dbContext.Add(newUser);
	                dbContext.SaveChanges();
	                
	                HttpContext.Session.SetInt32("UserId", newUser.UserId);
	                
	                User userInDb = dbContext.Users.FirstOrDefault(u => u.UserId == newUser.UserId);

	                return RedirectToAction("Index", "ACActivities");             
            	}
            	else
            	{
            		ModelState.AddModelError("Email", "An account has already been registered with that email.");
            		return View("~/Views/Home/Index.cshtml");
            	}
            }
            else 
            {
                return View("~/Views/Home/Index.cshtml");
            }
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginUser authenticateUser)
        {
            if(ModelState.IsValid)
            {
                User userInDb = dbContext.Users.FirstOrDefault(u => u.Email == authenticateUser.Email);

                if(userInDb == null){
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("~/Views/Home/Index.cshtml");
                }

                var hasher = new PasswordHasher<LoginUser>();

                var result = hasher.VerifyHashedPassword(authenticateUser, userInDb.Password, authenticateUser.Password);

                if(result == 0) 
                {
                    ModelState.AddModelError("Password", "Incorrect Password");
                    return View("~/Views/Home/Index.cshtml");
                }
                else 
                {
                    HttpContext.Session.SetInt32("UserId", userInDb.UserId);

                    return RedirectToAction("Index", "ACActivities");
                }
            }
            return View();
        }

        [HttpGet("Logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return View("~/Views/Home/Index.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
