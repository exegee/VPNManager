using DataLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using VPNManager.Core.Services;

namespace VPNManager
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application
    {
        ExceptionHandlerService handlerService = new ExceptionHandlerService();
        protected override void OnStartup(StartupEventArgs e)
        {
            Application.Current.DispatcherUnhandledException +=
    new DispatcherUnhandledExceptionEventHandler(DispatcherUnhandledExceptionHandler);
            // Create new instance of boostrapper and run
            
            base.OnStartup(e);
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();
            
        }

        private void DispatcherUnhandledExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            handlerService.ShowExceptionWindow2(null, null, null);
            //TODO Add new exception handler - add new custom notification ( view and viewmodel ) in VPNManager and publish exception messages via EventAggregator ?
            //handlerService.ShowExceptionWindow(e.Exception.Message);
            e.Handled = true;
        }
    }
}
