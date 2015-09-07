using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunshine.BizInterface
{
    /// <summary>
    /// 支持校验的操作之步骤2：校验验证码
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public sealed class ValidatableOperation2
    {
        /// <summary>
        /// 关联账号
        /// </summary>
        public string AccountId { get; private set; }
        IValidationTokenManager validationTokenMgr;
        public ValidatableOperation2(IValidationTokenManager vcodeMgr, string accountId, string bizType)
        {
            this.validationTokenMgr = vcodeMgr;
            this.AccountId = accountId;
            this.BizType = bizType;
        }

        /// <summary>
        /// 业务类型
        /// </summary>
        public string BizType { get; private set; }

        /// <summary>
        /// 验证码是否正确
        /// </summary>
        /// <param name="vcode"></param>
        /// <param name="removeIfSuccess"></param>
        /// <returns></returns>
        public bool CheckVCode(string vcode, bool removeIfSuccess = true)
        {
            return this.validationTokenMgr.CheckToken(AccountId, this.BizType, vcode, removeIfSuccess);
        }
    }
}
