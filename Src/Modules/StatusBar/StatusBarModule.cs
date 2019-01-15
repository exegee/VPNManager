using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Regions;
using StatusBar.Views;
using VPNManager.Core;

namespace StatusBar
{
    public class StatusBarModule : ModuleBase 
    {
        public StatusBarModule(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator) : base(container, regionManager, eventAggregator)
        {
        }

        protected override void InitializeModule()
        {
            RegionManager.RegisterViewWithRegion(RegionNames.StatusBarRegion, typeof(StatusBarView));
        }

        protected override void RegisterTypes()
        {
            
        }
    }
}
