using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
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
            Tasks.CollectionChanged += Tasks_CollectionChanged;
        }

        private void Tasks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Tasks.Where(x => x.IsEnabled).ToList().ForEach(x => x.Number = Tasks.IndexOf(x) + 1);
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

        #region 置顶
        private bool _topmost = false;
        public bool Topmost
        {
            get { return _topmost; }
            set
            {
                _topmost = value;
                TopmostSource = !_topmost ? new BitmapImage(new Uri("Resources/Top.png", UriKind.Relative)) : new BitmapImage(new Uri("Resources/TopBlue.png", UriKind.Relative));
                TopmostToolTip = !_topmost ? "置顶" : "取消置顶";
                PropertyChange(nameof(Topmost));
                PropertyChange(nameof(TopmostSource));
                PropertyChange(nameof(TopmostToolTip));
            }
        }
        public BitmapImage TopmostSource { get; set; } = new BitmapImage(new Uri("Resources/Top.png", UriKind.Relative));
        public string TopmostToolTip { get; set; } = "置顶";
        #endregion

        #region 排序
        private bool _sort = false;
        public bool Sort
        {
            get { return _sort; }
            set
            {
                _sort = value;
                SortSource = !_sort ? new BitmapImage(new Uri("Resources/Sort.png", UriKind.Relative)) : new BitmapImage(new Uri("Resources/SortBlue.png", UriKind.Relative));
                SortToolTip = !_sort ? "排序" : "完成排序";
                CanAdd = !_sort ? true : false;
                Tasks.Where(x => x.IsEnabled).ToList().ForEach(x => x.CanMove = !_sort ? Visibility.Collapsed : Visibility.Visible);
                PropertyChange(nameof(CanAdd));
                PropertyChange(nameof(Sort));
                PropertyChange(nameof(SortSource));
                PropertyChange(nameof(SortToolTip));
            }
        }
        public BitmapImage SortSource { get; set; } = new BitmapImage(new Uri("Resources/Sort.png", UriKind.Relative));
        public string SortToolTip { get; set; } = "排序";
        public bool CanAdd { get; set; } = true;
        #endregion

    }
}
