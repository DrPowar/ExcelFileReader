using IronXL;
using Server.Constants;
using Server.Models;
using System.Globalization;

namespace Server.Parser.Validation
{
    public static class PersonPropertiesParser
    {
        public static uint GetNumber(int index, WorkSheet cells) =>
            (uint)cells[PeopleColumnToPropertiesConst.Number + index.ToString()].Int32Value;

        public static uint GetId(int index, WorkSheet cells) =>
            (uint)cells[PeopleColumnToPropertiesConst.Id + index.ToString()].Int32Value;

        public static string GetFirstName(int index, WorkSheet cells) =>
            cells[PeopleColumnToPropertiesConst.FistName + index.ToString()].ToString();

        public static string GetLastName(int index, WorkSheet cells) =>
            cells[PeopleColumnToPropertiesConst.LastName + index.ToString()].ToString();

        public static Gender GetGender(int index, WorkSheet cells)
        {
            string gender = cells[PeopleColumnToPropertiesConst.Gender + index.ToString()].ToString();
            return gender == "Female" ? Gender.Female : Gender.Male;
        }

        public static string GetCountry(int index, WorkSheet cells) =>
            cells[PeopleColumnToPropertiesConst.Country + index.ToString()].ToString();

        public static byte GetAge(int index, WorkSheet cells) =>
            (byte)cells[PeopleColumnToPropertiesConst.Age + index.ToString()].Int32Value;

        public static DateTimeOffset GetDate(int index, WorkSheet cells)
        {
            string stringDate = cells[PeopleColumnToPropertiesConst.Date + index.ToString()].ToString();
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
