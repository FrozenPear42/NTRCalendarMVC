using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using CarlosInIt.EntityFramework.Mocks;
using Castle.Core.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using NTRCalendarMVC.Controllers;
using NTRCalendarMVC.ViewModels;
using Rhino.Mocks;

namespace NTRCalendarMVC.Tests.Controllers {
    [TestClass]
    public class CalendarControllerTest {
        [TestMethod]
        public void PrevShouldPrev() {
            var controller = new CalendarController();
            var today = DateTime.Today;

            var result = (RedirectToRouteResult) controller.Prev(today);

            Assert.AreEqual(result.RouteValues["FirstDay"], today.AddDays(-7));
        }

        [TestMethod]
        public void NextShouldNext() {
            var controller = new CalendarController();
            var today = DateTime.Today;

            var result = (RedirectToRouteResult) controller.Prev(today);

            Assert.AreEqual(result.RouteValues["FirstDay"], today.AddDays(-7));
        }

        [TestMethod]
        public void OpeningCalendarWithNoSessionShouldRedirectToLogin() {
            var controller = new CalendarController();
            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);

            var result = (RedirectToRouteResult) controller.Index(null);

            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Home", result.RouteValues["controller"]);
        }

        [TestMethod]
        public void OpeningCalendarWithSessionShouldRenderIndex() {
            var dbMock = new DbContextMock<StorageContext>();
            var controller = new CalendarController(dbMock.Object);
            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);
            controller.Session["UserID"] = "wgruszka";

            var result = (ViewResult) controller.Index(null);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void OpeningAppointmentShouldRenderAppointment() {
            var dbMock = new DbContextMock<StorageContext>();
            var controller = new CalendarController(dbMock.Object);
            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);
            controller.Session["UserID"] = "wgruszka";
            var appointments = new[] {
                new Appointment {
                    AppointmentID = Guid.NewGuid(),
                    Title = "AlaMaKota",
                    Description = "Opis",
                    StartTime = TimeSpan.Zero,
                    EndTime = TimeSpan.Zero,
                    AppointmentDate = DateTime.Today
                }
            };
            dbMock.WithDbSet(a => a.Appointments, appointments,
                (appointment, keys) => appointment.AppointmentID.Equals(keys[0]));

            var result = (ViewResult) controller.Details(appointments[0].AppointmentID);

            Assert.AreEqual("AlaMaKota", ((Appointment) result.Model).Title);
        }

        [TestMethod]
        public void RemovingAppointmentShouldRemove()
        {
            var dbMock = new DbContextMock<StorageContext>();
            var controller = new CalendarController(dbMock.Object);
          
            var appointments = new[] {
                new Appointment {
                    AppointmentID = Guid.NewGuid(),
                    Title = "AlaMaKota",
                    Description = "Opis",
                    StartTime = TimeSpan.Zero,
                    EndTime = TimeSpan.Zero,
                    AppointmentDate = DateTime.Today,
                }
            };
            dbMock.WithDbSet(a => a.Appointments, appointments, (appointment, keys) => appointment.AppointmentID.Equals(keys[0]));

            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);
            controller.Session["UserID"] = "wgruszka";
            
            var result = (RedirectToRouteResult)controller.Delete(appointments[0]);
            
            Console.Out.WriteLine(result.RouteValues["controller"]);
            Console.Out.WriteLine(result.RouteValues["action"]);

            Console.Out.WriteLine(appointments[0].AppointmentID);
            dbMock.Object.Appointments.ForEach(a => Console.Out.WriteLine(a.AppointmentID));

            Assert.AreEqual(false, dbMock.Object.Appointments.Any(a => a.AppointmentID.Equals(appointments[0].AppointmentID)));
        }

    }
}