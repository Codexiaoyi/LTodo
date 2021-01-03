using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LToDo.Managers;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace LToDo.ViewModel
{
    public class TodoControlViewModel : ViewModelBase
    {
        public RelayCommand<TaskModel> ClickTaskCommand { get; set; }
        public RelayCommand<TaskModel> UpdateCommand { get; set; }
        public RelayCommand<TaskModel> DeleteCommand { get; set; }

        public TodoControlViewModel()
        {
            Tasks.CollectionChanged += Tasks_CollectionChanged;
            Messenger.Default.Register<TaskModel>(this, "AddNewTask", (task) => AddNewTask(task));
            Messenger.Default.Register<TaskModel>(this, "SaveTaskContent", (task) => UpdateTask(task));
            Messenger.Default.Register<bool>(this, "EditStateChanged", (isEdit) => EditStateChanged(isEdit));
            Messenger.Default.Register<TaskModel>(this, "EndDrag", (task) => UpdateTask(task));
            ClickTaskCommand = new RelayCommand<TaskModel>((task) => ClickTask(task));
            UpdateCommand = new RelayCommand<TaskModel>((task) => Messenger.Default.Send(task, "UpdateTaskContent"));
            DeleteCommand = new RelayCommand<TaskModel>((task) => RemoveTask(task));
        }

        #region Property
        private ObservableCollection<TaskModel> _tasks = TaskManager.GetAllTasks();

        /// <summary>
        /// 任务集合
        /// </summary>
        public ObservableCollection<TaskModel> Tasks
        {
            get { return _tasks; }
            set
            {
                _tasks = value;
                RaisePropertyChanged(nameof(Tasks));
            }
        }
        /// <summary>
        /// 是否有Todo项
        /// </summary>
        public bool HasTodo
        {
            get
            {
                return Tasks.Count > 0;
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// 任务集合变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tasks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Tasks.ToList().ForEach(x =>
            {
                x.Number = x.IsEnabled ? Tasks.IndexOf(x) + 1 : x.Number;
            });
            if (e.Action != NotifyCollectionChangedAction.Move)
            {
                RaisePropertyChanged(nameof(HasTodo));
            }
            UpdateAllTasks();
        }

        /// <summary>
        /// 点击某个任务
        /// </summary>
        public void ClickTask(TaskModel task)
        {
            task.IsEnabled = !task.IsEnabled;
            Tasks.ToList().ForEach(x =>
            {
                x.Number = x.IsEnabled ? Tasks.IndexOf(x) + 1 : x.Number;
            });
            UpdateAllTasks();
            Tasks = TaskManager.GetAllTasks();
        }

        /// <summary>
        /// 编辑状态变化
        /// </summary>
        public void EditStateChanged(bool isEdit)
        {
            Tasks.ToList().ForEach(x =>
            {
                x.CanMove = !isEdit ? Visibility.Collapsed : Visibility.Visible;
                x.IsEdit = isEdit;
            });
        }

        /// <summary>
        /// 添加新任务
        /// </summary>
        /// <param name="newTask"></param>
        public void AddNewTask(TaskModel newTask)
        {
            Tasks.Insert(0, newTask);
            TaskManager.AddNewTask(Tasks[0]);
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

        ~TodoControlViewModel()
        {
            Tasks.CollectionChanged -= Tasks_CollectionChanged;
            Messenger.Default.Unregister<TaskModel>(this, "AddNewTask", (task) => AddNewTask(task));
            Messenger.Default.Unregister<TaskModel>(this, "SaveTaskContent", (task) => UpdateTask(task));
            Messenger.Default.Unregister<bool>(this, "EditStateChanged", (isEdit) => EditStateChanged(isEdit));
            Messenger.Default.Unregister<TaskModel>(this, "EndDrag", (task) => UpdateTask(task));
        }
    }
}
