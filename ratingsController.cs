using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GateBoys.Helpers;
using GateBoys.Models;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;

namespace GateBoys.Controllers
{
    public class ratingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ratings
        public ActionResult Index()
        {
            return View(db.ratings.ToList());
        }
        
        public ActionResult myRatings()
        {
            return View(db.ratings.ToList());
        }
        // GET: ratings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rating rating = db.ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // GET: ratings/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var email = User.Identity.GetUserName();
            var product = db.Products.FirstOrDefault(a => a.productId == id);
            var orders = db.Orders.Where(a => a.orderedItems.Contains(product.productName) && a.username == email).ToList();
            if (orders == null)
            {
                return RedirectToAction("Error", "Home");
            }
            ViewBag.prodId = product.productId;
            ViewBag.prodName = product.productName;
            return View();
        }

        // POST: ratings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,productId,userRate,comment,ratePic,userEmail,replied,rateDate")] rating rating,HttpPostedFileBase ratePicture)
        {
            var user = User.Identity.GetUserName();
            if (User == null)
            {
                return RedirectToAction("Index", "Main");
            }
            if (rating.userRate == 0)
            {
                ViewBag.prodId = rating.productId;
                ViewBag.prodName =db.Products.FirstOrDefault(a=>a.productId==rating.productId).productName;
                ViewBag.error = "Select Rating";
                return View(rating);
            }
            rating.userEmail = user;
            var product = db.Products.FirstOrDefault(a => a.productId == rating.productId);
            var rate = db.ratings.Where(a => a.productId == rating.productId).ToList();
            if (product == null)
            {
                return RedirectToAction("Index", "Home");
            }
            int perc = 0;
            //it start with 1 because we including the current rate
            int numRatings = 1;
            //it start with users current rate because we including the current rate to get the total rates
            int totalRatings = rating.userRate;
            if (rate.Count > 0 && product.numRatings > 0)
            {
                foreach (var r in rate)
                {
                    totalRatings += r.userRate;
                }
                numRatings += product.numRatings;
                double max = numRatings * 5;
                double percent = ((totalRatings/max)* 100);
                perc =Convert.ToInt32(percent);
            }
            else
            {
                double max = numRatings * 5;
                double percent = ((totalRatings / max) * 100);
                perc = Convert.ToInt32(percent);
            }

            if (ratePicture != null)
            {
                rating.ratePic = uploader.getFileByte(ratePicture);
            }
            if (ModelState.IsValid)
            {
                product.numRatings = numRatings;
                product.perc = perc;
                rating.rateDate = DateTime.Now;

                db.ratings.Add(rating);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            return View(rating);
        }

        public ActionResult GetPic(string id)
        {
            int myId = Convert.ToInt32(id);
            var fileToRetrieve = db.ratings.FirstOrDefault(a => a.id == myId);
            var ContentType = "image/jpg";
            return File(fileToRetrieve.ratePic, ContentType);
        }
        // GET: ratings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rating rating = db.ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: ratings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,productId,userRate,comment,ratePic,userEmail,replied,rateDate")] rating rating)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rating);
        }

        // GET: ratings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rating rating = db.ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            rating rating = db.ratings.Find(id);
            db.ratings.Remove(rating);
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
