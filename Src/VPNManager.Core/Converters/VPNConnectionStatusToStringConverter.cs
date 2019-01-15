using System;
using System.Globalization;
using System.Windows.Data;

namespace VPNManager.Core.Converters
{
    [ValueConversion(typeof(VPNConnectionStatus), typeof(string))]
    public class VPNConnectionStatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = "";
            switch (value)
            {
                case VPNConnectionStatus.Connected:
                    result = "Disconnect";
                    break;
                case VPNConnectionStatus.Disconnected:
                    result = "Connect";
                    break;
                case VPNConnectionStatus.Connecting:
                    result = "Connecting";
                    break;
                case VPNConnectionStatus.Disconnecting:
                    result = "Disconnecting";
                    break;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
