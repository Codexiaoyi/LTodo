using System;
using System.Collections.Generic;
using System.Text;

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
                PropertyChange(nameof(IsEnabled));
            }
        }
    }
}
