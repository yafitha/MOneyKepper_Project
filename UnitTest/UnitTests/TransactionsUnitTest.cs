using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyKepperServer.Controllers;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Net.Http;
using MoneyKepperServer;
using System.Collections.Generic;

namespace UnitTest.UnitTests
{
    [TestClass]
    public class TransactionsUnitTest
    {
        [TestMethod]
        public void GetAllTransactions()
        {
            // Arrange
            var controller = new TransactionController();
            this.ArrangeData(controller);

            // Act
            var response = controller.GetAllTransactions();

            // Assert
        }


        [TestMethod]
        public void GetTransactionsByTypes()
        {
            // Arrange
            var controller = new TransactionController();
            this.ArrangeData(controller);

            // Act
            List<int> types = new List<int>();
            types.Add(1);
            var response = controller.GetTransactionsByTypes(types);

            // Assert
        }

        [TestMethod]
        public void GetTransactionsByDate()
        {
            // Arrange
            var controller = new TransactionController();
            this.ArrangeData(controller);

            // Act
            var response = controller.GetTransactionsByDate(DateTime.Now);
            // Assert
        }

        private void ArrangeData(ApiController controller)
        {
            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost:63840/api/Transaction/")
            };
            AutoMapperConfiguration.Configure();
            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
            controller.RequestContext.RouteData = new HttpRouteData(
              route: new HttpRoute(),
              values: new HttpRouteValueDictionary { { "controller", "Transaction" } });
        }
    }
}
