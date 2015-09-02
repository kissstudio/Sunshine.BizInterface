using Sunshine.BizInterface.MessageInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunshine.BizInterface
{


    /// <summary>
    /// 邮箱验证服务
    /// </summary>
    public abstract class EmailAccountValidationServiceBase : IAccountValidationService
    {
        public IEmailSendChannel EmailSendChannel { get; private set; }
        public string AccountId { get; private set; }

        public EmailAccountValidationServiceBase(IEmailSendChannel emailSendChannel, string emailAccount)
        {
            this.EmailSendChannel = emailSendChannel;
            this.AccountId = emailAccount;
        }

        protected abstract string GetEmailSubject();

        protected abstract string GetEmailContent(string vcode);

        public void SendValidationMessage(string vcode)
        {
            var subject = this.GetEmailSubject();
            var content = this.GetEmailContent(vcode);
            EmailSendChannel.SendEmail(AccountId, subject, content);
        }
    }
}
