using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using wedding.Models;
using System.Linq;

namespace wedding.Controllers
{
    public class UserController : Controller
    {
        private UserContext _context;
 
        public UserController(UserContext context)
        {
            _context = context;
        }
        //GET: HOME
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.errors = new List<string>();  
            ViewBag.register = TempData["loginerror"];  
            return View();
        }    

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterViewModel anything)
        {
            //if user email exist in db then throw an error asking user to login!!
            User user = _context.users.Where(u => u.email == anything.email).SingleOrDefault();
            if (user != null)
            {
                System.Console.WriteLine("User already exists in my database");
                ViewBag.UserExists = "Account exists. Please create a new account or login instead";
                return View("index");
            }
            if (TryValidateModel(anything)== true)
            {
                //anything that we are sending into our db goes from here!!
                User newUser = new User
                {
                    first_name = anything.first_name,
                    last_name = anything.last_name,
                    email = anything.email,
                    password = anything.password,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now
                };
                _context.Add(newUser);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("currUserID", newUser.userID);
                // ViewBag.currUser = _context.users.SingleOrDefault(u => u.userID == HttpContext.Session.GetInt32("currUserID"));
                return RedirectToAction("Dashboard", "Wedding");                

            }
            else if(TryValidateModel(anything)== false){
                ViewBag.Status = "errors";
                ViewBag.errors = ModelState.Values;
                return View ("Index");
            }
            return View("Index");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login (string email, string password)
        {
           User userInfo = _context.users.Where(u => u.email == email).SingleOrDefault();
            if (userInfo != null){
                if(userInfo.password == password)
                {
                    HttpContext.Session.SetInt32("currUserID", userInfo.userID);
                    ViewBag.ID = HttpContext.Session.GetInt32("currUserID");
                    return RedirectToAction("Dashboard", "Wedding");
                }
            }
            ViewBag.LoginError = "Incorrect email/password! If you have not registered yet then please register frist!";
            return View("Index");
        }   
        [HttpPost]
        [Route("logout")]
        public IActionResult Logout ()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");  
      
        }    

    }
}
