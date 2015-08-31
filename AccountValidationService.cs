using Nx.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunshine.WebApi.BizInterface
{
    /// <summary>
    /// 账号验证服务
    /// </summary>
    public interface IAccountValidationService
    {
        string AccountId { get; }
        void SendValidationMessage(string vcode);
    }

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

    /// <summary>
    /// 短信验证服务
    /// </summary>
    public abstract class SmsAccountValidationServiceBase : IAccountValidationService
    {
        public SmsAccountValidationServiceBase(string accountId, ISmsSendChannel smsSendChannel) {
            this.SmsSendChannel = smsSendChannel;
            this.AccountId = accountId;
        }
        public ISmsSendChannel SmsSendChannel { get; private set; }

        public string AccountId { get; private set; }

        protected abstract string GetSmsContent(string vcode);

        public void SendValidationMessage(string vcode)
        {
            var content = this.GetSmsContent(vcode);
            SmsSendChannel.SendSms(AccountId, content);
        }
    }
}
