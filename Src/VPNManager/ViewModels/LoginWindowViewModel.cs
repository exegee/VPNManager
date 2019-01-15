using System;
using DataLayer;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using VPNManager.Core;
using VPNManager.Core.Events;

namespace VPNManager.ViewModels
{
    public class LoginWindowViewModel : ViewModelBase
    {

        public DelegateCommand LoginCommand { get; set; }
        public IEventAggregator _eventAggregator { get; set; }

        public LoginWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
            _eventAggregator = eventAggregator;
            LoginCommand = new DelegateCommand(CheckUserCredentials);
        }

        private void CheckUserCredentials()
        {
            bool result = false;
            result = SqlConnector.CheckUserCredentials(UserName, UserPassword);
            _eventAggregator.GetEvent<VPNUserCredentialsEvent>().Publish(result);
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private string _userPassword;

        public string UserPassword
        {
            get { return _userPassword; }
            set { SetProperty(ref _userPassword, value); }
        }



    }
}
