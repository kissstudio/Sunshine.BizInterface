using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunshine.BizInterface.AccountValidation
{
    public interface IUserService
    {
        bool TelLoginEnabled { get; }
        bool EmailLoginEnabled { get; }
        bool IsLoginNameCaseSensitive { get; }

        int ExchangeUserIdByName(string name);

        int ExchangeUserIdByTel(string tel);

        int ExchangeUserIdByEmail(string email);

        bool IsPasswordMatch(int id, string password);

        bool IsUserDisabled(int id);

        void SetPassword(int id, string password);
    }
}
