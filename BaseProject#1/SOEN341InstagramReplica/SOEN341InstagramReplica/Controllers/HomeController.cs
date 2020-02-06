using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SOEN341InstagramReplica.Models;

namespace SOEN341InstagramReplica.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /*
         * SignUp
         * 
         * 
         */
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(User model,string  returnUrl)
        {
            SOEN341Entities db = new SOEN341Entities();
            db.Users.Add(model);
            db.SaveChanges();
            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration Successful";
            return RedirectToAction("Index");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}