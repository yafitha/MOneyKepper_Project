using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class CategoryBL
    {
        public List<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();
            Task task = Task.Run(async () =>
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(new Uri("http://localhost:63840/api/Category/GetAllCategories")); // sends GET request
                    string httpResponseBody = "";
                    if (response.IsSuccessStatusCode)
                    {
                        httpResponseBody = await response.Content.ReadAsStringAsync();
                        categories = JsonConvert.DeserializeObject<List<Category>>(httpResponseBody);
                    }
                    return categories;
                }
            });
            task.Wait(); // Waitm
            return categories;
        }

        //(new Uri("http://localhost:63840/api/Category/GetCategoriesByTypes?$types=types")); // sends GET request
        public IList<Models.Category> GetCategoriesByTypes(List<int> types)
        {
            List<Category> categories = new List<Category>();
            Task task = Task.Run(async () =>
            {
                using (var client = new HttpClient())
                {
                    // string postBody = JsonConvert.SerializeObject(types);
                    // StringContent queryString = new StringContent(postBody);
                    HttpResponseMessage response = await client.GetAsync("http://localhost:63840/api/Category/GetCategoriesByTypes?types=types");
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

        public bool CreateNewCategory(Category category)
        {
            bool result = false;
            Task task = Task.Run(async () =>
            {
                using (var client = new HttpClient())
                {
                    string postBody = JsonConvert.SerializeObject(category);
                    StringContent queryString = new StringContent(postBody);
                    HttpResponseMessage response = await client.PostAsync("http://localhost:63840/api/Category/CreateNewCategory", queryString);
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
