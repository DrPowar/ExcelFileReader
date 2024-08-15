using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExcelFileReader.Models
{
    internal class Person
    {
        public uint Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender Gender { get; set; }
        public string? Country { get; set; }
        public byte Age { get; set; }
        public DateTime Birthday { get; set; }
        [NotMapped]
        public bool IsValid { get; set; }
    }
}
