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
                    ? $"{baseDirectory}Assets\\Delete.png"
                    : $"{baseDirectory}Assets\\DeleteDisabled.png",

                "Search" => isEnabled
                    ? $"{baseDirectory}Assets\\Search.png"
                    : throw new ArgumentException("Unknown image type"),

                "Select" => isEnabled
                    ? $"{baseDirectory}Assets\\Open.png"
                    : $"{baseDirectory}Assets\\OpenDisabled.png",

                "Save" => isEnabled
                    ? $"{baseDirectory}Assets\\Save.png"
                    : $"{baseDirectory}Assets\\SaveDisabled.png",

                "Get" => isEnabled
                    ? $"{baseDirectory}Assets\\Download.png"
                    : $"{baseDirectory}Assets\\DownloadDisabled.png",

                "Modify" => isEnabled
                    ? $"{baseDirectory}Assets\\Modify.png"
                    : $"{baseDirectory}Assets\\ModifyDisabled.png",

                "GetLogs" => isEnabled
                    ? $"{baseDirectory}Assets\\GetLogs.png"
                    : $"{baseDirectory}Assets\\GetLogsDisabled.png",

                "RemoveLog" => isEnabled
                    ? $"{baseDirectory}Assets\\RemoveLog.png"
                    : $"{baseDirectory}Assets\\RemoveLogDisabled.png",

                "Upload" => isEnabled
                    ? $"{baseDirectory}Assets\\Upload.png"
                    : $"{baseDirectory}Assets\\UploadDisabled.png",

                "AddPerson" => isEnabled
                    ? $"{baseDirectory}Assets\\AddPerson.png"
                    : $"{baseDirectory}Assets\\AddPersonDisabled.png",

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
