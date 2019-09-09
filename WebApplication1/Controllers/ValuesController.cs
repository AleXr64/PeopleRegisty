using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication1.DB;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ValuesController : ApiController
    {
        private DBClient _db = new DBClient();

        // GET api/values
        public async Task<IEnumerable<Person>> Get() { return _db.Persons.Take(5); }

        // GET api/values/5
        public async Task<Person> Get(int id) { return _db.GetById(id); }

        // POST api/values
        public void Post([FromBody] Person value) { _db.Add(value); }

        public List<Person> Search(SearchModel criteria) { return _db.Search(criteria); }
}
}
