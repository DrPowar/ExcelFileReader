using IronXL;
using Server.Constants;
using Server.Models;
using System.Globalization;

namespace Server.Parser.Validation
{
    public static class PersonPropertiesParser
    {
        public static uint GetNumber(int index, WorkSheet cells) =>
            (uint)cells[ColumnToPropertiesConst.Number + index.ToString()].Int32Value;

        public static uint GetId(int index, WorkSheet cells) =>
            (uint)cells[ColumnToPropertiesConst.Id + index.ToString()].Int32Value;

        public static string GetFirstName(int index, WorkSheet cells) =>
            cells[ColumnToPropertiesConst.FistName + index.ToString()].ToString();

        public static string GetLastName(int index, WorkSheet cells) =>
            cells[ColumnToPropertiesConst.LastName + index.ToString()].ToString();

        public static Gender GetGender(int index, WorkSheet cells)
        {
            string gender = cells[ColumnToPropertiesConst.Gender + index.ToString()].ToString();
            return gender == "Female" ? Gender.Female : Gender.Male;
        }

        public static string GetCountry(int index, WorkSheet cells) =>
            cells[ColumnToPropertiesConst.Country + index.ToString()].ToString();

        public static byte GetAge(int index, WorkSheet cells) =>
            (byte)cells[ColumnToPropertiesConst.Age + index.ToString()].Int32Value;

        public static DateTimeOffset GetDate(int index, WorkSheet cells)
        {
            string stringDate = cells[ColumnToPropertiesConst.Date + index.ToString()].ToString();
            if (DateTimeOffset.TryParseExact(stringDate, DataFormats.DataTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset date))
            {
                return date;
            }
            else
            {
                return DateTimeOffset.MinValue;
            }
        }
    }
}
