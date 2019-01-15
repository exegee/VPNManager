using System;

namespace VPNManager.Core.Services
{
    public interface IClosingService
    {
        event EventHandler ApplicationClosing;
    }
}
