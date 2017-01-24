using Models;
using Logic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace MoneyKepper_Core.BL
{
    public static class CategoryBL
    {
        public static List<Category> GetAllCategories()
        {
            return new Logic.CategoryBL().GetAllCategories();
            //var response = "";
            //Task task = Task.Run(async () =>
            //{
            //    using (var client = new HttpClient())
            //    {
            //        response = await client.GetStringAsync(new Uri("http://localhost:63840/api/Category/GetAllCategories")); // sends GET request

            //    }
            //});
            //task.Wait(); // Wait
            //return JsonConvert.DeserializeObject<List<Category>>(response);
        }

        public static IList<Category> GetCategoriesByTypes(List<int> types)
        {
            //return new Logic.CategoryBL().GetCategoriesByTypes(types);
            List<Category> categories = new List<Category>();
            Task task = Task.Run(async () =>
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync("http://localhost:63840/api/Category/GetCategoriesByTypes",types);
                    string httpResponseBody = "";
                    if (response.IsSuccessStatusCode)
                    {
                        httpResponseBody = await response.Content.ReadAsStringAsync();
                        categories = JsonConvert.DeserializeObject<List<Category>>(httpResponseBody);
                    }
                    return categories;
                }
            });
            task.Wait(); // Wait
            return categories;
        }

        public static bool CreateNewCategory(Category category)
        {
            // return new Logic.CategoryBL().CreateNewCategory(category);
            bool result = false;
            Task task = Task.Run(async () =>
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync("http://localhost:63840/api/Category/CreateNewCategory", category);
                    string httpResponseBody = "";
                    if (response.IsSuccessStatusCode)
                    {
                        httpResponseBody = await response.Content.ReadAsStringAsync();
                        result = true;
                    }
                }
            });
            task.Wait(); // Wait
            return result;
        }
    }
}
