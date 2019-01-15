using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using VPNManager.Core;

namespace MainMenu.ViewModels
{
    public class AboutNotificationViewModel : ViewModelBase, IInteractionRequestAware
    {
        private string _version;

        public string Version
        {
            get { return _version; }
            set { SetProperty(ref _version, value); }
        }

        private string _copyrights;

        public string Copyrights
        {
            get { return _copyrights; }
            set { SetProperty(ref _copyrights, value); }
        }

        private string _product;

        public string Product
        {
            get { return _product; }
            set { SetProperty(ref _product, value); }
        }


        public AboutNotificationViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            AssemblyCopyrightAttribute copyrightAttr = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
            AssemblyProductAttribute productAttr = assembly.GetCustomAttribute<AssemblyProductAttribute>();
            Product = productAttr.Product.ToString();
            Copyrights = copyrightAttr.Copyright.ToString();
            Version = fvi.FileVersion;
        }

        public INotification Notification { get; set; }
        public Action FinishInteraction { get; set; }
    }
}
