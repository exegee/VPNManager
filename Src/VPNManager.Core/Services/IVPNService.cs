using System;

namespace VPNManager.Core.Services
{
    public interface IVPNService
    {
        string InterfaceName { get; set; }
        string ServerIPAddress { get; set; }
        string PreSharedKey { get; set; }
        string UserName { get; set; }
        string UserPassword { get; set; }
        string Status { get; set; }
        string RouteNetworkDst { get; set; }
        string RouteNetworkMask { get; set; }
        string RouteGateway { get; set; }
        int RouteInterfaceIndex { get; set; }
        void CreateConnection();
        int GetInterfaceIndex(string adapterName);
        void SetRoute();
        void Connect();
        void Disconnect();
        event EventHandler StatusChanged;
        event EventHandler Connected;
        event EventHandler Disconnected;
    }
}
