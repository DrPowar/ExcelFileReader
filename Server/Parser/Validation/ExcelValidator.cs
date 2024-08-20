using IronXL;
using Server.Models;

namespace Server.Parser.Validation
{
    internal static class ExcelValidator
    {
        internal static ValidationResult ValidateFile(WorkBook workBook)
        {
            try
            {
                WorkSheet cells = workBook.DefaultWorkSheet;
                List<Person> people = new List<Person>();

                int a = 0;
                for (int i = 2; i < cells.RowCount + 1; i++)
                {
                    if (string.IsNullOrWhiteSpace(cells[$"A{i}"].ToString()) && string.IsNullOrWhiteSpace(cells[$"H{i}"].ToString()))
                    {
                        continue;
                    }

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

                return new ValidationResult(true, people, ParsingResultMessages.Success);
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, null, ParsingResultMessages.ParsingError);
            }
        }
    }
}
