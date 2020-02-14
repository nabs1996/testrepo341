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
        public ActionResult SignUp(User model, string returnUrl)
        {
            SOEN341Entities db = new SOEN341Entities();

            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration Successful";
            Session["username"] = model.First_Name;
            return RedirectToAction("Index");
            return View();
        }

        public ActionResult SignOut()
        {
            Session.Remove("username");
            return RedirectToAction("Index", "Home");
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

        public ActionResult Login()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Login(User model, string returnUrl)
        {
            SOEN341Entities db = new SOEN341Entities();
            string pass = (from x in db.Users where x.Username == model.Username select x.Password).ToString();
            if (pass == model.Password)
            {
                Session["username"] = model.Username;
                Session["id"] = (from x in db.Users where x.Username == model.Username select x.ID);
                //return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }
    }
}