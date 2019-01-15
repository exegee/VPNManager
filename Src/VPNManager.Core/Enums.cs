using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPNManager.Core
{
    public enum VPNConnectionStatus
    {
        Disconnected = 0,
        Disconnecting = 1,
        Connected = 2,
        Connecting = 3,
        NotReachable = 4
    }
}
