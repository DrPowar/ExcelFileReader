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
            if (value is bool boolValue)
            {
                return boolValue
                    ? RGBBrush.GetBrushFromRGB(RGBColors.RGBSuccessRed, RGBColors.RGBSuccessGreen, RGBColors.RGBSuccessBlue)
                    : RGBBrush.GetBrushFromRGB(RGBColors.RGBFailureRed, RGBColors.RGBFailureGreen, RGBColors.RGBFailureBlue);
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
