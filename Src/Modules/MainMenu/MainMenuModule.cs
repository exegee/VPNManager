using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainMenu.Views;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Regions;
using VPNManager.Core;

namespace MainMenu
{
    public class MainMenuModule : ModuleBase
    {
        public MainMenuModule(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator) : base(container, regionManager, eventAggregator)
        {
        }

        protected override void InitializeModule()
        {
            RegionManager.RegisterViewWithRegion(RegionNames.MenuRegion, typeof(MainMenuView));
        }

        protected override void RegisterTypes()
        {
        }
    }
}
