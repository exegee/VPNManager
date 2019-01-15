using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPNManager.Core
{
    public class ViewModelBase : BindableBase
    {
        /// <summary>
        /// Public members to hold instances
        /// </summary>
        protected IRegionManager RegionManager;
        protected IEventAggregator EventAggregator;

        /// <summary>
        /// Constructor that asks for regionManager
        /// </summary>
        /// <param name="regionManager"></param>
        public ViewModelBase(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            RegionManager = regionManager;
            EventAggregator = eventAggregator;
        }
    }
}
