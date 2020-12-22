using GalaSoft.MvvmLight;
using LToDo.Database;
using LToDo.Http;
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
            Edit = false;
            Tasks.CollectionChanged += Tasks_CollectionChanged;
        }

        #region Message
        //private void SubscribeMessage()
        //{
        //    MessageManager.Instance.Connect("874183200@qq.com", "aaaaaa");
        //    MessageManager.OnReceiveMessage += MessageManager_OnReceiveMessage;
        //}

        //private void MessageManager_OnReceiveMessage(LTodo.Model.MessageType message, TaskModel task)
        //{
        //    switch (message)
        //    {
        //        case LTodo.Model.MessageType.Add:
        //            if (!TaskManager.IsTaskExist(task.Id))
        //                AddNewTask(task);
        //            else
        //                AddNewTask(task, false);
        //            break;
        //        case LTodo.Model.MessageType.Remove:
        //            if (TaskManager.IsTaskExist(task.Id))
        //                RemoveTask(task);
        //            break;
        //        case LTodo.Model.MessageType.Update:
        //            if (TaskManager.IsTaskExist(task.Id))
        //                UpdateTask(task);
        //            break;
        //        default:
        //            break;
        //    }
        //}
        #endregion

        #region Property
        private ObservableCollection<TaskModel> _tasks = TaskManager.GetAllTasks();
        public ObservableCollection<TaskModel> Tasks
        {
            get { return _tasks; }
            set
            {
                _tasks = value;
                RaisePropertyChanged(nameof(Tasks));
            }
        }

        private string _time = DateTime.Now.ToLongDateString().ToString();
        public string Time
        {
            get { return _time; }
            set { _time = value; RaisePropertyChanged(nameof(Time)); }
        }

        /// <summary>
        /// 是否有Todo项
        /// </summary>
        public bool HasTodo { get; set; }

        private bool _isMultiInput;
        /// <summary>
        /// 是否多行输入框
        /// </summary>
        public bool IsMultiInput
        {
            get { return _isMultiInput; }
            set { _isMultiInput = value; RaisePropertyChanged(nameof(IsMultiInput)); }
        }

        #region 置顶
        private bool _topmost = false;
        public bool Topmost
        {
            get { return _topmost; }
            set
            {
                _topmost = value;
                TopmostSource = !_topmost ? new BitmapImage(new Uri("/Resources/TopMost.png", UriKind.Relative)) : new BitmapImage(new Uri("/Resources/TopMostBlue.png", UriKind.Relative));
                TopmostToolTip = !_topmost ? "置顶" : "取消置顶";
                RaisePropertyChanged(nameof(Topmost));
                RaisePropertyChanged(nameof(TopmostSource));
                RaisePropertyChanged(nameof(TopmostToolTip));
            }
        }
        public BitmapImage TopmostSource { get; set; } = new BitmapImage(new Uri("/Resources/TopMost.png", UriKind.Relative));
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
                Tasks.ToList().ForEach(x =>
                {
                    x.CanMove = !Edit ? Visibility.Collapsed : Visibility.Visible;
                    //x.CanMove = x.IsEnabled ? !Edit ? Visibility.Collapsed : Visibility.Visible : x.CanMove;
                    x.IsEdit = Edit;
                });
                RaisePropertyChanged(nameof(Edit));
                RaisePropertyChanged(nameof(ListName));
                RaisePropertyChanged(nameof(EditToolTip));
            }
        }
        public string EditToolTip { get; set; } = "编辑";
        public string ListName { get; set; } = "清单列表";
        #endregion

        private bool _isSync = false;
        /// <summary>
        /// 是否正在同步
        /// </summary>
        public bool IsSync
        {
            get { return _isSync; }
            set { _isSync = value; RaisePropertyChanged(nameof(IsSync)); }
        }
        #endregion

        #region Method
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
                RaisePropertyChanged(nameof(HasTodo));
            }
        }

        /// <summary>
        /// 添加新任务
        /// </summary>
        /// <param name="newTask"></param>
        public async void AddNewTask(TaskModel newTask, bool isUpdate = true)
        {
            if (Config.IsTaskToBottom)
            {
                var lastIndex = Tasks.Where(x => x.IsEnabled == true).Count();
                Tasks.Insert(lastIndex, newTask);
                TaskManager.AddNewTask(Tasks[lastIndex]);
                var res = await HttpManager.Instance.PostAsync<HttpResponse>(new AddTaskRequest(Tasks[lastIndex]));
            }
            else
            {
                Tasks.Insert(0, newTask);
                TaskManager.AddNewTask(Tasks[0]);
                var res = await HttpManager.Instance.PostAsync<HttpResponse>(new AddTaskRequest(Tasks[0]));
            }
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="task"></param>
        public void RemoveTask(TaskModel task)
        {
            Tasks.Remove(task);
            TaskManager.RemoveTask(task);
        }

        /// <summary>
        /// 更新任务
        /// </summary>
        /// <param name="task"></param>
        public void UpdateTask(TaskModel task)
        {
            var oldTask = Tasks.FirstOrDefault(x => x.Id == task.Id);
            if (oldTask != null)
            {
                oldTask.Number = task.Number;
                oldTask.Content = task.Content;
                TaskManager.UpdateTask(task);
            }
        }

        /// <summary>
        /// 批量更新任务
        /// </summary>
        /// <param name="task"></param>
        public void UpdateAllTasks()
        {
            TaskManager.UpdateTasks(Tasks.ToArray());
        }
        #endregion
    }
}
