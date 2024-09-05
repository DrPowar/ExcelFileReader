using ExcelFileReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelFileReader.Services.FakeData
{
    internal class FakeDataInitializer : IFakeDataInitializer
    {
        private static readonly Random _random = new Random();
        private static readonly string[] _firstNames = { "John", "Jane", "Alice", "Bob", "Charlie", "Diana", "Eva", "Frank", "Grace", "Henry" };
        private static readonly string[] _lastNames = { "Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor" };
        private static readonly string[] _countries = { "USA", "Canada", "UK", "Germany", "France", "Italy", "Spain", "Australia", "Japan", "China" };

        public List<Person> CreateFakeData(int count)
        {
            var people = new List<Person>();
            for (int i = 0; i < count; i++)
            {
                people.Add(new Person
                {
                    Number = (uint)i + 1,
                    Id = (uint)_random.Next(1, 10000),
                    FirstName = _firstNames[_random.Next(_firstNames.Length)],
                    LastName = _lastNames[_random.Next(_lastNames.Length)],
                    Gender = (Gender)_random.Next(2),
                    Country = _countries[_random.Next(_countries.Length)],
                    Age = (byte)_random.Next(1, 100),
                    Birthday = DateTimeOffset.Now.AddDays(-_random.Next(600, 30000)),
                    IsValid = false
                });
            }

            foreach (var person in people)
            {
                person.UpdateIsValidProperty();
            }

            return people;
        }
    }
}
