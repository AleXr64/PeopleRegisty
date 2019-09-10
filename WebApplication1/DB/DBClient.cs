using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBM.Data.Informix;
using WebApplication1.Models;

namespace WebApplication1.DB
{
    public class DBClient
    {
        private const string _connString =
            "Host=127.0.0.1;Server = alx_test; User ID = informix; password = 462753;Database=alxr64_PeopleRegistry_Demo";

        private readonly IfxConnection _conn;

        public DBClient() { _conn = new IfxConnection(_connString); }

        private List<Person> FromDb(DbDataReader reader)
        {
            var result = new List<Person>();

            if(reader.HasRows)
                while(reader.Read())
                    {
                        var firstName = reader["FirstName"];
                        var lastName = reader["LastName"];
                        var surName = reader["SurName"];
                        var birthDate = reader["BirthDate"];
                        var id = reader["id"];
                        var p = new Person();

                        if(firstName != null) p.FirstName = (string) firstName;
                        if(lastName != null) p.LastName = (string) lastName;
                        if(surName != null) p.SurName = (string) surName;
                        if(birthDate != null) p.BirthDate = (DateTime) birthDate;
                        if(id != null) p.Id = (int) id;
                        result.Add(p);
                    }

            return result;
        }

        public async Task<Person> GetById(int id)
        {
            var req = _conn.CreateCommand();
            req.CommandText = $"SELECT * FROM person WHERE id={id};";
            await _conn.OpenAsync();
            var res = await req.ExecuteReaderAsync();
            var p = FromDb(res);
            _conn.Close();

            return p.First();
        }

        public async Task<int> Add(Person p)
        {
            var req = _conn.CreateCommand();
            req.CommandText = "INSERT INTO person (FirstName, LastName, SurName, BirthDate) VALUES ( ?, ?, ?, ?)";
            req.Parameters.Add("FirstName", IfxType.VarChar, 100);
            req.Parameters.Add("LastName", IfxType.VarChar, 100);
            req.Parameters.Add("SurName", IfxType.VarChar, 100);
            req.Parameters.Add("BirthDate", IfxType.DateTime);
            req.Parameters["FirstName"].Value = p.FirstName;
            req.Parameters["LastName"].Value = p.LastName;
            req.Parameters["SurName"].Value = p.SurName;
            req.Parameters["BirthDate"].Value = p.BirthDate;

            await _conn.OpenAsync();

            await req.ExecuteNonQueryAsync();

            req = _conn.CreateCommand();
            req.CommandText = @"SELECT DBINFO( 'sqlca.sqlerrd1' ) FROM systables WHERE tabid = 1;";
            var id = (int) req.ExecuteScalar();

            _conn.Close();
            return id;
        }

        public async Task Update(Person p)
        {
            if(p.Id == 0)
                {
                    await Add(p);
                    return;
                }

            var req = _conn.CreateCommand();
            req.CommandText = $"select count(*) from person where id={p.Id};";
            await _conn.OpenAsync();

            var count = (int) await req.ExecuteScalarAsync();
            _conn.Close();
            if(count == 1)
                {
                    await _conn.OpenAsync();
                    req.CommandText =
                        $"UPDATE people SET FirstName={p.FirstName}, LastName={p.LastName}, SurName={p.SurName}," +
                        $" BirthDate={p.BirthDate.ToSQL()} WHERE id={p.Id};";
                    await req.ExecuteNonQueryAsync();
                    _conn.Close();
                }
            else
                {
                    await Add(p);
                }
        }

        public async Task<List<Person>> Search(SearchModel criteria)
        {
            var req = _conn.CreateCommand();
            var sb = new StringBuilder();
            var needAnd = false;

            sb.Append(!criteria.Valid() ? "SELECT * FROM person;" : "SELECT * FROM person WHERE ");

            if(criteria.FirstName.IsNotEmpty())
                {
                    sb.Append($"FirstName = {criteria.FirstName} ");
                    needAnd = true;
                }

            if(criteria.LastName.IsNotEmpty())
                {
                    if(needAnd)
                        {
                            sb.Append($"AND LastName = {criteria.LastName}");
                        }
                    else
                        {
                            sb.Append($"LastName = {criteria.LastName}");
                            needAnd = true;
                        }
                }

            if(criteria.SurName.IsNotEmpty())
                {
                    if(needAnd)
                        {
                            sb.Append($"AND SurName = {criteria.SurName}");
                        }
                    else
                        {
                            sb.Append($"SurName = {criteria.SurName}");
                            needAnd = true;
                        }
                }

            if(criteria.BirthAfter != DateTime.MaxValue)
                {
                    if(needAnd)
                        {
                            sb.Append($"AND BirthDate > {criteria.BirthAfter.ToSQL()}");
                        }
                    else
                        {
                            sb.Append($"BirthDate > {criteria.BirthAfter.ToSQL()}");
                            needAnd = true;
                        }
                }

            if(criteria.BirthBefore != DateTime.MinValue)
                {
                    if(needAnd)
                        {
                            sb.Append($"AND BirthDate < {criteria.BirthBefore.ToSQL()}");
                        }
                    else
                        {
                            sb.Append($"BirthDate > {criteria.BirthBefore.ToSQL()}");
                            needAnd = true;
                        }
                }

            req.CommandText = sb.ToString();

            await _conn.OpenAsync();
            var res = await req.ExecuteReaderAsync();

            var result = FromDb(res);

            _conn.Close();

            return result;
        }
    }
}
