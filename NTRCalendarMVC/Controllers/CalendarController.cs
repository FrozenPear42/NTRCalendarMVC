using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NTRCalendarMVC.ViewModels;

namespace NTRCalendarMVC.Controllers {
    public class CalendarController : Controller {
        private StorageContext db = new StorageContext();

        // GET: Calendar
        public ActionResult Index(DateTime? firstDay) {
            string userID = (string) Session["UserID"];
            if (userID == null) return RedirectToAction("Index", "Home");
            var user = db.People.FirstOrDefault(p => p.UserID.Equals(userID));
            var weeks = new List<Week>(4);
            var day = firstDay ?? DateTime.Today;
            while (day.DayOfWeek != DayOfWeek.Monday) day = day.AddDays(-1);
            var date = day;

            var appointments = db.Attendances
                .Where(a => a.Person.UserID.Equals(userID))
                .Select(a => a.Appointment)
                .ToList();

            for (var weekNo = 0; weekNo < 4; ++weekNo) {
                var w = new Week {
                    Year = day.Year,
                    Number = new GregorianCalendar().GetWeekOfYear(day, CalendarWeekRule.FirstDay, DayOfWeek.Monday),
                    Days = new List<Day>()
                };

                for (var dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++) {
                    var d = new Day {
                        Date = day,
                        Appointments = new List<Appointment>(
                            appointments
                                .Where(a => a.AppointmentDate.Equals(day))
                                .OrderBy(a => a.StartTime)
                                .ToList())
                    };
                    w.Days.Add(d);
                    day = day.AddDays(1);
                }
                weeks.Add(w);
            }

            var model = new CalendarViewModel {
                FirstDay = date,
                Today = DateTime.Today,
                Weeks = weeks,
                User = $"{user.FirstName} {user.LastName}"
            };

            return View(model);
        }

        public ActionResult Prev(DateTime day) {
            var firstDay = day.AddDays(-7);
            return RedirectToAction("Index", new {firstDay = firstDay});
        }

        public ActionResult Next(DateTime day) {
            var firstDay = day.AddDays(7);
            return RedirectToAction("Index", new {firstDay = firstDay});
        }


        //DETAILS
        public ActionResult Details(Guid? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null) {
                return HttpNotFound();
            }
            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(
            [Bind(Include = "AppointmentID,Title,Description,AppointmentDate,StartTime,EndTime,timestamp")]
            Appointment appointment) {
            if (ModelState.IsValid) {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appointment);
        }

        //CREATE
        public ActionResult Create(DateTime day) {
            var appointment = new Appointment {AppointmentDate = day};
            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "AppointmentID,Title,Description,AppointmentDate,StartTime,EndTime,timestamp")]
            Appointment appointment) {
            string userID = (string) Session["UserID"];
            if (userID == null) return RedirectToAction("Index", "Home");

            if (ModelState.IsValid) {
                appointment.AppointmentID = Guid.NewGuid();
                db.Appointments.Add(appointment);
                var person = db.People.FirstOrDefault(p => p.UserID.Equals(userID));
                if (person != null) {
                    var att = new Attendance {
                        AttendanceID = Guid.NewGuid(),
                        PersonID = person.PersonID,
                        AppointmentID = appointment.AppointmentID,
                    };
                    db.Attendances.Add(att);
                }


                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(appointment);
        }


        public ActionResult Delete(Guid? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null) {
                return HttpNotFound();
            }
            return View(appointment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id) {
            Appointment appointment = db.Appointments.Find(id);
            db.Appointments.Remove(appointment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}