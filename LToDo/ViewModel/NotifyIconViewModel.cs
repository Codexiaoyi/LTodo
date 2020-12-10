using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace LToDo
{
    public class NotifyIconViewModel : ViewModelBase
    {
        public NotifyIconViewModel()
        {
            AccountManager.Instance.OnAccountStateChange += () =>
            {
                AccountHeader = string.IsNullOrEmpty(AccountManager.Instance.CurrentAccount) ? "云端同步" : AccountManager.Instance.CurrentAccount;
            };

            SetAutoRunCommand = new RelayCommand(SetAutoRun);
            SetTaskSiteCommand = new RelayCommand(SetTaskSite);
            ChangeAccountCommand = new RelayCommand(ChangeAccount);
            ShowMainWindowCommand = new RelayCommand(ShowMainWindow);
            HideMainWindowCommand = new RelayCommand(HideMainWindow);
            ExitApplicationCommand = new RelayCommand(ExitApplication);
        }

        #region Property
        private string _autoRunIcon = SettingHelper.IsAutoRunEnabled()? "  √" : string.Empty;
        public string AutoRunIcon
        {
            get
            {
                return _autoRunIcon;
            }
            set
            {
                _autoRunIcon = value;
                RaisePropertyChanged();
            }
        }

        private string _taskToBottomIcon;
        public string TaskToBottomIcon
        {
            get
            {
                return _taskToBottomIcon;
            }
            set
            {
                _taskToBottomIcon = value;
                RaisePropertyChanged();
            }
        }

        private string _accountHeader = "云端同步";
        public string AccountHeader
        {
            get
            {
                return _accountHeader;
            }
            set
            {
                _accountHeader = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Command
        public RelayCommand SetAutoRunCommand { get; set; }
        public RelayCommand SetTaskSiteCommand { get; set; }
        public RelayCommand ChangeAccountCommand { get; set; }
        public RelayCommand ShowMainWindowCommand { get; set; }
        public RelayCommand HideMainWindowCommand { get; set; }
        public RelayCommand ExitApplicationCommand { get; set; }
        #endregion

        #region Method
        /// <summary>
        /// 设置开机自启动
        /// </summary>
        public void SetAutoRun()
        {
            AutoRunIcon = string.Empty;
            if (SettingHelper.IsAutoRunEnabled())
            {
                SettingHelper.AutoRun(false);
            }
            else
            {
                var result = SettingHelper.AutoRun(true);
                if (result)
                {
                    AutoRunIcon = "  √";
                }
            }
        }

        /// <summary>
        /// 设置任务插入位置
        /// </summary>
        public void SetTaskSite()
        {
            TaskToBottomIcon = string.Empty;
            if (Config.IsTaskToBottom)
            {
                Config.IsTaskToBottom = false;
            }
            else
            {
                var result = Config.IsTaskToBottom = true;
                if (result)
                {
                    TaskToBottomIcon = "  √";
                }
            }
        }

        /// <summary>
        /// 切换账号
        /// </summary>
        public void ChangeAccount()
        {

        }

        /// <summary>
        /// 打开软件
        /// </summary>
        public void ShowMainWindow()
        {
            Application.Current.MainWindow.Show();
            Application.Current.MainWindow.Activate();
        }

        /// <summary>
        /// 隐藏/开启软件
        /// </summary>
        public void HideMainWindow()
        {
            if (Application.Current.MainWindow.IsVisible)
            {
                Application.Current.MainWindow.Hide();
            }
            else
            {
                Application.Current.MainWindow.Show();
                Application.Current.MainWindow.Activate();
            }
        }

        /// <summary>
        /// 关闭软件
        /// </summary>
        public void ExitApplication() => Application.Current.Shutdown();
        #endregion
    }
}
