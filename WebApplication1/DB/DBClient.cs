using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using IBM.Data.Informix;
using WebApplication1.Models;

namespace WebApplication1.DB
{
    public class DBClient
    {
        private const string _connString =
            "Host=127.0.0.1;Server = alx_test; User ID = informix; password = 462753;Database=alxr64_PeopleRegistry_Demo";

        private readonly IfxConnection _conn;

        private readonly DataContext _linq;

        public DBClient()
        {
            _conn = new IfxConnection(_connString);
            _linq = new DataContext(_conn);
        }

        public Table<Person> Persons => _linq.GetTable<Person>();

        public Person GetById(int id) { return Persons.FirstOrDefault(x => x.Id == id); }

        public List<Person> Search(SearchModel criteria)
        {
            return Persons.ByLastName(criteria.LastName)
                          .ByFirstName(criteria.FirstName)
                          .BySurName(criteria.SurName)
                          .ByBirthDate(criteria.BirthAfter, criteria.BirthBefore)
                          .ToList();
        }

        public void Add(Person p)
        {
            Persons.InsertOnSubmit(p);
            _linq.SubmitChanges();
        }

        public void Update(Person p)
        {
            if(!Persons.Any(x=>x.Id == p.Id) || p.Id == 0)
                {
                    Add(p);
                    return;
                }
            var up = Persons.FirstOrDefault(x => x.Id == p.Id);
            up.LastName = p.LastName;
            up.FirstName = p.FirstName;
            up.BirthDate = p.BirthDate;
            _linq.SubmitChanges();
        }
    }

    internal static class PersonHelper
    {
        public static IQueryable<Person> ByFirstName(this IQueryable<Person> q, string name)
        {
            if(string.IsNullOrEmpty(name)) return q;
            return q.Where(x => x.FirstName.StartsWith(name) || x.FirstName == name);
        }

        public static IQueryable<Person> ByLastName(this IQueryable<Person> q, string name)
        {
            if(string.IsNullOrEmpty(name)) return q;
            return q.Where(x => x.LastName.StartsWith(name) || x.LastName == name);
        }

        public static IQueryable<Person> BySurName(this IQueryable<Person> q, string name)
        {
            if(string.IsNullOrEmpty(name)) return q;
            return q.Where(x => x.SurName.StartsWith(name) || x.SurName == name);
        }

        public static IQueryable<Person> ByBirthDate(this IQueryable<Person> q, DateTime min, DateTime max)
        {
            return q.Where(x => min < x.BirthDate && x.BirthDate < max);
        }
    }
}
