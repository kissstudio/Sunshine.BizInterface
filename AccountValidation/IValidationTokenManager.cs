using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunshine.BizInterface
{
    /// <summary>
    /// 验证码生成和校验接口
    /// </summary>
    public interface IValidationTokenManager
    {
        /// <summary>
        /// 生成新的验证码
        /// </summary>
        /// <param name="accountId">账号Id</param>
        /// <param name="bizType">业务类型</param>
        /// <returns></returns>
        string NewToken(string accountId, string bizType);
        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <param name="accountId">账号Id</param>
        /// <param name="bizType">业务类型</param>
        /// <param name="token">验证码</param>
        /// <param name="once">true表示校验正确就移除，false表示校验正确但保留，下次可继续使用。</param>
        /// <returns></returns>
        bool CheckToken(string accountId, string bizType, string token, bool once);
    }
}
