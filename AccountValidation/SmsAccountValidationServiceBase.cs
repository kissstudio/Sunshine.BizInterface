using Sunshine.BizInterface.MessageInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunshine.BizInterface
{
    /// <summary>
    /// 短信验证服务
    /// </summary>
    public abstract class SmsAccountValidationServiceBase : IAccountValidationService
    {
        public SmsAccountValidationServiceBase(ISmsSendChannel smsSendChannel)
        {
            this.SmsSendChannel = smsSendChannel;
        }
        public ISmsSendChannel SmsSendChannel { get; private set; }

        protected abstract string GetSmsContent(string vcode);

        public void SendValidationMessage(string accountId, string vcode)
        {
            var content = this.GetSmsContent(vcode);
            SmsSendChannel.SendSms(accountId, content);
        }
    }
}
