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
    public class TransactionController : ApiController
    {
        //GET: api/cateogry
        [System.Web.Http.HttpGet]
        [Route("api/Transaction/GetAllTransactions")]
        public List<Models.Transaction> GetAllTransactions()
        {
            using (moneyEntities3 context = new moneyEntities3())
            {
                var allTransactions = context.Transactions.ToList();
                return Mapper.Map<List<Models.Transaction>>(allTransactions);
            }
        }

     
        [System.Web.Http.HttpPost]
        [Route("api/Transaction/GetTransactionsByDate")]
        public List<Models.Transaction> GetTransactionsByDate([FromBody] DateTime dateTime)
        {
            using (moneyEntities3 context = new moneyEntities3())
            {
                var allTransactions = context.Transactions.Where(t=>t.Date.Month == dateTime.Month && t.Date.Year == dateTime.Year).ToList();
                return Mapper.Map<List<Models.Transaction>>(allTransactions);
            }
        }

        [HttpPost]
        [Route("api/Transaction/GetTransactionsByTypes")]
        public HttpResponseMessage GetTransactionsByTypes([FromBody] List<int> types)
        {
            using (moneyEntities3 context = new moneyEntities3())
            {
                var test = context.Transactions.Where(tan => types.Contains(tan.Category.TypeID)).ToList();
                var result = Mapper.Map<List<Models.Transaction>>(test);
                return Request.CreateResponse<List<Models.Transaction>>(HttpStatusCode.OK, result);
            }
        }


        [HttpPost]
        [Route("api/Transaction/CreateNewTransaction")]
        public HttpResponseMessage CreateNewTransaction([FromBody] Models.Transaction transaction)
        {
            using (moneyEntities3 context = new moneyEntities3())
            {
                var tan = Mapper.Map<Transaction>(transaction);
                var result = context.Transactions.Add(tan);
                context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }


        [HttpDelete]
        [Route("api/Transaction/DeleteTransaction/{transactionID}")]
        public HttpResponseMessage DeleteTransaction([FromUri] int transactionID)
        {
            moneyEntities3 context = new moneyEntities3();

            var transaction = context.Transactions.FirstOrDefault(t => t.ID == transactionID);
            if (transaction == null)
            {
                context.Dispose();
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            context.Transactions.Remove(transaction);
            context.SaveChanges();
            context.Dispose();
            return Request.CreateResponse(HttpStatusCode.OK);
        }


        [HttpPut]
        [Route("api/Transaction/UpdateTransaction")]
        public HttpResponseMessage UpdateTransaction([FromBody]Models.Transaction transaction)
        {
            moneyEntities3 context = new moneyEntities3();
            var tan = Mapper.Map<Transaction>(transaction);
            var selectTransaction = context.Transactions.FirstOrDefault(t => t.ID == transaction.ID);
            if (selectTransaction != null)
            {
                selectTransaction = tan;
                context.SaveChanges();
            }
            context.Dispose();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
