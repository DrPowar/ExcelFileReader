using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExcelFileReader.Models
{
    internal class Person
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
    }
}
