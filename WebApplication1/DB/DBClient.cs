using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using IBM.Data.Informix;

namespace WebApplication1.DB
{
    public class DBClient
    {
        private const string _connString =
            "Host=127.0.0.1;Server = alx_test; User ID = informix; password = 462753;Database=alxr64_PeopleRegistry_Demo";

        private readonly IfxConnection _conn;

        private DataContext _linq;

        public Table<Person> Persons => _linq.GetTable<Person>();

        public DBClient()
        {
            _conn = new IfxConnection(_connString);
            _linq = new DataContext(_conn);
        }


        public Person GetById(int id)
        {
            return Persons.FirstOrDefault(x => x.Id == id);

        }


        public void Add(Person p)
        {
         Persons.InsertOnSubmit(p);
         _linq.SubmitChanges();
        }
    }
}
