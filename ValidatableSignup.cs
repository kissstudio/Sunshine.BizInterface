using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunshine.WebApi.BizInterface
{
    /// <summary>
    /// 支持校验的注册之步骤1，发送验证码
    /// </summary>
    public class ValidatableSignup : ITrackableBizAction
    {
        /// <summary>
        /// 业务操作类型
        /// </summary>
        public string BizType { get; private set; }
        public IValidationTokenManager validationTokenMgr { get; private set; }
        public IAccountValidationService ValidationService { get; private set; }

        /// <summary>
        /// 支持校验的注册
        /// </summary>
        /// <param name="validationSvc">账号校验服务</param>
        /// <param name="vcodeMgr">验证码管理器</param>
        /// <param name="bizType">业务类型</param>
        public ValidatableSignup(IAccountValidationService validationSvc, IValidationTokenManager vcodeMgr, string bizType = "signup")
        {
            this.ValidationService = validationSvc;
            validationTokenMgr = vcodeMgr;
            this.BizType = bizType;
        }

        /// <summary>
        /// 发送校验码
        /// </summary>
        public void SendVCode()
        {
            var vcode = this.validationTokenMgr.NewToken(ValidationService.AccountId, this.BizType);
            this.ValidationService.SendValidationMessage(vcode);
        }
    }

    /// <summary>
    /// 支持校验的注册之步骤2：校验验证码
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class ValidatableSignup2 : ITrackableBizAction
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

    /// <summary>
    /// 可追踪的业务操作
    /// </summary>
    public interface ITrackableBizAction
    {
        /// <summary>
        /// 业务操作类型
        /// </summary>
        string BizType { get; }
    }
}
