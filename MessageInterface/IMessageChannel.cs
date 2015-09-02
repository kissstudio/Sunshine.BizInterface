using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunshine.BizInterface.MessageInterface
{
    public interface IMessageChannel
    {
        IMessageChannelAccount Account { get; }
        string ChannelName { get; }
        string ChannelVersion { get; }
        string ChannelType { get; }
    }

    public interface IMessageChannelAccount
    {
        string AccountName { get; }
        float QueryBalance();
    }
}
