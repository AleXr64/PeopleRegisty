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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SurName { get; set; }
        public DateTime BirthDate { get; set; }

    }
}
