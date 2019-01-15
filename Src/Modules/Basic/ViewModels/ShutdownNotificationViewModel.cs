using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business;
using DataLayer;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using VPNManager.Core.Services;

namespace Basic.ViewModels
{
    public class ShutdownNotificationViewModel : BasicViewModel, IInteractionRequestAware
    {
        public VpnRemoteHost ActiveVPNConnection;
        public DelegateCommand InitializeCommand { get; set; }

        public ShutdownNotificationViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IVPNService vPNService, ISqlConnector sqlConnector, IClosingService closingService) : base(regionManager, eventAggregator, vPNService, sqlConnector, closingService)
        {
            InitializeCommand = new DelegateCommand(Initialize);
        }

        private void Initialize()
        {
            ActiveVPNConnection = (VpnRemoteHost)Notification.Content;

        }

        public INotification Notification { get; set; }
        public Action FinishInteraction { get; set; }
    }
}
