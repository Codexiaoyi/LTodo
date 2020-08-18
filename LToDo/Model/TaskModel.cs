using LToDo.Database;
using LToDo.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace LToDo
{
    public class TaskModel : ViewModelBase
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        public bool IsSort;

        private int _number;
        /// <summary>
        /// Todo编号
        /// </summary>
        public int Number
        {
            get
            {
                return _number;
            }
            set
            {
                _number = value;
                PropertyChange(nameof(Text));
            }
        }

        private string _content;
        /// <summary>
        /// Todo内容
        /// </summary>
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                PropertyChange(nameof(Text));
            }
        }

        /// <summary>
        /// 显示文本
        /// </summary>
        [Ignore]
        public string Text
        {
            get
            {
                var p = string.IsNullOrEmpty(Number.ToString()) || Number == 0 ? string.Empty : $"{Number}、";
                return $"{p}{_content}";
            }
        }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                CanMove = _isEnabled ? IsSort ? Visibility.Visible : CanMove : Visibility.Collapsed;
                Number = _isEnabled ? Number : 0;
                PropertyChange(nameof(IsEnabled));
            }
        }

        private Visibility _canMove = Visibility.Collapsed;
        [Ignore]
        public Visibility CanMove
        {
            get
            {
                return _canMove;
            }
            set
            {
                _canMove = value;
                PropertyChange(nameof(CanMove));
            }
        }

        /// <summary>
        /// 删除Todo
        /// </summary>
        [Ignore]
        public ICommand DeleteCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () =>
                    {
                        if (App.Current.MainWindow is MainWindow mainWindow)
                        {
                            mainWindow._mainWindowViewModel.Tasks.Remove(this);
                            TaskManager.RemoveTask(this);
                        }
                    }
                };
            }
        }
    }
}
