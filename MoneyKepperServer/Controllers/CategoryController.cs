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
            using (money4 context = new money4())
            {
                var result = context.Categories.ToList();
                return Mapper.Map<List<Models.Category>>(result);
            }
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
            using (money4 context = new money4())
            {
                var test = context.Categories.Where(cat => types.Contains(cat.TypeID)).ToList();
                var result = Mapper.Map<List<Models.Category>>(test);
                return Request.CreateResponse<List<Models.Category>>(HttpStatusCode.OK, result);
            }
        }


        [HttpPost]
        [Route("api/Category/CreateNewCategory")]
        public HttpResponseMessage CreateNewCategory([FromBody] Models.Category category)
        {
            using (money4 context = new money4())
            {
                var cat = Mapper.Map<Category>(category);
                var result = context.Categories.Add(cat);
                context.SaveChanges();
                var respone = new HttpRequestMessage();
                respone.Properties.Add("cat", category);
                return Request.CreateResponse(respone);
            }
        }


        [System.Web.Http.HttpDelete]
        [Route("api/Category/DeleteCategory/{categoryID}")]
        public HttpResponseMessage DeleteCategory([FromUri] int categoryID)
        {
            using (money4 context = new money4())
            {
                var category = context.Categories.FirstOrDefault(c => c.ID == categoryID);
                if (category == null)
                {
                    context.Dispose();
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                context.Transactions.RemoveRange(context.Transactions.Where(t => t.Category.ID == categoryID));
                context.Bugets.RemoveRange(context.Bugets.Where(t => t.Category.ID == categoryID));
                var categories = context.Categories.Where(c => c.ParentID == categoryID);
                foreach (var cat in categories)
                {
                    context.Categories.Remove(category);
                    context.Transactions.RemoveRange(context.Transactions.Where(t => t.Category.ID == cat.ID));
                    context.Bugets.RemoveRange(context.Bugets.Where(t => t.Category.ID == cat.ID));
                }
                if (categories != null)
                {
                    context.Categories.RemoveRange(categories);
                }
                context.Categories.Remove(category);
                context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }

        [System.Web.Http.HttpDelete]
        [Route("api/Category/DeleteCategories/{ids}")]
        public HttpResponseMessage DeleteCategories([FromUri]IEnumerable<int> ids)
        {
            using (money4 context = new money4())
            {
                foreach (var id in ids)
                {
                    var category = context.Categories.FirstOrDefault(c => c.ID == id);
                    if (category == null)
                    {
                        context.Dispose();
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                    }
                    if (category.IsParent)
                    {
                        var deletedCategories = context.Categories.Where(c => c.ParentID == id);
                        if (deletedCategories != null)
                        {
                            context.Categories.RemoveRange(deletedCategories);
                        }
                    }
                    context.Categories.Remove(category);
                    context.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }


        [System.Web.Http.HttpPut]
        [Route("api/Category/UpdateCategory")]
        public HttpResponseMessage UpdateCategory([FromBody]Models.Category category)
        {
            using (money4 context = new money4())
            {
                var cat = Mapper.Map<Category>(category);
                var selectCategory = context.Categories.Find(cat.ID);
                if (selectCategory == null)
                {
                    context.Dispose();
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                //selectCategory.Name = category.Name;
                //selectCategory.PictureName = cat.PictureName;
                context.Entry(selectCategory).CurrentValues.SetValues(cat);
                context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
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
