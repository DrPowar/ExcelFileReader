using IronXL;
using Microsoft.AspNetCore.Http;
using Microsoft.SqlServer.Server;
using Server.Models;
using System.Globalization;
using Server.Constants;

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
                    people.Add(new Person(
                    (uint)cells[$"A{i}"].Int32Value,
                    (uint)cells[$"H{i}"].Int32Value,
                    cells[$"B{i}"].ToString(),
                    cells[$"C{i}"].ToString(),
                    (Gender)cells[$"D{i}"].Int32Value,
                    cells[$"E{i}"].ToString(),
                    (byte)cells[$"F{i}"].Int32Value,
                    ParseDate(cells[$"G{i}"].ToString())
                    ));
                }

                return new ValidationResult(true, people, ParsingResultMessages.Success);
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, null, ParsingResultMessages.ParsingError);
            }
        }

        private static DateTime ParseDate(string stringDate)
        {
            if(DateTime.TryParseExact(stringDate, DataFormats.DataTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }
            else
            {
                return DateTime.MinValue;
            }
        }
    }
}
