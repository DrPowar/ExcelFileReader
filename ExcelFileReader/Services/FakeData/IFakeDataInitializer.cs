using ExcelFileReader.Models;
using System.Collections.Generic;

namespace ExcelFileReader.Services.FakeData
{
    internal interface IFakeDataInitializer
    {
        public List<Person> CreateFakeData(int count);
    }
}
