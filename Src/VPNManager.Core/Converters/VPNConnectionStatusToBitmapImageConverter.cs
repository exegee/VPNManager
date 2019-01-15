using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace VPNManager.Core.Converters
{
    [ValueConversion(typeof(VPNConnectionStatus), typeof(BitmapImage))]
    public class VPNConnectionStatusToBitmapImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bitmapImage = "";
            switch (value)
            {
                case VPNConnectionStatus.Connected:
                    bitmapImage = "Images/green_ball.png";
                    break;
                case VPNConnectionStatus.Disconnected:
                    bitmapImage = "Images/grey_ball.png";
                    break;
                case VPNConnectionStatus.Connecting:
                    bitmapImage = "Images/yellow_ball.png";
                    break;
                case VPNConnectionStatus.Disconnecting:
                    bitmapImage = "Images/red_ball.png";
                    break;
                default:
                    bitmapImage = "Images/grey_ball.png";
                    break;
            }
            return new BitmapImage(new Uri($"/VPNManager.Core;Component/{bitmapImage}", UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
