using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CarlosInIt.EntityFramework.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using NTRCalendarMVC;
using NTRCalendarMVC.Controllers;

namespace NTRCalendarMVC.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void OpeningShouldRenderIndex()
        {
            // Arrange
            HomeController controller = new HomeController();
            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);
            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RegisterShouldRenderRegister()
        {
            // Arrange
            HomeController controller = new HomeController();
            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);
            // Act
            ViewResult result = controller.Register() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void OpeningCalendarWithNotExistingUserShouldRedirectToRegisterScreen()
        {
            var dbMock = new DbContextMock<StorageContext>();
            var controller = new HomeController(dbMock.Object);
            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);

            var result =  (RedirectToRouteResult) controller.Index("asd");
            
            Assert.AreEqual("Register", result.RouteValues["action"]);
        }

        [TestMethod]
        public void RegisterShouldAddUser()
        {
            var dbMock = new DbContextMock<StorageContext>();
            var controller = new HomeController(dbMock.Object);
            TestControllerBuilder builder = new TestControllerBuilder();
            builder.InitializeController(controller);
            var user = new Person {
                PersonID = Guid.NewGuid(),
                FirstName = "Ala",
                LastName = "MaKota",
                UserID = "ala"
            };

            var result = (RedirectToRouteResult)controller.Register(user);
            var dbUser = dbMock.Object.People.First(p => p.UserID.Equals("ala")); 
           
            Assert.IsNotNull(dbUser);
        }

    }
}
