using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunshine.BizInterface
{


    /// <summary>
    /// 支持校验的注册之步骤2：校验验证码
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class ValidatableSignup2
    {
        /// <summary>
        /// 要注册的账号
        /// </summary>
        public string AccountId { get; private set; }
        IValidationTokenManager validationTokenMgr;
        public ValidatableSignup2(IValidationTokenManager vcodeMgr, string accountId, string bizType = "signup")
        {
            this.validationTokenMgr = vcodeMgr;
            this.AccountId = accountId;
        }

        /// <summary>
        /// 业务类型
        /// </summary>
        public string BizType { get; private set; }

        /// <summary>
        /// 验证码是否正确
        /// </summary>
        /// <param name="vcode"></param>
        /// <returns></returns>
        public bool CheckVCode(string vcode)
        {
            return this.validationTokenMgr.CheckToken(AccountId, this.BizType, vcode, true);
        }
    }
}
