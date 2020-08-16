using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace LToDo
{
    public class TaskModel : ViewModelBase
    {
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
                CanMove = _isEnabled ? Visibility.Visible : Visibility.Collapsed;
                Number = _isEnabled ? Number : 0;
                PropertyChange(nameof(IsEnabled));
            }
        }

        private Visibility _canMove = Visibility.Collapsed;
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
    }
}
