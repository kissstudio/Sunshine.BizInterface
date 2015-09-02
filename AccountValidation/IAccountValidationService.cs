using Sunshine.BizInterface.MessageInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunshine.BizInterface
{
    /// <summary>
    /// 账号验证服务
    /// </summary>
    public interface IAccountValidationService
    {
        string AccountId { get; }
        void SendValidationMessage(string vcode);
    }
}
