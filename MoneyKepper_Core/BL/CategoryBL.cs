using Models;
using Logic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace MoneyKepper_Core.BL
{
    public static class CategoryBL
    {
        public static void Run(HttpClient client)
        {
            client.BaseAddress = new Uri("http://localhost:63840/api/Category/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static List<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();
            try
            {
                Task task = Task.Run(async () =>
                {
               using (var client = new HttpClient())
               {
                   Run(client);
                   HttpResponseMessage response = await client.GetAsync("GetAllCategories"); // sends GET request
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
            }
            catch (Exception ex)
            {

            }
            return categories;
        }


        public static IList<Category> GetCategoriesByTypes(List<int> types)
        {
            List<Category> categories = new List<Category>();
            try
            {
                Task task = Task.Run(async () =>
                {
                    using (var client = new HttpClient())
                    {
                        Run(client);
                        HttpResponseMessage response = await client.PostAsJsonAsync("GetCategoriesByTypes", types);
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
            }
            catch (Exception ex)
            {

            }
            return categories;
        }

        public static Tuple<bool, Category> CreateNewCategory(Category category)
        {
            var result = new Tuple<bool, Category>(false, null);
            try
            {
                Task task = Task.Run(async () =>
                {
                    using (var client = new HttpClient())
                    {
                        Run(client);
                        HttpResponseMessage response = await client.PostAsJsonAsync("CreateNewCategory", category);
                        string httpResponseBody = "";
                        if (response.IsSuccessStatusCode)
                        {
                            var x = response.RequestMessage.Properties;
                            httpResponseBody = await response.Content.ReadAsStringAsync();
                            var returnedcategory = JsonConvert.DeserializeObject<Category>(httpResponseBody);
                            result = new Tuple<bool, Category>(true, returnedcategory);
                        }
                    }
                });
                task.Wait(); // Wait
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public static bool UpdateCategory(Category category)
        {
            bool result = false;
            try
            {
                Task task = Task.Run(async () =>
                {
                    using (var client = new HttpClient())
                    {
                        Run(client);
                        HttpResponseMessage response = await client.PutAsJsonAsync("UpdateCategory", category);
                        string httpResponseBody = "";
                        if (response.IsSuccessStatusCode)
                        {
                            httpResponseBody = await response.Content.ReadAsStringAsync();
                            result = true;
                        }
                    }
                });
                task.Wait(); // Wait
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public static bool DeleteCategory(int categoryID)
        {
            // return new Logic.CategoryBL().CreateNewCategory(category);
            bool result = false;
            try
            {
                Task task = Task.Run(async () =>
                {
                    using (var client = new HttpClient())
                    {
                        Run(client);
                        HttpResponseMessage response = await client.DeleteAsync($"DeleteCategory/{categoryID}");
                        string httpResponseBody = "";
                        if (response.IsSuccessStatusCode)
                        {
                            httpResponseBody = await response.Content.ReadAsStringAsync();
                            result = true;
                        }
                    }
                });
                task.Wait(); // Wait
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public static bool DeleteCategories(List<int> categoriesID)
        {
            // return new Logic.CategoryBL().CreateNewCategory(category);
            bool result = false;
            try
            {
                Task task = Task.Run(async () =>
                {
                    using (var client = new HttpClient())
                    {
                        Run(client);
                        string querystring = string.Join("&", categoriesID);

                        HttpResponseMessage response = await client.DeleteAsync($"DeleteCategories/{querystring}");
                        string httpResponseBody = "";
                        if (response.IsSuccessStatusCode)
                        {
                            httpResponseBody = await response.Content.ReadAsStringAsync();
                            result = true;
                        }
                    }
                });
                task.Wait(); // Wait
            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}
