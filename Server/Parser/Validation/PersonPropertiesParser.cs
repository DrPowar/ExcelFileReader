using IronXL;
using Server.Constants;
using Server.Models;
using System.Globalization;

namespace Server.Parser.Validation
{
    public static class PersonPropertiesParser
    {
        public static uint GetNumber(int index, WorkSheet cells) =>
            (uint)cells[$"A{index}"].Int32Value;

        public static uint GetId(int index, WorkSheet cells) =>
            (uint)cells[$"H{index}"].Int32Value;

        public static string GetFirstName(int index, WorkSheet cells) =>
            cells[$"B{index}"].ToString();

        public static string GetLastName(int index, WorkSheet cells) =>
            cells[$"C{index}"].ToString();

        public static Gender GetGender(int index, WorkSheet cells)
        {
            string gender = cells[$"D{index}"].ToString();
            return gender == "Female" ? Gender.Female : Gender.Male;
        }

        public static string GetCountry(int index, WorkSheet cells) =>
            cells[$"E{index}"].ToString();

        public static byte GetAge(int index, WorkSheet cells) =>
            (byte)cells[$"F{index}"].Int32Value;

        public static DateTime GetDate(int index, WorkSheet cells)
        {
            string stringDate = cells[$"G{index}"].ToString();
            if (DateTime.TryParseExact(stringDate, DataFormats.DataTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
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
