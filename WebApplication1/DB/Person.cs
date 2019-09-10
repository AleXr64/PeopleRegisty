using System;
using System.Data.Linq.Mapping;

namespace WebApplication1.DB
{
    /// <summary>
    ///     Person Model
    /// </summary>
    [Table(Name = "Person")]
    public class Person
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }
        [Column(DbType = "VARCHAR(100)")]
        public string FirstName { get; set; }
        [Column(DbType = "VARCHAR(100)")]
        public string LastName { get; set; }
        [Column(DbType = "VARCHAR(100)")]
        public string SurName { get; set; }
        [Column(DbType = "DATE")]
        public DateTime BirthDate { get; set; }

    }
}
