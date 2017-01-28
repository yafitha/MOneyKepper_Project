using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepper_Core.BL
{
    public static class BaseBL
    {
        public  static async Task RunAsync(HttpClient client)
        {
            // New code:
            client.BaseAddress = new Uri("http://localhost:63840/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
