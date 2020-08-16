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
            AutoRunIcon = SettingHelper.IsAutoRunEnabled() ? "  √" : string.Empty;
        }

        /// <summary>
        /// 开机自启
        /// </summary>
        public ICommand AutoRunCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () =>
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
                };
            }
        }

        /// <summary>
        /// 打开软件
        /// </summary>
        public ICommand ShowWindowCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () =>
                    {
                        Application.Current.MainWindow.Show();
                        Application.Current.MainWindow.Activate();
                    }
                };
            }
        }

        /// <summary>
        /// 隐藏/开启软件
        /// </summary>
        public ICommand HideCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () =>
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
                };
            }
        }

        /// <summary>
        /// 关闭软件
        /// </summary>
        public ICommand ExitApplicationCommand
        {
            get
            {
                return new DelegateCommand { CommandAction = () => Application.Current.Shutdown() };
            }
        }

        private string _autoRunIcon;
        public string AutoRunIcon
        {
            get
            {
                return _autoRunIcon;
            }
            set
            {
                _autoRunIcon = value;
                PropertyChange(nameof(AutoRunIcon));
            }
        }
    }
}
