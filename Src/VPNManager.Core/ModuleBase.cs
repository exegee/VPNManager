using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPNManager.Core
{
    public abstract class ModuleBase : IModule
    {

        /// <summary>
        /// Public members to hold instances
        /// </summary>
        protected IUnityContainer Container { get; private set; }
        protected IRegionManager RegionManager { get; private set; }
        protected IEventAggregator EventAggregator { get; private set; }


        /// <summary>
        /// Constructor that asks for container and regionManager parameters
        /// </summary>
        /// <param name="container"></param>
        /// <param name="regionManager"></param>
        public ModuleBase(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            Container = container;
            RegionManager = regionManager;
            EventAggregator = eventAggregator;
        }

        public void Initialize()
        {
            RegisterTypes();
            InitializeModule();
        }

        /// <summary>
        /// Abstract method used for module initialization
        /// </summary>
        protected abstract void InitializeModule();
        /// <summary>
        /// Abstract method used for module registration
        /// </summary>
        protected abstract void RegisterTypes();
    }
}
