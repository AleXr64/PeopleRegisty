using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication1.DB;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly DBClient _db = new DBClient();

        // GET api/values
        public async Task<IEnumerable<Person>> Get()
        {
            return await _db.Search(new SearchModel("", "", "", DateTime.MinValue, DateTime.MaxValue));
        }


        // GET api/values/5
        public async Task<Person> Get(int id)
        {
            return await _db.GetById(id);
        }

        // POST api/values
        public void Post([FromBody] Person value) { _db.Update(value); }

        [Route("api/values/search")]
        public async Task<List<Person>> Search(SearchModel criteria) { return await _db.Search(criteria); }
    }
}
