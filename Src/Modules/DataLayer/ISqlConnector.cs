using Business;
using System.Collections.Generic;

namespace DataLayer
{
    public interface ISqlConnector
    {
        List<VpnRemoteHost> GetRemoteHosts();
        bool UpdateVpnRemoteHost(VpnRemoteHost vpnRemoteHost);
    }
}
