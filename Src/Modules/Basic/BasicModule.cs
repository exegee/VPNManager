using Basic.Views;
using DataLayer;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Regions;
using Prism.Unity;
using VPNManager.Core;
using VPNManager.Core.Services;

namespace Basic
{
    public class BasicModule : ModuleBase
    {
        public BasicModule(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator) : base(container, regionManager, eventAggregator)
        {
        }

        protected override void InitializeModule()
        {
            RegionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(BasicView));
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<BasicView>();
            Container.RegisterType<IVPNService, VPNService>();
            Container.RegisterType<ISqlConnector, SqlConnector>();
            Container.RegisterType<IClosingService, ClosingService>();
        }
    }
}
