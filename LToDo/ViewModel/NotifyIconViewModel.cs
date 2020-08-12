﻿using System;
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
            AutoRunIcon = SettingHelper.IsRegister("ltodo.exe") ? "  √" : string.Empty;
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
                        if (SettingHelper.IsRegister("ltodo.exe"))
                        {
                            SettingHelper.UnregisterAutoRun();
                        }
                        else
                        {
                            var result = SettingHelper.RegisterAutoRun();
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

    public class DelegateCommand : ICommand
    {
        public Action CommandAction { get; set; }
        public Func<bool> CanExecuteFunc { get; set; }

        public void Execute(object parameter)
        {
            CommandAction();
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc == null || CanExecuteFunc();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
