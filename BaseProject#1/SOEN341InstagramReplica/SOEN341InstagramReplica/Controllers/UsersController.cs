﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SOEN341InstagramReplica.Models;

namespace SOEN341InstagramReplica.Controllers
{
    public class UsersController : Controller
    {
        private SOEN341Entities db = new SOEN341Entities();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Details/5
        public ActionResult Details2(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAndPosts user = new UserAndPosts();
            user.user = db.Users.Find(id);
            user.posts = (from x in db.UserPosts where x.User_ID == id select x).ToList();
            
            /*
             * There are several situations when it comes to being able to follow
             * and see if you are already following. We also have to consider whether
             * a user is viewing their own page in which case the option to follow
             * or unfollow shouldn't appear as a user can't follow themseves
             */

            //No one is logged in or user is viewing themselves
            if(Session["username"] == null || 
               Session["username"].ToString() == user.user.Username)
            {
                user.following = "invalid";
            }
            //User is viewing another profile and is logged in
            else
            {
                int sessionID = (int) Session["id"];
                //int followingint = db.FollowLists.Where(x => (x.FolloweeID == id) && (x.FollowerID == sessionID)).Select(x => x.ID);

                if ((db.FollowLists.Where(x => (x.FolloweeID == id) && (x.FollowerID == sessionID)).Select(x => x.ID)).FirstOrDefault() != 0)
                {
                    //There is a following between current user and profile of the user they are on,
                    //meaning they should get the option to unfollow
                    user.following = "Unfollow";
                }
                else
                {
                    //They are not following the user profile they are currently on.
                    //meaning they should get the option to follow
                    user.following = "Follow";
                }
            }

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,First_Name,Last_Name,Username,Password,Email,Age,DOB,Date_Joined")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,First_Name,Last_Name,Username,Password,Email,Age,DOB,Date_Joined")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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

        [HttpPost]
        public ActionResult FollowOrUnfollowUser(int newFollowStatus, int loggedInUser, int userProfile)
        {
            //They clicked that they want to follow
            if(newFollowStatus == 1)
            {
                FollowList newEntry = new FollowList();
                newEntry.FollowerID = loggedInUser;
                newEntry.FolloweeID = userProfile;
                db.FollowLists.Add(newEntry);
                db.SaveChanges();
            }
            else //They want to unfollow
            {
                int id = (db.FollowLists.Where(x => (x.FolloweeID == userProfile) && (x.FollowerID == loggedInUser)).Select(x => x.ID)).FirstOrDefault();
                db.FollowLists.Remove(db.FollowLists.Find(id));
                db.SaveChanges();
            }
            UserAndPosts user = new UserAndPosts();
            user.following = "unfollowing";
            return Json(new { user, Status = "Ok", Error = "" });
        }
    }
}
