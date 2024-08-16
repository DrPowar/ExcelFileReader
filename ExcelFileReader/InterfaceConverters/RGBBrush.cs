using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelFileReader.InterfaceConverters
{
    internal static class RGBBrush
    {
        internal static Brush GetBrushFromRGB(byte red, byte green, byte blue)
        {
            return new SolidColorBrush(Color.FromRgb(red, green, blue));
        }
    }
}
