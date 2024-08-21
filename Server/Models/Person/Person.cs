using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models.Person
{
    public class Person
    {
        public uint Number { get; init; }
        public uint Id { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public Gender Gender { get; init; }
        public string? Country { get; init; }
        public byte Age { get; init; }
        public DateTime Birthday { get; init; }
        [NotMapped]
        public bool IsValid { get; init; }

        public Person(uint number, uint id, string firstName, string lastName, Gender gender, string country, byte age, DateTime birthday)
        {
            Number = number;
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            Country = country;
            Age = age;
            Birthday = birthday;
            IsValid = UpdateIsValidField();
        }

        private bool UpdateIsValidField()
        {
            return !string.IsNullOrWhiteSpace(FirstName) &&
                      !string.IsNullOrWhiteSpace(LastName) &&
                      !string.IsNullOrWhiteSpace(Country) &&
                      Age > 0 &&
                      Birthday != default;
        }
    }
}
