using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ActivityCenter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ActivityCenter.Controllers
{
    public class ACActivitiesController : Controller
    {
        private Context dbContext;
        
        public ACActivitiesController(Context context)
        {
            dbContext = context;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("UserId") != null)
            {
                User userInDb = dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));

                IEnumerable<ACActivity> AllACActivities = dbContext.ACActivities
                                                    .Where(a => a.Date > DateTime.Now)
                                                    .Include(a => a.User)
                                                    .Include(a => a.Participants)
                                                    .OrderByDescending(a => a.CreatedAt)
                                                    .ToList();

                ViewBag.AllACActivities = AllACActivities;
                ViewBag.CurrUserId = userInDb.UserId;
                ViewBag.CurrUser = userInDb;

                return View();
            }
            else 
            {
                return View("~/Views/Home/Index.cshtml");
            }   
        }

        [HttpGet("activities/new")]
        public IActionResult New()
        {
            if(HttpContext.Session.GetInt32("UserId") != null)
            {
                return View();
            }
            else
            {
                return View("~/Views/Home/Index.cshtml");
            }
        }

        [HttpPost("AddActivity")]
        public IActionResult AddActivity(ACActivity newActivity)
        {
            
            if(ModelState.IsValid)
            {
                if(newActivity.Date < DateTime.Now)
                {
                    ModelState.AddModelError("Date", "Date Must Be Planned for the Future");
                    return View("New");
                }
                else
                {       
                    int UserID = (int)HttpContext.Session.GetInt32("UserId");

                    User userInDb = dbContext.Users.FirstOrDefault(u => u.UserId == UserID);

                    newActivity.UserId = userInDb.UserId;
                    newActivity.Duration = Request.Form["DurationNum"] + " " + Request.Form["DurationOption"];
                    dbContext.Add(newActivity);
                    dbContext.SaveChanges();      

                    return RedirectToAction("Index");
                }                
            }
            else
            {
                if(newActivity.Date < DateTime.Now)
                {
                    ModelState.AddModelError("Date", "Date Must Be Planned for the Future");
                }                
                return View("New");
            }
        }

        [HttpGet("activities/{activityId}")]
        public IActionResult Show(int activityId)
        {
            User userInDb = dbContext.Users.FirstOrDefault(u => u.UserId == (int)HttpContext.Session.GetInt32("UserId"));

            ACActivity activityInDb = dbContext.ACActivities
                                    .Include(a => a.User)
                                    .Include(a => a.Participants)
                                    .ThenInclude(p => p.User)
                                    .FirstOrDefault(a => a.ACActivityId == activityId);

            ViewBag.ActivityCreator = activityInDb.User;

            if(userInDb != null)
            {
                ViewBag.CurrUserId = userInDb.UserId;
                ViewBag.CurrUser = userInDb;
            }
            else
            {
                ViewBag.CurrUserId = null;
                ViewBag.CurrUser = null;                
            }

            return View(activityInDb);
        }

        [HttpGet("activities/{activityId}/delete")]
        public IActionResult Delete(int activityId)
        {
            ACActivity activityInDb = dbContext.ACActivities.FirstOrDefault(a => a.ACActivityId == activityId);

            dbContext.ACActivities.Remove(activityInDb);

            dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet("activities/{activityId}/join")]
        public IActionResult Join(int activityId)
        {
            User userInDb = dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));

            ACActivity activityInDb = dbContext.ACActivities.FirstOrDefault(a => a.ACActivityId == activityId);

            Join newJoin = new Join(){ UserId = userInDb.UserId, ACActivityId = activityInDb.ACActivityId };

            activityInDb.Participants.Add(newJoin);

            dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet("activities/{activityId}/leave")]
        public IActionResult Leave(int activityId)
        {
            Join joinInDb = dbContext.Joins.FirstOrDefault(j => j.UserId == HttpContext.Session.GetInt32("UserId") && j.ACActivityId == activityId);

            dbContext.Joins.Remove(joinInDb);

            dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }

}
