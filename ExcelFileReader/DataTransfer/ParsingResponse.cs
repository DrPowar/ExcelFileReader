using ExcelFileReader.Models;
using System;
using System.Collections.Generic;

namespace ExcelFileReader.DataTransfer
{
    internal record ParsingResponse(List<Person> People, bool IsValid, string Message);
}
