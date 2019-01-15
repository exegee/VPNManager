using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Regions;
using System;
using VPNManager.Core;

namespace DataLayer
{
    public class DataLayerModule : ModuleBase
    {
        public DataLayerModule(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator) : base(container, regionManager, eventAggregator)
        {
        }

        protected override void InitializeModule()
        {
            
        }

        protected override void RegisterTypes()
        {
           
        }
    }
}
