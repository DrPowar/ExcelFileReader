using IronXL;
using Server.Constants;
using Server.Models.Person;

namespace Server.Parser.Validation
{
    internal static class ExcelValidator
    {
        internal static FileValidatinResult ValidateFile(WorkBook workBook)
        {
            try
            {
                WorkSheet cells = workBook.DefaultWorkSheet;
                List<Person> people = new List<Person>();

                int a = 0;

                Parallel.For(2, cells.RowCount, i =>
                {
                    if (!string.IsNullOrWhiteSpace(cells[$"A{i}"].ToString()) && !string.IsNullOrWhiteSpace(cells[$"H{i}"].ToString()))
                    {
                        people.Add(new Person(
                        PersonPropertiesParser.GetNumber(i, cells),
                        PersonPropertiesParser.GetId(i, cells),
                        PersonPropertiesParser.GetFirstName(i, cells),
                        PersonPropertiesParser.GetLastName(i, cells),
                        PersonPropertiesParser.GetGender(i, cells),
                        PersonPropertiesParser.GetCountry(i, cells),
                        PersonPropertiesParser.GetAge(i, cells),
                        PersonPropertiesParser.GetDate(i, cells)
                        ));
                    }
                });

                return new FileValidatinResult(true, people, ParsingResultMessages.Success);
            }
            catch (Exception ex)
            {
                return new FileValidatinResult(false, null, ParsingResultMessages.ParsingError);
            }
        }

        internal static DataValidationResult ValidateData(List<Person> people)
        {
            try
            {
                WorkBook workBook = WorkBook.Create(ExcelFileFormat.XLSX);
                WorkSheet workSheet = workBook.DefaultWorkSheet;

                workSheet["A1"].Value = "Number";
                workSheet["H1"].Value = "ID";
                workSheet["B1"].Value = "First Name";
                workSheet["C1"].Value = "Last Name";
                workSheet["D1"].Value = "Gender";
                workSheet["E1"].Value = "Country";
                workSheet["F1"].Value = "Age";
                workSheet["G1"].Value = "Date";

                object lockObj = new object();

                Parallel.For(0, people.Count, i =>
                {
                    var person = people[i];
                    int rowIndex = i + 2;

                    lock(lockObj)
                    {
                        PersonToDataRowParser.SetNumber(rowIndex, workSheet, person.Number);
                        PersonToDataRowParser.SetId(rowIndex, workSheet, person.Id);
                        PersonToDataRowParser.SetFirstName(rowIndex, workSheet, person.FirstName);
                        PersonToDataRowParser.SetLastName(rowIndex, workSheet, person.LastName);
                        PersonToDataRowParser.SetGender(rowIndex, workSheet, person.Gender);
                        PersonToDataRowParser.SetCountry(rowIndex, workSheet, person.Country);
                        PersonToDataRowParser.SetAge(rowIndex, workSheet, person.Age);
                        PersonToDataRowParser.SetDate(rowIndex, workSheet, person.Birthday);
                    }
                });




                return new DataValidationResult(true, workBook.ToByteArray(), ParsingResultMessages.Success);
            }
            catch (Exception ex)
            {
                return new DataValidationResult(false, null, ParsingResultMessages.ParsingError);
            }

        }
    }
}
