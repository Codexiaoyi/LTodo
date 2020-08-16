using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace LToDo
{
    public class TaskModel : ViewModelBase
    {
        public string Content { get; set; }

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
                CanMove = _isEnabled ? CanMove : Visibility.Collapsed;
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
