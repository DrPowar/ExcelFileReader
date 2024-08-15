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
                    people.Add(new Person(
                        (uint)cells[$"H{i}"].Int32Value,
                        cells[$"B{i}"].ToString(),
                        cells[$"C{i}"].ToString(),
                        (Gender)cells[$"D{i}"].Int32Value,
                        cells[$"E{i}"].ToString(),
                        (byte)cells[$"F{i}"].Int32Value,
                        cells[$"G{i}"].DateTimeValue ?? DateTime.MinValue
                    ));

                }

                return new ValidationResult(true, people);
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, null);
            }
        }
    }
}
