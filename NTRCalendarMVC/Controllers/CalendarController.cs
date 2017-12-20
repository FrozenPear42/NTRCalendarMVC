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
        public ActionResult Index(DateTime? baseDate) {
            var weeks = new List<Week>(4);

            var day = baseDate ?? DateTime.Today;

            while (day.DayOfWeek != DayOfWeek.Monday) day = day.AddDays(-1);
            var appointments = db.Appointments.ToList();

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
                                .Reverse()
                                .ToList())
                    };
                    w.Days.Add(d);
                    day = day.AddDays(1);
                }
                weeks.Add(w);
            }

            return View(weeks);
        }

        public ActionResult Prev() {
            return RedirectToAction("Index");
        }

        public ActionResult Next() {
            return RedirectToAction("Index");
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
            if (ModelState.IsValid) {
                appointment.AppointmentID = Guid.NewGuid();
                db.Appointments.Add(appointment);
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