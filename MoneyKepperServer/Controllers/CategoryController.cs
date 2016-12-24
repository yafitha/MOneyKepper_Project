using System;
using Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MoneyKepperServer.Controllers
{
    public class CategoryController : ApiController
    {
        //GET: api/cateogry
        [System.Web.Http.HttpGet]
        public List<Models.Category> Get1()
        {
            moneyEntities3 context = new moneyEntities3();
            return context.Categories.Where(cat => cat.ID == 1).ToList();
            //SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog =money; Integrated Security = True");

            //Category matchingPerson = new Category();
            //using (SqlConnection myConnection = (con))
            //{
            //    string oString = "Select * from Category where ID=1";
            //    SqlCommand oCmd = new SqlCommand(oString, myConnection);
            //    oCmd.Parameters.AddWithValue("@Fname", "food");
            //    myConnection.Open();
            //    using (SqlDataReader oReader = oCmd.ExecuteReader())
            //    {
            //        //while (oReader.Read())
            //        //{
            //        //    matchingPerson.firstName = oReader["FirstName"].ToString();
            //        //    matchingPerson.lastName = oReader["LastName"].ToString();
            //        //}

            //        myConnection.Close();
            //    }
            //}
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new MyClass() { StatusCode = 200, Message = "This Is Get" });
        }
        public class MyClass
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }
        }
    }
}
