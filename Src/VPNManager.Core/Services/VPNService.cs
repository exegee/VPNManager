using DotRas;
using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace VPNManager.Core.Services
{
    public class VPNService : IVPNService
    {
        public string InterfaceName { get; set; }
        public string ServerIPAddress { get; set; }
        public string PreSharedKey { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string Status { get; set; }
        public string InterfaceID { get; private set; }
        public string RouteNetworkDst { get; set; }
        public string RouteNetworkMask { get; set; }
        public string RouteGateway { get; set; }
        public int RouteInterfaceIndex { get; set; }

        RasEntry RasEntry;
        RasPhoneBook RasPhoneBook = new RasPhoneBook();
        RasDialer RasDialer = new RasDialer();

        public event EventHandler StatusChanged;
        public event EventHandler Connected;
        public event EventHandler Disconnected;

        public void CreateConnection()
        {
            RasEntry = RasEntry.CreateVpnEntry(
                InterfaceName,
                ServerIPAddress,
                RasVpnStrategy.L2tpOnly,
                RasDevice.Create("VPN device", RasDeviceType.Vpn));
        }

        public void Connect()
        {
            if (RasEntry == null)
            {
                CreateConnection();
            }
            RasEntry.Options.UsePreSharedKey = true;
            RasEntry.Options.RemoteDefaultGateway = false;
            RasEntry.Options.DisableClassBasedStaticRoute = true;
            
            RasPhoneBook.Open(RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User));
            if (RasPhoneBook.Entries.Where(x => x.Name == RasEntry.Name).Count() == 0)
            {
                RasPhoneBook.Entries.Add(RasEntry);
                RasEntry.UpdateCredentials(RasPreSharedKey.Client, PreSharedKey);
            }

            RasDialer.Credentials = new System.Net.NetworkCredential(UserName, UserPassword);
            RasDialer.EntryName = RasEntry.Name;
            RasDialer.PhoneBookPath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User);
            RasDialer.DialAsync();
            RasDialer.StateChanged += RasDialer_StateChanged;
            RasDialer.DialCompleted += RasDialer_DialCompleted;
        }


        private void RasDialer_DialCompleted(object sender, DialCompletedEventArgs e)
        {
            RouteInterfaceIndex = GetInterfaceIndex(InterfaceName);
            SetRoute();
            Connected.Invoke(this, e);
        }

        private void RasDialer_StateChanged(object sender, StateChangedEventArgs e)
        {
            UpdateStatus(e.State.ToString());
        }

        void OnStatusChanged(EventArgs e)
        {
            StatusChanged?.Invoke(this, e);
        }

        public void Disconnect()
        {
            UpdateStatus("Disconnecting...");
            foreach (RasConnection connection in RasConnection.GetActiveConnections().Where(s => s.EntryName == InterfaceName))
            {
                connection.HangUp();
                if (connection.Handle.IsClosed) Disconnected.Invoke(this, null);
                UpdateStatus("Disconnected");
            }
        }

        public int GetInterfaceIndex(string adapterName)
        {
            int _index = -1;
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                if (adapter.Description.Equals(adapterName, StringComparison.OrdinalIgnoreCase))
                {
                    IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                    IPv4InterfaceProperties p = adapterProperties.GetIPv4Properties();
                    _index = p.Index;
                }
            }
            return _index;
        }

        public void SetRoute()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/C route add" + " " + RouteNetworkDst + " " + "mask" + " " + RouteNetworkMask
                + " " + RouteGateway + " " + "if" + " " + RouteInterfaceIndex,
                Verb = "runas"
            };
            process.StartInfo = startInfo;
            process.Start();
            UpdateStatus("Connection routed to: " + RouteNetworkDst);
        }

        void UpdateStatus(string message)
        {
            Status = message;
            OnStatusChanged(new EventArgs());
        }


    }
}
