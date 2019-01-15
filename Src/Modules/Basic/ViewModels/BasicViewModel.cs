using Business;
using DataLayer;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Timers;
using VPNManager.Core;
using VPNManager.Core.Events;
using VPNManager.Core.Services;

namespace Basic.ViewModels
{
    public class BasicViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public DateTime start;
        private System.Timers.Timer DelayTimer;
        public string pingtime;
        private string _status;
        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        private VpnRemoteHost _activeVPNConnection;

        public VpnRemoteHost ActiveVPNConnection
        {
            get { return _activeVPNConnection; }
            set
            {
                SetProperty(ref _activeVPNConnection, value);
                UpdateStatusBarActiveConnectionMessage(value == null ? "Not connected" : value.HostName);
            }
        }
        private bool _isBusy = true;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        public DelegateCommand<object> VPNHostCommand { get; set; }
        public DelegateCommand BasicViewLoadedCommand { get; set; }

        private ObservableCollection<VpnRemoteHost> _vpnRemoteHosts;
        public ObservableCollection<VpnRemoteHost> VpnRemoteHosts
        {
            get { return _vpnRemoteHosts; }
            set { SetProperty(ref _vpnRemoteHosts, value); }
        }

        #region Services
        public static ISqlConnector _sqlConnector;
        public IVPNService _vPNService;
        public IClosingService _closingService;
        #endregion

        public InteractionRequest<INotification> ShutdownRequest { get; set; }
        public BasicViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, 
            IVPNService vPNService, ISqlConnector sqlConnector, IClosingService closingService) : base(regionManager, eventAggregator)
        {
            VPNHostCommand = new DelegateCommand<object>(VPNHostAction);
            BasicViewLoadedCommand = new DelegateCommand(OnViewLoad);
             _vPNService = vPNService;
            _sqlConnector = sqlConnector;
            //_sqlConnector.EncryptConnectionString();
            _closingService = closingService;
            ShutdownRequest = new InteractionRequest<INotification>();
            _vPNService.StatusChanged += _vPNService_StatusChanged;
            _vPNService.Connected += _vPNService_Connected;
            _vPNService.Disconnected += _vPNService_Disconnected;
            _closingService.ApplicationClosing += _closingService_ApplicationClosing;
        }

        // Called when BasicView load event is fired
        private void OnViewLoad()
        {
            LoadVPNHosts();
            InitConnection();
            InitDelayTimer(1000);
        }

        private void _closingService_ApplicationClosing(object sender, EventArgs e)
        {
            //ShutdownRequest.Raise(new Notification { Title = "VPN Manager closing", Content = ActiveVPNConnection });
            UpdateStatusBarMessage("Closing active connections and clearing resources...");
            _vPNService.Disconnect();
        }


        private void LoadVPNHosts()
        {
            Task.Run(async () =>
            {
                await Task.Run(() =>
                {
                    VpnRemoteHosts = new ObservableCollection<VpnRemoteHost>(_sqlConnector.GetRemoteHosts());
                });
                IsBusy = false;
            });
        }

        private void CurrentDispatcher_ShutdownStarted(object sender, EventArgs e)
        {
            
            _vPNService.Disconnect();
        }

        private void _vPNService_Disconnected(object sender, EventArgs e)
        {
            VpnRemoteHosts[VpnRemoteHosts.IndexOf(ActiveVPNConnection)].Status = VPNConnectionStatus.Disconnected;
            VpnRemoteHosts[VpnRemoteHosts.IndexOf(ActiveVPNConnection)].Latency = "n/a";
            ActiveVPNConnection = null;
            foreach (VpnRemoteHost vpnRemoteHost in VpnRemoteHosts)
            {
                vpnRemoteHost.IsNoOtherConnectionActive = true;
            }
        }

        private void _vPNService_Connected(object sender, EventArgs e)
        {
            VpnRemoteHosts[VpnRemoteHosts.IndexOf(ActiveVPNConnection)].Status = VPNConnectionStatus.Connected;
            foreach(VpnRemoteHost vpnRemoteHost in VpnRemoteHosts.Where(s => s != ActiveVPNConnection))
            {
                vpnRemoteHost.IsNoOtherConnectionActive = false;
            }
            UpdateStatusBarInterfaceMessage(_vPNService.InterfaceName);
        }

        private void VPNHostAction(object obj)
        {
            if (obj == null) return;
            VpnRemoteHost vpnRemoteHost = obj as VpnRemoteHost;

            if (vpnRemoteHost.Status != VPNConnectionStatus.Connected)
            {
                ActiveVPNConnection = vpnRemoteHost;
                VpnRemoteHosts[VpnRemoteHosts.IndexOf(ActiveVPNConnection)].Status = VPNConnectionStatus.Connecting;
                _vPNService.RouteNetworkDst = ActiveVPNConnection.NetworkDst;
                _vPNService.RouteNetworkMask = ActiveVPNConnection.NetworkMask;
                _vPNService.RouteGateway = ActiveVPNConnection.Gateway;
                _vPNService.Connect();
                foreach (VpnRemoteHost remoteHost in VpnRemoteHosts.Where(s => s != ActiveVPNConnection))
                {
                    remoteHost.IsNoOtherConnectionActive = false;
                }
            }
            else
            {
                VpnRemoteHosts[VpnRemoteHosts.IndexOf(ActiveVPNConnection)].Status = VPNConnectionStatus.Disconnecting;
                _vPNService.Disconnect();
            }

        }

        void InitConnection()
        {
            _vPNService.InterfaceName = "Stolarczyk";
            _vPNService.ServerIPAddress = "217.117.139.45";
            _vPNService.PreSharedKey = "p47:{L=*L#B@e6SG";
            _vPNService.UserName = "PawelZ";
            //_vPNService.UserName = "AndrzejK";
            //_vPNService.UserPassword = "+smFuWFtK*8Z(R8k";
            //_vPNService.UserPassword = "y5:hz//4sN:V+Ah+";
            _vPNService.UserPassword = "123456";
        }

        private void _vPNService_StatusChanged(object sender, EventArgs e)
        {
            Status = _vPNService.Status;
            UpdateStatusBarMessage(_vPNService.Status);
        }

        private Task CheckHostLatency(string ipAddress)
        {
            return Task.Factory.StartNew(() =>
            {
                var ping = new Ping();
                if (VpnRemoteHosts[VpnRemoteHosts.IndexOf(ActiveVPNConnection)].Status == VPNConnectionStatus.Connected)
                {
                    PingOptions pingOptions = new PingOptions(64, true);
                    start = DateTime.Now;
                    PingReply pingReply = ping.Send(ipAddress, 100);
                    if (pingReply.Status == IPStatus.Success)
                    {
                        pingtime = Math.Floor((DateTime.Now - start).TotalMilliseconds).ToString() + " ms";
                    }
                    else if (pingReply.Status == IPStatus.DestinationHostUnreachable)
                    {
                        pingtime = "Offline";
                    }
                }
                else
                {
                    pingtime = "n/a";
                }
            });
        }

        public void InitDelayTimer(double delay)
        {
            DelayTimer = new System.Timers.Timer
            {
                Interval = delay
            };
            DelayTimer.Elapsed += DelayTimer_Elapsed;
            DelayTimer.Start();
        }

        private async void DelayTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (ActiveVPNConnection != null & VpnRemoteHosts != null)
            {
                string ipAddress = ActiveVPNConnection.NetworkDst.Remove(ActiveVPNConnection.NetworkDst.Length-1) + "1";
                await CheckHostLatency(ipAddress);
                VpnRemoteHosts[VpnRemoteHosts.IndexOf(ActiveVPNConnection)].Latency = pingtime;
                Console.WriteLine(pingtime);
            }
        }

        private void UpdateStatusBarMessage(string message)
        {
            EventAggregator.GetEvent<StatusBarMessageUpdateEvent>().Publish(message);
        }

        private void UpdateStatusBarActiveConnectionMessage(string message)
        {
            EventAggregator.GetEvent<StatusBarActiveConnectionMessageUpdateEvent>().Publish(message);
        }

        private void UpdateStatusBarInterfaceMessage(string message)
        {
            EventAggregator.GetEvent<StatusBarInterfaceMessageUpdateEvent>().Publish(message);
        }
    }
}
