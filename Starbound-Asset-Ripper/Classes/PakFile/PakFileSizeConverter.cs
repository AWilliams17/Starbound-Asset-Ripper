using SharpUtils.FileUtils;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Starbound_Asset_Ripper.Classes
{
    public class PakFileSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return FileSizeHelper.GetHumanReadableSize(long.Parse(value.ToString()));
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
