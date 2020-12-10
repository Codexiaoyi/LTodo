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
    }
}
