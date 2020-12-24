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

        public MainWindowViewModel()
        {
            EditCommand = new RelayCommand(ChangeEditState);
            TopmostCommand = new RelayCommand(ChangeTopmostState);
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
        private bool _topmost = false;
        private bool _isEdit = false;
        private string _time = DateTime.Now.ToLongDateString().ToString();

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
                //Tasks.ToList().ForEach(x =>
                //{
                //    x.CanMove = !IsEdit ? Visibility.Collapsed : Visibility.Visible;
                //    x.IsEdit = IsEdit;
                //});
                RaisePropertyChanged(nameof(IsEdit));
            }
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
        #endregion
    }
}
