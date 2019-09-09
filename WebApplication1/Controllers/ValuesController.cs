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
        public IEnumerable<Person> Get() { return _db.Persons.Take(5); }

        public IEnumerable<Person> GetAll() { return _db.Persons.ToList();}
        // GET api/values/5
        public Person Get(int id)
        {
            return _db.GetById(id);
        }

        // POST api/values
        public void Post([FromBody] Person value) { _db.Update(value); }

        public List<Person> Search(SearchModel criteria) { return _db.Search(criteria); }
    }
}
