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

namespace MoneyKepper_Core.BL
{
    class TransactionBL
    {
        public static void Run(HttpClient client)
        {
            client.BaseAddress = new Uri("http://localhost:63840/api/Transaction/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static List<Transaction> GetAllTransactions()
        {
            List<Transaction> transactions = new List<Transaction>();
            Task task = Task.Run(async () =>
            {
                using (var client = new HttpClient())
                {
                    Run(client);
                    HttpResponseMessage response = await client.GetAsync("GetAllTransactions"); // sends GET request
                    string httpResponseBody = "";
                    if (response.IsSuccessStatusCode)
                    {
                        httpResponseBody = await response.Content.ReadAsStringAsync();
                        transactions = JsonConvert.DeserializeObject<List<Transaction>>(httpResponseBody);
                    }
                    return transactions;
                }
            });
            task.Wait(); // Wait
            return transactions;
        }


        public static IList<Transaction> GetTransactionsByTypes(List<int> types)
        {
            List<Transaction> transactions = new List<Transaction>();
            Task task = Task.Run(async () =>
            {
                using (var client = new HttpClient())
                {
                    Run(client);
                    HttpResponseMessage response = await client.PostAsJsonAsync("GetTransactionsByTypes", types);
                    string httpResponseBody = "";
                    if (response.IsSuccessStatusCode)
                    {
                        httpResponseBody = await response.Content.ReadAsStringAsync();
                        transactions = JsonConvert.DeserializeObject<List<Transaction>>(httpResponseBody);
                    }
                    return transactions;
                }
            });
            task.Wait(); // Wait
            return transactions;
        }

        public static IList<Transaction> GetTransactionsByDate(DateTime dateTime)
        {
            List<Transaction> transactions = new List<Transaction>();
            Task task = Task.Run(async () =>
            {
                using (var client = new HttpClient())
                {
                    Run(client);
                    HttpResponseMessage response = await client.PostAsJsonAsync("GetTransactionsByDate", dateTime);
                    string httpResponseBody = "";
                    if (response.IsSuccessStatusCode)
                    {
                        httpResponseBody = await response.Content.ReadAsStringAsync();
                        transactions = JsonConvert.DeserializeObject<List<Transaction>>(httpResponseBody);
                    }
                    return transactions;
                }
            });
            task.Wait(); // Wait
            return transactions;
        }

        public static IList<Transaction> GetTransactionsByDatesAndType(DateTime startDateTime, DateTime endDateTime, int? typeID)
        {
            List<Transaction> transactions = new List<Transaction>();
            Task task = Task.Run(async () =>
            {
                using (var client = new HttpClient())
                {
                    Run(client);
                    HttpResponseMessage response = await client.PostAsJsonAsync("GetTransactionsByDatesAndType", new { startDateTime,endDateTime,typeID});
                    string httpResponseBody = "";
                    if (response.IsSuccessStatusCode)
                    {
                        httpResponseBody = await response.Content.ReadAsStringAsync();
                        transactions = JsonConvert.DeserializeObject<List<Transaction>>(httpResponseBody);
                    }
                    return transactions;
                }
            });
            task.Wait(); // Wait
            return transactions;
        }

        public static bool CreateNewTransaction(Transaction transaction)
        {
            bool result = false;
            Task task = Task.Run(async () =>
            {
                using (var client = new HttpClient())
                {
                    Run(client);
                    HttpResponseMessage response = await client.PostAsJsonAsync("CreateNewTransaction", transaction);
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


        public static bool DeleteTransaction(int transactionID)
        {
            bool result = false;
            Task task = Task.Run(async () =>
            {
                using (var client = new HttpClient())
                {
                    Run(client);
                    HttpResponseMessage response = await client.DeleteAsync($"DeleteTransaction/{transactionID}");
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
