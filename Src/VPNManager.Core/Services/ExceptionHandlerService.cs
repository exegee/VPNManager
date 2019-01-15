using System.Windows;
using System.Windows.Controls;
using VPNManager.Core.Views;

namespace VPNManager.Core.Services
{
    public class ExceptionHandlerService : IExceptionHandlerService
    {
        public void ShowExceptionWindow(string message)
        {
            MessageBox.Show(message, "Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public void ShowExceptionWindow2(string message, string source, string target)
        {
            ExceptionWindow window = new ExceptionWindow();
            window.ShowDialog();
        }
    }
}
