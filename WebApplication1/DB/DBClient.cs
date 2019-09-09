using System;
using System.Data.Common;
using System.Threading.Tasks;
using IBM.Data.Informix;

namespace WebApplication1.DB
{
    public class DBClient
    {
        private const string _connString = "Host=127.0.0.1;Server = alx_test; User ID = informix; password = 462753;Database=alxr64_PeopleRegistry_Demo";
        private readonly IfxConnection _conn;

        public DBClient() { _conn = new IfxConnection(_connString); }

        private Person FromDb(DbDataReader reader)
        {
            Person p;

            if(reader.HasRows)
                {
                    var firstName = reader["FirstName"];
                    var lastName = reader["LastName"];
                    var surName = reader["SurName"];
                    var birthDate = reader["BirthDate"];
                    var id = reader["id"];
                    p = new Person();

                    if(firstName != null) p.FirstName = (string) firstName;
                    if(lastName != null) p.LastName = (string) lastName;
                    if(surName != null) p.SurName = (string) surName;
                    if(birthDate != null) p.BirthDate = (DateTime) birthDate;
                    if(id != null) p.Id = (int) id;
                }
            else
                {
                    p = new Person();
                }

            return p;
        }

        public async Task<Person> GetById(int id)
        {
            var req = _conn.CreateCommand();
            req.CommandText = @"SELECT * FROM person WHERE id={id};";
            await _conn.OpenAsync();
            var res = await req.ExecuteReaderAsync();
            var p = FromDb(res);
            _conn.Close();
            
            return p;
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
           /// req.Prepare();
            await _conn.OpenAsync();
            var id = await req.ExecuteNonQueryAsync();
            _conn.Close();
            return id;
        }
    }
}
