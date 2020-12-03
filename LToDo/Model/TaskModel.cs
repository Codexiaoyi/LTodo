using LToDo.Database;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace LToDo
{
    public class TaskModel : ViewModelBase
    {
        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();

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
                PropertyChange(nameof(Number));
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
                PropertyChange(nameof(Content));
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
                CanMove = _isEnabled ? IsEdit ? Visibility.Visible : CanMove : Visibility.Collapsed;
                Number = _isEnabled ? Number : 0;
                PropertyChange(nameof(IsEnabled));
            }
        }

        private bool _isEdit;
        [Ignore]
        public bool IsEdit
        {
            get
            {
                return _isEdit;
            }
            set
            {
                _isEdit = value;
                PropertyChange(nameof(IsEdit));
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
    }
}
