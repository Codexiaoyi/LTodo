using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows.Threading;

namespace LToDo.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public RelayCommand EditCommand { get; set; }
        public RelayCommand TopmostCommand { get; set; }
        public RelayCommand UpdateCancelCommand { get; set; }
        public RelayCommand UpdateSaveCommand { get; set; }

        public MainWindowViewModel()
        {
            EditCommand = new RelayCommand(ChangeEditState);
            TopmostCommand = new RelayCommand(ChangeTopmostState);
            UpdateCancelCommand = new RelayCommand(CancelUpdate);
            UpdateSaveCommand = new RelayCommand(SaveUpdate);
            Messenger.Default.Register<TaskModel>(this, "UpdateTaskContent", (task) => UpdateTaskContent(task));
            var timer = new DispatcherTimer() { Interval = TimeSpan.FromDays(1) };
            timer.Tick += (sender, e) =>
            {
                Time = DateTime.Now.ToLongDateString().ToString();
            };
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
        public TaskModel CurrentTask;
        private bool _topmost = false;
        private bool _isEdit = false;
        private bool _isUpdate = false;
        private string _updateContent;
        private string _time = DateTime.Now.ToLongDateString().ToString();

        /// <summary>
        /// 当前时间轴
        /// </summary>
        public string Time
        {
            get { return _time; }
            set { _time = value; RaisePropertyChanged(nameof(Time)); }
        }
        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool IsTopmost
        {
            get { return _topmost; }
            set { _topmost = value; RaisePropertyChanged(nameof(IsTopmost)); }
        }
        /// <summary>
        /// 是否在编辑状态
        /// </summary>
        public bool IsEdit
        {
            get { return _isEdit; }
            set
            {
                _isEdit = value;
                RaisePropertyChanged(nameof(IsEdit));
            }
        }
        /// <summary>
        /// 是否更新任务
        /// </summary>
        public bool IsUpdateTask
        {
            get { return _isUpdate; }
            set { _isUpdate = value; RaisePropertyChanged(nameof(IsUpdateTask)); }
        }
        /// <summary>
        /// 更新文本
        /// </summary>
        public string UpdateContent
        {
            get { return _updateContent; }
            set { _updateContent = value; RaisePropertyChanged(nameof(UpdateContent)); }
        }
        #endregion

        #region Method
        /// <summary>
        /// 切换编辑状态
        /// </summary>
        public void ChangeEditState()
        {
            IsEdit = !IsEdit;
            Messenger.Default.Send(IsEdit, "EditStateChanged");
        }
        /// <summary>
        /// 切换状态
        /// </summary>
        public void ChangeTopmostState()
        {
            IsTopmost = !IsTopmost;
        }
        /// <summary>
        /// 修改任务内容
        /// </summary>
        /// <param name="content"></param>
        public void UpdateTaskContent(TaskModel task)
        {
            CurrentTask = task;
            UpdateContent = task.Content;
            IsUpdateTask = true;
        }
        /// <summary>
        /// 保存修改
        /// </summary>
        public void SaveUpdate()
        {
            IsUpdateTask = false;
            CurrentTask.Content = UpdateContent;
            Messenger.Default.Send<TaskModel>(CurrentTask, "SaveTaskContent");
        }
        /// <summary>
        /// 取消修改
        /// </summary>
        public void CancelUpdate()
        {
            IsUpdateTask = false;
        }
        #endregion

        ~MainWindowViewModel()
        {
            Messenger.Default.Unregister<TaskModel>(this, "UpdateTaskContent", (task) => UpdateTaskContent(task));
        }
    }
}
