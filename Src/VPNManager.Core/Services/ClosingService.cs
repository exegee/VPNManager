using System;
using System.Windows;

namespace VPNManager.Core.Services
{
    public class ClosingService : IClosingService
    {
        public ClosingService()
        {
            Application.Current.MainWindow.Closing += MainWindow_Closing;
        }

        public event EventHandler ApplicationClosing;

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ApplicationClosing.Invoke(this, null);
        }

    }
}
