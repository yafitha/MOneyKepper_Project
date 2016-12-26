using Models;
using Logic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace MoneyKepper_Core.BL
{
    public static class CategoryBL
    {
        public static List<Category> GetAllCategories()
        {
           return new Logic.CategoryBL().GetAllCategories2();
            //var response = "";
            //Task task = Task.Run(async () =>
            //{
            //    using (var client = new HttpClient())
            //    {
            //        response = await client.GetStringAsync(new Uri("http://localhost:63840/api/Category/Get1")); // sends GET request

            //    }
            //});
            //task.Wait(); // Wait
            //return JsonConvert.DeserializeObject<List<Category>>(response);
        }
    }
}
