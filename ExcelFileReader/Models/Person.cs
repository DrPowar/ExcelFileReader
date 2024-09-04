using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExcelFileReader.Models
{
    internal class Person : ICloneable
    {
        public uint Number { get; set; }
        public uint Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender Gender { get; set; }
        public string? Country { get; set; }
        public byte Age { get; set; }
        public DateTimeOffset Birthday { get; set; }
        public bool IsValid { get; set; }

        public void UpdateIsValidProperty()
        {
            IsValid = UpdateIsValidField();
        }

        private bool UpdateIsValidField()
        {
            return !string.IsNullOrWhiteSpace(FirstName) &&
                      !string.IsNullOrWhiteSpace(LastName) &&
                      !string.IsNullOrWhiteSpace(Country) &&
                      Age > 0 &&
                      Birthday != default(DateTimeOffset);
        }

        public override string ToString()
        {
            return $"Person [Id={Id}, Number={Number}, Name={FirstName} {LastName}, Gender={Gender}, Country={Country}, Age={Age}, Birthday={Birthday:yyyy-MM-dd}, IsValid={IsValid}]";
        }

        public object Clone()
        {
            return new Person
            {
                Number = this.Number,
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Gender = this.Gender,
                Country = this.Country,
                Age = this.Age,
                Birthday = this.Birthday,
                IsValid = this.IsValid
            };
        }
    }
}
