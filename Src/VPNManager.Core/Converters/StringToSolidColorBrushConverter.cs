using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace VPNManager.Core.Converters
{
    [ValueConversion(typeof(string), typeof(SolidColorBrush))]
    public class StringToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return new SolidColorBrush(Colors.Black);
            }
            string string_value = ((string)value).Remove(((string)value).Length - 2);
            int latency;
            if (Int32.TryParse(string_value, out latency))
            {
                if (latency <= 100)
                {
                    return new SolidColorBrush(Colors.DarkGreen);
                }
                if (latency > 100 & latency < 500)
                {
                    return new SolidColorBrush(Colors.DarkOrange);
                }
                else
                {
                    return new SolidColorBrush(Colors.DarkRed);
                }
            }
            else
            {
                if ((string)value == "Offline")
                {
                    return new SolidColorBrush(Colors.DarkRed);
                }
                else
                {
                    return new SolidColorBrush(Colors.Black);
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
