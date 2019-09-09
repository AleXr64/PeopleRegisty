using System;

namespace WebApplication1.DB
{
    /// <summary>
    ///     Person Model
    /// </summary>
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SurName { get; set; }
        public DateTime BirthDate { get; set; }

    }
}
