using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NTRCalendarMVC.Controllers {
    public class HomeController : Controller {
        private StorageContext db = new StorageContext();

        public ActionResult Index() {
            SignOut();
            return View();
        }

        [HttpPost]
        public ActionResult Index(string userId) {
            var person = db.People.FirstOrDefault(p => p.UserID.Equals(userId));
            if (person == null)
                return RedirectToAction("Register");

            SignUser(person);
            return RedirectToAction("Index", "Calendar");
        }

        public ActionResult Register() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "PersonID,FirstName,LastName,UserID,timestamp")] Person person)
        {
            if (ModelState.IsValid)
            {
                person.PersonID = Guid.NewGuid();
                db.People.Add(person);
                db.SaveChanges();
                SignUser(person);
                return RedirectToAction("Index", "Calendar");
            }

            return View(person);
        }

        public ActionResult LogOut() {
            SignOut();
            return RedirectToAction("Index");
        }

        private void SignUser(Person person) {
            Session["UserId"] = person.UserID;
        }

        private void SignOut()
        {
            Session.Remove("UserID");
        }

    }
}