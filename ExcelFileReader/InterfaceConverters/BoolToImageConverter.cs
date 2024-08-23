using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelFileReader.InterfaceConverters
{
    internal class BoolToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isEnabled = (bool)value;
            string imageType = parameter as string;

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string imagePath = imageType switch
            {   
                "Delete" => isEnabled
                    ? $"{baseDirectory}Assets\\DeleteData.png"
                    : $"{baseDirectory}Assets\\DeleteData_Disabled.png",

                "Search" => isEnabled
                    ? $"{baseDirectory}Assets\\Search.png"
                    : throw new ArgumentException("Unknown image type"),

                "Select" => isEnabled
                    ? $"{baseDirectory}Assets\\SelectFile.png"
                    : $"{baseDirectory}Assets\\SelectFile_Disabled.png",

                "Save" => isEnabled
                    ? $"{baseDirectory}Assets\\SaveData.png"
                    : $"{baseDirectory}Assets\\SaveData_Disabled.png",

                "Get" => isEnabled
                    ? $"{baseDirectory}Assets\\GetData.png"
                    : $"{baseDirectory}Assets\\GetData_Disabled.png",

                "Modify" => isEnabled
                    ? $"{baseDirectory}Assets\\EditData.png"
                    : $"{baseDirectory}Assets\\EditData_Disabled.png",

                _ => throw new ArgumentException("Unknown image type"),
            };
            Bitmap bitmap = new Bitmap(imagePath);

            return bitmap;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
