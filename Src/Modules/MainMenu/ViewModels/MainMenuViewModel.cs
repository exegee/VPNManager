using System;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using VPNManager.Core;

namespace MainMenu.ViewModels
{
    public class MainMenuViewModel : ViewModelBase
    {
        public InteractionRequest<INotification> AboutNotificationRequest { get; set; }

        public DelegateCommand AboutCommand { get; set; }

        public MainMenuViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
            AboutCommand = new DelegateCommand(RaiseAboutNotification);
            AboutNotificationRequest = new InteractionRequest<INotification>();
        }

        private void RaiseAboutNotification()
        {
            AboutNotificationRequest.Raise(new Notification { Title = "About VPN Manager" });
        }
    }
}
