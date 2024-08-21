using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelFileReader.InterfaceConverters
{
    internal class DateOnlyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTimeOffset dateTimeOffset)
            {
                return new DateTimeOffset(dateTimeOffset.Date, dateTimeOffset.Offset);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTimeOffset dateTimeOffset)
            {
                return new DateTimeOffset(dateTimeOffset.Date, dateTimeOffset.Offset);
            }
            return value;
        }
    }
}
