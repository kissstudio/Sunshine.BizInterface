using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunshine.BizInterface
{
    /// <summary>
    /// 支持校验的操作之步骤1，发送验证码
    /// </summary>
    public class ValidatableOperation
    {
        /// <summary>
        /// 业务操作类型
        /// </summary>
        public string BizType { get; private set; }
        public IValidationTokenManager validationTokenMgr { get; private set; }
        public IAccountValidationService ValidationService { get; private set; }

        /// <summary>
        /// 支持校验操作
        /// </summary>
        /// <param name="validationSvc">账号校验服务</param>
        /// <param name="vcodeMgr">验证码管理器</param>
        /// <param name="bizType">业务类型</param>
        public ValidatableOperation(IAccountValidationService validationSvc, IValidationTokenManager vcodeMgr, string bizType)
        {
            this.ValidationService = validationSvc;
            validationTokenMgr = vcodeMgr;
            this.BizType = bizType;
        }

        /// <summary>
        /// 发送校验码
        /// </summary>
        public void SendVCode(string accountId)
        {
            var vcode = this.validationTokenMgr.NewToken(accountId, this.BizType);
            this.ValidationService.SendValidationMessage(accountId, vcode);
        }
    }
}
