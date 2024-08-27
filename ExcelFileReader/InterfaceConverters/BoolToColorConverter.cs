using Avalonia.Data.Converters;
using Avalonia.Media;
using ExcelFileReader.Constants;
using System;

using System.Globalization;

namespace ExcelFileReader.InterfaceConverters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush success = new SolidColorBrush(Color.Parse(ColorsConst.Success));
            SolidColorBrush error = new SolidColorBrush(Color.Parse(ColorsConst.Error));

            if (value is bool boolValue)
            {
                return boolValue ? success : error;
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
