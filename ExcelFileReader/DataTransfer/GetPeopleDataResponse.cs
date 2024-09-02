using ExcelFileReader.Models;
using System.Collections.Generic;

namespace ExcelFileReader.DataTransfer
{
    internal record GetPeopleDataResponse(List<Person> People, string Message, bool Result);
}
