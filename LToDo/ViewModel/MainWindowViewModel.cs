using LToDo.Database;
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
            var timer = new DispatcherTimer() { Interval = TimeSpan.FromDays(1) };
            timer.Tick += (sender, e) =>
            {
                Time = DateTime.Now.ToLongDateString().ToString();
            };
            HasTodo = Tasks.Count > 0;
            Tasks.CollectionChanged += Tasks_CollectionChanged;
        }

        private void Tasks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Tasks.ToList().ForEach(x =>
            {
                x.Number = x.IsEnabled ? Tasks.IndexOf(x) + 1 : x.Number;
                if (!Edit)
                {
                    TaskManager.UpdateTask(x);
                }
            });
            if (e.Action != System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                HasTodo = Tasks.Count <= 0 ? false : true;
                PropertyChange(nameof(HasTodo));
            }
        }

        private ObservableCollection<TaskModel> _tasks = TaskManager.GetAllTasks();
        public ObservableCollection<TaskModel> Tasks
        {
            get { return _tasks; }
            set
            {
                _tasks = value;
                PropertyChange(nameof(Tasks));
            }
        }

        private string _time = DateTime.Now.ToLongDateString().ToString();
        public string Time
        {
            get { return _time; }
            set { _time = value; PropertyChange(nameof(Time)); }
        }

        /// <summary>
        /// 是否有Todo项
        /// </summary>
        public bool HasTodo { get; set; }

        #region 置顶
        private bool _topmost = false;
        public bool Topmost
        {
            get { return _topmost; }
            set
            {
                _topmost = value;
                TopmostSource = !_topmost ? new BitmapImage(new Uri("Resources/TopMost.png", UriKind.Relative)) : new BitmapImage(new Uri("Resources/TopMostBlue.png", UriKind.Relative));
                TopmostToolTip = !_topmost ? "置顶" : "取消置顶";
                PropertyChange(nameof(Topmost));
                PropertyChange(nameof(TopmostSource));
                PropertyChange(nameof(TopmostToolTip));
            }
        }
        public BitmapImage TopmostSource { get; set; } = new BitmapImage(new Uri("Resources/TopMost.png", UriKind.Relative));
        public string TopmostToolTip { get; set; } = "置顶";
        #endregion

        #region 编辑
        private bool _edit = false;
        public bool Edit
        {
            get { return _edit; }
            set
            {
                _edit = value;
                EditToolTip = !Edit ? "编辑" : "取消编辑";
                ListName = !Edit ? "清单列表" : "拖动可排序";
                CanAdd = !Edit ? true : false;
                Tasks.ToList().ForEach(x =>
                {
                    x.CanMove = x.IsEnabled ? !Edit ? Visibility.Collapsed : Visibility.Visible : x.CanMove;
                    x.IsEdit = Edit;
                });
                PropertyChange(nameof(CanAdd));
                PropertyChange(nameof(Edit));
                PropertyChange(nameof(ListName));
                PropertyChange(nameof(EditToolTip));
            }
        }
        public string EditToolTip { get; set; } = "编辑";
        public string ListName { get; set; } = "清单列表";
        public bool CanAdd { get; set; } = true;
        #endregion

    }
}
