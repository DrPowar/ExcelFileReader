using IronXL;
using Server.Constants;
using Server.Models;

namespace Server.Parser.Validation
{
    public static class LogToDataRowParser
    {
        public static void SetId(int index, WorkSheet cells, Guid id)
        {
            cells[$"{LogsColumnToPropertiesConst.Id}{index}"].Value = id.ToString();
        }

        public static void SetNumber(int index, WorkSheet cells, uint number)
        {
            cells[$"{LogsColumnToPropertiesConst.Number}{index}"].Int32Value = (int)number;
        }

        public static void SetOldValue(int index, WorkSheet cells, string oldValue)
        {
            cells[$"{LogsColumnToPropertiesConst.OldValue}{index}"].Value = oldValue;
        }

        public static void SetNewValue(int index, WorkSheet cells, string newValue)
        {
            cells[$"{LogsColumnToPropertiesConst.NewValue}{index}"].Value = newValue;
        }

        public static void SetDate(int index, WorkSheet cells, DateTimeOffset date)
        {
            cells[$"{LogsColumnToPropertiesConst.Date}{index}"].DateTimeValue = date.DateTime;
        }
    }
}
