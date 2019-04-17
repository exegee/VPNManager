using Business;
using DataLayer;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VPNManager.Core;

namespace Basic.ViewModels
{
    class EditVpnRemoteHostViewModel : ViewModelBase, IInteractionRequestAware
    {

        private VpnRemoteHost _vpnRemoteHost;

        public VpnRemoteHost VpnRemoteHost
        {
            get { return _vpnRemoteHost; }
            set { SetProperty(ref _vpnRemoteHost, value); }
        }

        private IpAddr _networkDst = new IpAddr();
        public IpAddr NetworkDst
        {
            get { return _networkDst; }
            set { SetProperty(ref _networkDst, value);
                Console.WriteLine(NetworkDst.ASegment);
            }
        }

        private IpAddr _networkMask = new IpAddr();
        public IpAddr NetworkMask
        {
            get { return _networkMask; }
            set { SetProperty(ref _networkMask, value); }
        }

        private IpAddr _gateway = new IpAddr();
        public IpAddr Gateway
        {
            get { return _gateway; }
            set { SetProperty(ref _gateway, value); }
        }

        private string _hostName;
        public string HostName
        {
            get { return _hostName; }
            set { SetProperty(ref _hostName, value); }
        }

        private string _hostDescription;
        public string HostDescription
        {
            get { return _hostDescription; }
            set { SetProperty(ref _hostDescription, value); }
        }

        private bool _updateResult;

        public bool UpdateResult
        {
            get { return _updateResult; }
            set { SetProperty(ref _updateResult, value); }
        }


        public DelegateCommand InitializeCommand { get; set; }
        public DelegateCommand UpdateVpnRemoteHostCommand { get; set; }
        public DelegateCommand CancelUpdateVpnRemoteHostCommand { get; set; }

        public ISqlConnector _sqlConnector;

        public EditVpnRemoteHostViewModel(IRegionManager regionManager,
            IEventAggregator eventAggregator,
            ISqlConnector sqlConnector) : base(regionManager, eventAggregator)
        {
            InitializeCommand = new DelegateCommand(Initialize);
            UpdateVpnRemoteHostCommand = new DelegateCommand(onUpdateVpnRemoteHost);
            CancelUpdateVpnRemoteHostCommand = new DelegateCommand(onCancelUpdateVpnRemoteHost);
            _sqlConnector = sqlConnector;
        }

        private void onCancelUpdateVpnRemoteHost()
        {
            FinishInteraction();
        }

        private void onUpdateVpnRemoteHost()
        {
            VpnRemoteHost.HostName = HostName;
            VpnRemoteHost.HostDescription = HostDescription;
            VpnRemoteHost.NetworkDst = NetworkDst.GetIpAddress();
            VpnRemoteHost.NetworkMask = NetworkMask.GetIpAddress();
            VpnRemoteHost.Gateway = Gateway.GetIpAddress();
            _updateResult = _sqlConnector.UpdateVpnRemoteHost(VpnRemoteHost);
            if (_updateResult)
                FinishInteraction();
        }

        private void Initialize()
        {
            VpnRemoteHost = new VpnRemoteHost();
            VpnRemoteHost = (VpnRemoteHost)Notification.Content;
            _networkDst.SetIpAddress = VpnRemoteHost.NetworkDst;
            _networkMask.SetIpAddress = VpnRemoteHost.NetworkMask;
            _gateway.SetIpAddress = VpnRemoteHost.Gateway;
            _hostName = VpnRemoteHost.HostName;
            _hostDescription = VpnRemoteHost.HostDescription;
            RaisePropertyChanged("NetworkDst");
            RaisePropertyChanged("NetworkMask");
            RaisePropertyChanged("Gateway");
            RaisePropertyChanged("HostName");
            RaisePropertyChanged("HostDescription");
        }

        public INotification Notification { get; set; }
        public Action FinishInteraction { get; set; }
    }
}
