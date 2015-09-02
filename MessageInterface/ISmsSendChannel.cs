using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunshine.BizInterface.MessageInterface
{
    public interface ISmsSendChannel : IMessageChannel
    {
        void SendSms(string mobileTo, string content);
        void SendSms(IEnumerable<string> mobileTo, string content);
    }
}
