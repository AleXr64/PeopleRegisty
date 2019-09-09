using IBM.Data.Informix;

namespace DBSetupUtility
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var connString = "Host=127.0.0.1;Server = alx_test; User ID = informix; password = 462753;";
            var conn = new IfxConnection(connString);
            conn.Open();

            var createDbCommand = conn.CreateCommand();
            var selectDbCommand = conn.CreateCommand();
            var createTableCommand = conn.CreateCommand();

            createDbCommand.CommandText = "CREATE DATABASE 'alxr64_PeopleRegistry_Demo';";
            selectDbCommand.CommandText = "DATABASE 'alxr64_PeopleRegistry_Demo';";
            createTableCommand.CommandText = @"CREATE TABLE person (
    id SERIAL not null,
    FirstName VARCHAR(100),
    LastName VARCHAR(100),
    SurName VARCHAR(100),
    BirthDate DATETIME YEAR TO FRACTION NOT NULL DEFAULT
              DATETIME(0001-01-01 17:00:00.000) YEAR TO FRACTION
);";

            createDbCommand.ExecuteNonQuery();
            selectDbCommand.ExecuteNonQuery();
            createTableCommand.ExecuteReader();
        }
    }
}
