using Basic;
using DataLayer;
using MainMenu;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Unity;
using StatusBar;
using System;
using System.Windows;
using VPNManager.Core.Events;
using VPNManager.ViewModels;
using VPNManager.Views;

namespace VPNManager
{
    public class Bootstrapper : UnityBootstrapper
    {
        private LoginWindow _loginWindow { get; set; }
        private bool _authorize { get; set; }
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }
        protected override void InitializeShell()
        {
            _loginWindow = new LoginWindow();
            _loginWindow.Show();
            _loginWindow.Closed += (s, e) =>
            {
                if (!_authorize)
                {
                    App.Current.Shutdown();
                }
            };
            var dc = _loginWindow.DataContext as LoginWindowViewModel;
            dc._eventAggregator.GetEvent<VPNUserCredentialsEvent>().Subscribe(CheckUserCredentials);
        }

        private void CheckUserCredentials(bool result)
        {
            _authorize = result;
            if (_authorize == true)
            {
                App.Current.MainWindow.Show();
                _loginWindow.Close();
            }
        }

        protected override void ConfigureModuleCatalog()
        {
            Type basicType = typeof(BasicModule);
            ModuleCatalog.AddModule(new ModuleInfo()
            {
                ModuleName = basicType.Name,
                ModuleType = basicType.AssemblyQualifiedName,
                InitializationMode = InitializationMode.WhenAvailable
            });

            Type dataLayerType = typeof(DataLayerModule);
            ModuleCatalog.AddModule(new ModuleInfo()
            {
                ModuleName = dataLayerType.Name,
                ModuleType = dataLayerType.AssemblyQualifiedName,
                InitializationMode = InitializationMode.WhenAvailable
            });

            Type statusBarType = typeof(StatusBarModule);
            ModuleCatalog.AddModule(new ModuleInfo()
            {
                ModuleName = statusBarType.Name,
                ModuleType = statusBarType.AssemblyQualifiedName,
                InitializationMode = InitializationMode.WhenAvailable
            });

            Type menuType = typeof(MainMenuModule);
            ModuleCatalog.AddModule(new ModuleInfo()
            {
                ModuleName = menuType.Name,
                ModuleType = menuType.AssemblyQualifiedName,
                InitializationMode = InitializationMode.WhenAvailable
            });
        }
    }
}
