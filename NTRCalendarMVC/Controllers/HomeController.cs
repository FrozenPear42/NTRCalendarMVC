using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace NTRCalendarMVC.Controllers {
    public class HomeController : Controller {

        ILog log = log4net.LogManager.GetLogger(typeof(HomeController).ToString());

        private StorageContext db = new StorageContext();

        public ActionResult Index() {
            SignOut();
            log.Info("New connection");
            return View();
        }

        [HttpPost]
        public ActionResult Index(string userId) {
            var person = db.People.FirstOrDefault(p => p.UserID.Equals(userId));
            if (person == null)
                return RedirectToAction("Register");

            SignUser(person);
            log.InfoFormat("User {0} signed in", userId);
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
                log.InfoFormat("User {0} registered", person.UserID);
                SignUser(person);
                return RedirectToAction("Index", "Calendar");
            }

            return View(person);
        }

        public ActionResult LogOut() {
            SignOut();
            log.InfoFormat("User signed out");
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