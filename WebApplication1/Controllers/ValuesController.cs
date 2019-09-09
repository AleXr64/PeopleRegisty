using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication1.DB;

namespace WebApplication1.Controllers
{
    public class ValuesController : ApiController
    {
        private DBClient _db = new DBClient();
        // GET api/values
        public async Task<IEnumerable<string>> Get()
        {
            var p = new Person();
            p.LastName = "Last";
            p.FirstName = "First";
            p.BirthDate = DateTime.Today;
            p.SurName = "Sur";
           await _db.Add(p);
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
