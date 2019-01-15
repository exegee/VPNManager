using Prism.Events;

namespace VPNManager.Core.Events
{
    public class StatusBarMessageUpdateEvent : PubSubEvent<string> { }
    public class StatusBarActiveConnectionMessageUpdateEvent : PubSubEvent<string> { }
    public class StatusBarInterfaceMessageUpdateEvent : PubSubEvent<string> { }

}
