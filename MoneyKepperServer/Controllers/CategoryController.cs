using System;
using Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using AutoMapper;
using System.Web.Http;

namespace MoneyKepperServer.Controllers
{
    public class CategoryController : ApiController
    {
        //GET: api/cateogry
        [System.Web.Http.HttpGet]
        [Route("api/Category/GetAllCategories")]
        public List<Models.Category> GetAllCategories()
        {
            moneyEntities3 context = new moneyEntities3();
            var result = context.Categories.ToList();
            context.Dispose();
            return Mapper.Map<List<Models.Category>>(result);
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

        [HttpPost]
        [Route("api/Category/GetCategoriesByTypes")]
        public HttpResponseMessage GetCategoriesByTypes([FromBody] List<int> types)
        {
            moneyEntities3 context = new moneyEntities3();
            var test = context.Categories.Where(cat => types.Contains(cat.TypeID)).ToList();
            var result = Mapper.Map<List<Models.Category>>(test);
            context.Dispose();
            return Request.CreateResponse<List<Models.Category>>(HttpStatusCode.OK, result);
        }


        [HttpPost]
        [Route("api/Category/CreateNewCategory")]
        public HttpResponseMessage CreateNewCategory([FromBody] Models.Category category)
        {
            var c = new Models.Category("מתנות",1,"30",true);
            moneyEntities3 context = new moneyEntities3();
            var cat = Mapper.Map<Category>(c);
            var result = context.Categories.Add(cat);
            context.SaveChanges();
            context.Dispose();
            return Request.CreateResponse(HttpStatusCode.OK);
        }


        [System.Web.Http.HttpDelete]
        [Route("api/Category/DeleteCategory/{categoryID}")]
        public HttpResponseMessage DeleteCategory([FromUri] int categoryID)
        {
            moneyEntities3 context = new moneyEntities3();

            var category = context.Categories.FirstOrDefault(c => c.ID == categoryID);
            if (category == null)
            {
                context.Dispose();
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            context.Categories.Remove(category);
            context.SaveChanges();
            context.Dispose();
            return Request.CreateResponse(HttpStatusCode.OK);
        }


        [System.Web.Http.HttpPut]
        [Route("api/Category/UpdateCategory")]
        public HttpResponseMessage UpdateCategory([FromBody]Models.Category category)
        {
            moneyEntities3 context = new moneyEntities3();
            var cat = Mapper.Map<Category>(category);
            var selectCategory = context.Categories.FirstOrDefault(c => c.ID == cat.ID);
            if (selectCategory == null)
            {
                context.Dispose();
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            selectCategory = cat;
            context.SaveChanges();
            context.Dispose();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        //[System.Web.Http.HttpGet]
        //public HttpResponseMessage Get()
        //{
        //    return Request.CreateResponse(HttpStatusCode.OK, new MyClass() { StatusCode = 200, Message = "This Is Get" });
        //}
        //public class MyClass
        //{
        //    public int StatusCode { get; set; }
        //    public string Message { get; set; }
        //}
    }
}
