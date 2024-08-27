using IronXL;
using Server.Constants;
using Server.Models;
using System.Globalization;

namespace Server.Parser.Validation
{
    public static class PersonToDataRowParser
    {
        public static void SetNumber(int index, WorkSheet cells, uint number)
        {
            cells[$"{ColumnToPropertiesConst.Number}{index}"].Int32Value = (int)number;
        }

        public static void SetId(int index, WorkSheet cells, uint id)
        {
            cells[$"{ColumnToPropertiesConst.Id}{index}"].Int32Value = (int)id;
        }

        public static void SetFirstName(int index, WorkSheet cells, string firstName)
        {
            cells[$"{ColumnToPropertiesConst.FistName}{index}"].Value = firstName;
        }

        public static void SetLastName(int index, WorkSheet cells, string lastName)
        {
            cells[$"{ColumnToPropertiesConst.LastName}{index}"].Value = lastName;
        }

        public static void SetGender(int index, WorkSheet cells, Gender gender)
        {
            cells[$"{ColumnToPropertiesConst.Gender}{index}"].Value = gender == Gender.Female ? "Female" : "Male";
        }

        public static void SetCountry(int index, WorkSheet cells, string country)
        {
            cells[$"{ColumnToPropertiesConst.Country}{index}"].Value = country;
        }

        public static void SetAge(int index, WorkSheet cells, byte age)
        {
            cells[$"{ColumnToPropertiesConst.Age}{index}"].Int32Value = age;
        }

        public static void SetDate(int index, WorkSheet cells, DateTimeOffset date)
        {
            cells[$"{ColumnToPropertiesConst.Date}{index}"].DateTimeValue = date.DateTime;
        }

    }
}
