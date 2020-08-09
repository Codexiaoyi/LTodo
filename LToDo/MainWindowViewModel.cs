using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace LToDo
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            Tasks = new ObservableCollection<TaskModel>();
            var timer = new DispatcherTimer() { Interval = TimeSpan.FromDays(1) };
            timer.Tick += (sender, e) =>
            {
                Time = DateTime.Now.ToLongDateString().ToString();
            };
        }

        private ObservableCollection<TaskModel> _tasks;
        public ObservableCollection<TaskModel> Tasks
        {
            get { return _tasks; }
            set { _tasks = value; PropertyChange(nameof(Tasks)); }
        }

        private string _time = DateTime.Now.ToLongDateString().ToString();
        public string Time
        {
            get { return _time; }
            set { _time = value; PropertyChange(nameof(Time)); }
        }

        private bool _topmost = false;
        public bool Topmost
        {
            get { return _topmost; }
            set { _topmost = value; PropertyChange(nameof(Topmost)); }
        }
    }
}
