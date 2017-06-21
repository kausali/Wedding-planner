using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using wedding.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace wedding.Controllers
{
    public class WeddingController : Controller
    {
        private UserContext _context;
 
        public WeddingController(UserContext context)
        {
            _context = context;
        }


        //GET: HOME
        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            // if no one is in the session then user can't access the route
            var mysession = HttpContext.Session.GetInt32("currUserID");
            if (mysession == null){
                TempData["loginerror"] = "Sorry you need to register/login first!!";
                return RedirectToAction("Index", "User");
            }
            ViewBag.Delete = TempData["delete"];
            ViewBag.Status = TempData["status"];
            ViewBag.Rsvp = TempData["Rsvp"];
            ViewBag.remove = TempData["remove"];
            ViewBag.MySession = mysession;

            // List<Wedding> AllWedding = _context.wedding.Where( u => u.userId >0).ToList();
            // ViewBag.weddings = AllWedding;
            List<Guest> allGuests = _context.guests.Where( u => u.guestID >0).ToList();
            ViewBag.guests = allGuests;

           
            List<Wedding> Allweddings = _context.wedding.Include(y => y.WedGuestList).ToList();
            ViewBag.Allweddings = Allweddings;

            return View("Dashboard");
        }  

        //GET: render a page to add new wedding
        [HttpGet]
        [Route("NewWedding")]
        public IActionResult NewWedding()
        {
            var mysession = HttpContext.Session.GetInt32("currUserID");
            if (mysession == null){
                TempData["loginerror"] = "Sorry you need to register/login first!!";
                return RedirectToAction("Index", "User");
            }
            ViewBag.Date = TempData["date"];
            return View("Add");
        }  


        //add wedding to the db
        [HttpPost]
        [Route("add")]
        public IActionResult Add(Wedding newWedding)
        {
            var mysession = HttpContext.Session.GetInt32("currUserID");
            if (mysession == null){
                TempData["loginerror"] = "Sorry you need to register/login first!!";
                return RedirectToAction("Index", "User");
            }
            if (newWedding.date < DateTime.Today){
                TempData["date"] = "Wedding date cannot be from past!";
                return RedirectToAction("NewWedding");
            }
            if (ModelState.IsValid)
            {
                int currUserID = (int)HttpContext.Session.GetInt32("currUserID");
                newWedding.userId = currUserID;
                newWedding.created_at = DateTime.Now;
                newWedding.updated_at = DateTime.Now;
                _context.Add(newWedding);
                _context.SaveChanges();
            
                return RedirectToAction("Dashboard");
            }
            return RedirectToAction("NewWedding");
        }
        [HttpPost]
        [Route("delete/{wedID}")]
        public IActionResult Delete(int wedID)
        {
            var mysession = HttpContext.Session.GetInt32("currUserID");
            if (mysession == null){
                TempData["loginerror"] = "Sorry you need to register/login first!!";
                return RedirectToAction("Index", "User");
            }

            Wedding retrieveWed = _context.wedding.SingleOrDefault(u => u.weddingID == wedID);
            List<Guest> retGuest = _context.guests.Where(Guest => Guest.weddingID == wedID).ToList();
            foreach(var guest in retGuest)
            {
                _context.guests.Remove(guest);
                _context.SaveChanges();
            }
            _context.wedding.Remove(retrieveWed);
            _context.SaveChanges();
            TempData["delete"] = " You have deleted the wedding you planned!";
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [Route("rsvp/{wedID}")]
        public IActionResult Rsvp(int wedID)
        {
            var mysession = HttpContext.Session.GetInt32("currUserID");
            if (mysession == null){
                TempData["loginerror"] = "Sorry you need to register/login first!!";
                return RedirectToAction("Index", "User");
            }
            // TempData["status"] = false;
            int currUserID = (int)HttpContext.Session.GetInt32("currUserID");
            Guest newGuest = new Guest
            {
                weddingID = wedID,
                userID = currUserID,
            };
            _context.Add(newGuest);
            _context.SaveChanges();
            ViewBag.Status = false;
            TempData["Rsvp"] = " You have RSVP'ed yourself!";
            return RedirectToAction("Dashboard");
        }
        [HttpPost]
        [Route("unrsvp/{wedID}")]
        public IActionResult Unrsvp(int wedID)
        {
            var mysession = HttpContext.Session.GetInt32("currUserID");
            if (mysession == null){
                TempData["loginerror"] = "Sorry you need to register/login first!!";
                return RedirectToAction("Index", "User");
            }

            int currUserID = (int)HttpContext.Session.GetInt32("currUserID");
            Guest RemoveGuest = _context.guests.SingleOrDefault(guest => guest.weddingID == wedID && guest.userID == currUserID);
            _context.guests.Remove(RemoveGuest);
            _context.SaveChanges();
            TempData["remove"] = "You have successfully removed yourself from the guest list!";
            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        [Route("Guest/Detail/{wedID}")]
        public IActionResult Detail(int wedID)
        {
            var mysession = HttpContext.Session.GetInt32("currUserID");
            if (mysession == null){
                TempData["loginerror"] = "Sorry you need to register/login first!!";
                return RedirectToAction("Index", "User");
            }
            // int currUserID = (int)Convert.ToInt32(HttpContext.Session.GetInt32("currUserID"));
            Wedding List = _context.wedding.Where(wedding => wedding.weddingID ==wedID)
                                    .Include(wedding => wedding.WedGuestList)
                                    .ThenInclude(guest =>guest.User)
                                    .SingleOrDefault();
            ViewBag.List = List;
            return View("Detail");
        }

    }
}
