using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MoneyKepperServer.Controllers
{
    public class BugetController : ApiController
    {
        [System.Web.Http.HttpPost]
        [Route("api/Buget/GetBugetByDatesAndType")]
        public List<Models.Buget> GetBugetByDatesAndType([FromBody] dynamic model)
        {
            DateTime startDateTime = (DateTime)model.startDateTime;
            DateTime endDateTime = (DateTime)model.endDateTime;
            int? typeID = (int?)model.typeID;
            using (money4 context = new money4())
            {
                var allBugets = context.Bugets.Where(t => t.Date >= startDateTime && t.Date <= endDateTime).ToList();
                if (typeID.HasValue)
                {
                    allBugets = allBugets.Where(t => t.Category.TypeID == typeID).ToList();
                }
                return Mapper.Map<List<Models.Buget>>(allBugets);
            }
        }


        [HttpPost]
        [Route("api/Buget/GetBugetByTypes")]
        public HttpResponseMessage GetBugetByTypes([FromBody] List<int> types)
        {
            using (money4 context = new money4())
            {
                var test = context.Bugets.Where(b => types.Contains(b.Category.TypeID)).ToList();
                var result = Mapper.Map<List<Models.Buget>>(test);
                return Request.CreateResponse<List<Models.Buget>>(HttpStatusCode.OK, result);
            }
        }


        [HttpPost]
        [Route("api/Buget/CreateNewBuget")]
        public HttpResponseMessage CreateNewBuget([FromBody] Models.Buget buget)
        {
            using (money4 context = new money4())
            {
                var bugetModel = Mapper.Map<Buget>(buget);
                var result = context.Bugets.Add(bugetModel);
                context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }


        [HttpDelete]
        [Route("api/Buget/DeleteBuget/{bugetID}")]
        public HttpResponseMessage DeleteBuget([FromUri] int bugetID)
        {
            money4 context = new money4();

            var buget = context.Bugets.FirstOrDefault(t => t.ID == bugetID);
            if (buget == null)
            {
                context.Dispose();
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            context.Bugets.Remove(buget);
            context.SaveChanges();
            context.Dispose();
            return Request.CreateResponse(HttpStatusCode.OK);
        }


        [HttpPut]
        [Route("api/Buget/UpdateBuget")]
        public HttpResponseMessage UpdateBuget([FromBody]Models.Buget buget)
        {
            money4 context = new money4();
            var b = Mapper.Map<Buget>(buget);
            var selectbuget = context.Bugets.FirstOrDefault(t => t.ID == buget.ID);
            if (selectbuget != null)
            {
                selectbuget = b;
                context.SaveChanges();
            }
            context.Dispose();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}