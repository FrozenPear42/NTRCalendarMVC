using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NTRCalendarMVC.Controllers;
using NTRCalendarMVC.ViewModels;

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
        public void NextShouldNext()
        {
            var controller = new CalendarController();
            var today = DateTime.Today;

            var result = (RedirectToRouteResult)controller.Prev(today);

            Assert.AreEqual(result.RouteValues["FirstDay"], today.AddDays(-7));
        }

//        [TestMethod]
//        public void OpeningAppointmentShouldPassID()
//        {
//            var controller = new CalendarController();
//
//
//            var result = (RedirectToRouteResult)controller.Details();
//
//            Assert.AreEqual(result.RouteValues["FirstDay"], today.AddDays(-7));
//        }
    }
}