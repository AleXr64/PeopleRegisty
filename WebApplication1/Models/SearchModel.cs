using System;

namespace WebApplication1.Models
{
    public class SearchModel
    {
        public SearchModel(string firstName, string lastName, string surName, DateTime birthBefore, DateTime birthAfter)
        {
            FirstName = firstName;
            LastName = lastName;
            SurName = surName;
            BirthBefore = birthBefore;
            BirthAfter = birthAfter;
        }
        public string FirstName { get; }
        public string LastName { get; }
        public string SurName { get; }
        public DateTime BirthBefore { get; }
        public DateTime BirthAfter { get; }
        public bool Valid()
        {
            var dateValid = (BirthBefore != DateTime.MinValue) && (BirthAfter != DateTime.MaxValue);
            return FirstName.IsNormalized() && LastName.IsNotEmpty() && SurName.IsNotEmpty() && dateValid;
        }
    }
}
