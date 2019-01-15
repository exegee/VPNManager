using Prism.Events;

namespace VPNManager.Core.Events
{
    public class VPNStatusMessageEvent : PubSubEvent<string> { }
    public class VPNUserCredentialsEvent : PubSubEvent<bool> { }

}
