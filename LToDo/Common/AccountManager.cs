using LToDo.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LToDo
{
    public class AccountManager
    {
        private static readonly Lazy<AccountManager> _accountManager = new Lazy<AccountManager>(new AccountManager());

        public static AccountManager Instance
        {
            get
            {
                return _accountManager.Value;
            }
        }

        private string _currentAccount;
        public event Action OnAccountStateChange;

        public string CurrentAccount
        {
            get
            {
                return _currentAccount;
            }
            set
            {
                _currentAccount = value;
                OnAccountStateChange?.Invoke();
            }
        }

        public string AccountToken { get; set; }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userEmail">用户邮箱</param>
        /// <param name="password">密码</param>
        public async void LoginAsync(string userEmail, string password)
        {
            var res = await HttpManager.Instance.PostAsync<LoginResponse>(new LoginRequest(userEmail, password));
            if (res.IsSuccessResponse)
            {
                this.CurrentAccount = "874183200@qq.com";
                this.AccountToken = res.Token;
            }
        }
    }
}
