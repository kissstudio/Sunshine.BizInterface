using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sunshine.BizInterface.AccountValidation
{
    /// <summary>
    /// 短信验证码管理器
    /// </summary>
    public abstract class SmsVerifyCodeManagerBase : IValidationTokenManager, IDisposable
    {
        private SunshineRandom Rnd = new SunshineRandom();
        private bool Disposed { get; set; }
        private Task LoopTask { get; set; }
        public SmsVerifyCodeManagerBase()
        {
            LoopTask = Task.Factory.StartNew(() =>
             {
                 while (!Disposed)
                 {
                     lock (syncObj)
                     {
                         var keys = vcodeDictionary.Keys.ToList();
                         foreach (var key in keys)
                         {
                             if (vcodeDictionary[key].Removed || vcodeDictionary[key].IsTimeout())
                             {
                                 vcodeDictionary.Remove(key);
                             }
                         }
                         GC.Collect();
                     }
                     Thread.Sleep(1000);
                 }
             });
        }
        object syncObj = new object();
        Dictionary<string, BizVerifyModel> vcodeDictionary = new Dictionary<string, BizVerifyModel>();
        string IValidationTokenManager.NewToken(string accountId, string bizType)
        {
            return this.NewVCode(accountId, bizType, TimeSpan.FromMinutes(TimeoutInMinutes));
        }

        bool IValidationTokenManager.CheckToken(string accountId, string bizType, string token, bool once)
        {
            return this.CheckVCode(accountId, bizType, token, once);
        }
        protected abstract int TimeoutInMinutes { get; }
        protected abstract string NextVerifyCode(SunshineRandom rnd);

        public string NewVCode(string id, string bizId, TimeSpan timeout)
        {
            lock (syncObj)
            {
                var vcode = this.NextVerifyCode(this.Rnd);
                var vm = BizVerifyModel.Create(id, bizId, vcode, timeout);
                if (vcodeDictionary.ContainsKey(vm.Md5))
                {
                    vcodeDictionary[vm.Md5].Refresh(timeout);
                }
                else
                {
                    vcodeDictionary.Add(vm.Md5, vm);
                }
                return vcodeDictionary[vm.Md5].VerifyCode;
            }
        }

        private static string GetMd5(string id, string bizId)
        {
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                var input = Encoding.UTF8.GetBytes(id + "." + bizId);
                byte[] output = md5.ComputeHash(input, 0, input.Length);
                return Encoding.UTF8.GetString(output);
            }
        }

        /// <summary>
        /// 对于一个验证码 仅能正确一次
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bizId">VerifyCodeBizType</param>
        /// <param name="vcode"></param>
        /// <param name="errorType"></param>
        /// <returns></returns>
        public bool CheckVCode(string userId, string bizId, string vcode, bool removeIfSuccess = true)
        {
            var hashKey = GetMd5(userId, bizId);
            lock (syncObj)
            {
                if (vcodeDictionary.ContainsKey(hashKey))
                {
                    if (vcodeDictionary[hashKey].Removed)
                    {
                        return false;
                    }

                    var isTimeout = vcodeDictionary[hashKey].IsTimeout();
                    if (isTimeout)
                    {
                        MarkAsRemoved(hashKey);
                        return false;
                    }
                    else
                    {
                        var matched = vcodeDictionary[hashKey].IsMatch(vcode);
                        if (matched && removeIfSuccess)
                        {
                            MarkAsRemoved(hashKey);
                        }
                        return matched;
                    }
                }
                return false;
            }
        }
        private void MarkAsRemoved(string hashKey)
        {
            if (!vcodeDictionary[hashKey].Removed)
            {
                vcodeDictionary[hashKey].MarkAsRemoved();
            }
        }

        private class BizVerifyModel
        {
            private static SunshineRandom Rnd = new SunshineRandom();
            private BizVerifyModel() { }
            public static BizVerifyModel Create(string id, string bizId, string vcode, TimeSpan timeout)
            {
                var model = new BizVerifyModel();
                model.CreateDate = DateTime.Now;
                model.Timeout = timeout;
                model.Id = id;
                model.BizId = bizId;
                model.VerifyCode = vcode;
                model.Md5 = SmsVerifyCodeManagerBase.GetMd5(id, bizId);
                return model;
            }
            public static BizVerifyModel Create(string id, string bizId, TimeSpan timeout)
            {
                var vcode = Rnd.Next(100000, 999999).ToString();
                return BizVerifyModel.Create(id, bizId, vcode, timeout);
            }


            /// <summary>
            /// 业务标识号（用户注册/找回密码/其他，请自行定义）
            /// </summary>
            public string BizId { get; private set; }

            /// <summary>
            /// 创建验证码对象
            /// </summary>
            public string Id { get; private set; }
            /// <summary>
            /// 验证码
            /// </summary>
            public string VerifyCode { get; private set; }

            /// <summary>
            /// 超时时间
            /// </summary>
            public TimeSpan Timeout { get; private set; }

            /// <summary>
            /// 创建日期
            /// </summary>
            public DateTime CreateDate { get; private set; }

            /// <summary>
            /// 是否有效
            /// </summary>
            /// <param name="vcode">验证码</param>
            /// <param name="bizId">业务类型</param>
            /// <returns></returns>
            /// <summary>
            /// 是否匹配
            /// </summary>
            /// <returns></returns>
            public virtual bool IsMatch(string vcode)
            {
                return VerifyCode == vcode;
            }

            /// <summary>
            /// 是否过期
            /// </summary>
            /// <returns></returns>
            public bool IsTimeout()
            {
                return CreateDate.AddMilliseconds(Timeout.TotalMilliseconds) < DateTime.Now;
            }

            public bool Removed { get; private set; }

            public void MarkAsRemoved()
            {
                this.Removed = true;
            }

            public string Md5 { get; private set; }

            internal void Refresh(TimeSpan timeout)
            {
                //对标记为删除的刷新验证码
                if (this.Removed)
                {
                    this.VerifyCode = Rnd.Next(100000, 999999).ToString();
                }
                this.CreateDate = DateTime.Now;
                this.Timeout = timeout;
            }
        }

        public void Dispose()
        {
            this.Disposed = true;
            LoopTask.Wait();
        }
    }
}
