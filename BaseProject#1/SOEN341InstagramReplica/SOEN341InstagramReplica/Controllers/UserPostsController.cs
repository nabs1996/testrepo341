using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SOEN341InstagramReplica.Models;

namespace SOEN341InstagramReplica.Controllers
{
    public class UserPostsController : Controller
    {
        private SOEN341Entities db = new SOEN341Entities();
        
        // GET: UserPosts
        public ActionResult Index()
        {
            var userPosts = db.UserPosts.Include(u => u.User);
            return View(userPosts.ToList());
        }

        // GET: UserPosts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPost userPost = db.UserPosts.Find(id);
            if (userPost == null)
            {
                return HttpNotFound();
            }
            return View(userPost);
        }

        // GET: UserPosts/Details/5
        public ActionResult Details2(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPost userPost = db.UserPosts.Find(id);
            if (userPost == null)
            {
                return HttpNotFound();
            }
            ViewBag.userPost = userPost;
            ViewBag.comments = (List<Comment>)(from x in db.Comments where x.Post_ID == userPost.ID select x).ToList();
            //ViewData["userPost"] = userPost.Title;
            //ViewData["comments"] = (from x in db.Comments where x.Post_ID == userPost.ID select x).ToList();
            //UserPostsAndComments xmodel = new UserPostsAndComments();
            //xmodel.currentUserPost = userPost;
            //xmodel.currentUserPostComments = (List<Comment>) (from x in db.Comments where x.Post_ID == userPost.ID select x);

            //return View(xmodel);

            ////////////////
            return View();
        }

        // GET: UserPosts/Create
        public ActionResult Create()
        {
            ViewBag.User_ID = new SelectList(db.Users, "ID", "First_Name");
            return View();
        }

        // POST: UserPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Description,POST,Rating,Date_Posted,User_ID")] UserPost userPost, HttpPostedFileBase image)
        {
            userPost.User_ID = (int) Session["id"];
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    userPost.POST = new byte[image.ContentLength];
                    image.InputStream.Read(userPost.POST, 0, image.ContentLength);
                }
                db.UserPosts.Add(userPost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.User_ID = new SelectList(db.Users, "ID", "First_Name", userPost.User_ID);
            return View(userPost);
        }

        // GET: UserPosts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPost userPost = db.UserPosts.Find(id);
            if (userPost == null)
            {
                return HttpNotFound();
            }
            ViewBag.User_ID = new SelectList(db.Users, "ID", "First_Name", userPost.User_ID);
            return View(userPost);
        }

        // POST: UserPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Description,POST,Rating,Date_Posted,User_ID")] UserPost userPost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.User_ID = new SelectList(db.Users, "ID", "First_Name", userPost.User_ID);
            return View(userPost);
        }

        // GET: UserPosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserPost userPost = db.UserPosts.Find(id);
            if (userPost == null)
            {
                return HttpNotFound();
            }
            return View(userPost);
        }

        // POST: UserPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserPost userPost = db.UserPosts.Find(id);
            db.UserPosts.Remove(userPost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
