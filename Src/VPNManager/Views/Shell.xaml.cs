using System.Windows;
using VPNManager.Core;

namespace VPNManager.Views
{
    /// <summary>
    /// Logika interakcji dla klasy Shell.xaml
    /// </summary>
    public partial class Shell : Window
    {
        public Shell()
        {
            InitializeComponent();
            MinimizeToTray.Enable(this);
        }
    }
}
