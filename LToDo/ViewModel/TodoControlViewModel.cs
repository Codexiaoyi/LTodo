using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LToDo.Database;
using LToDo.Http;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            ClickTaskCommand = new RelayCommand<TaskModel>((task) => ClickTask(task));
            UpdateCommand = new RelayCommand<TaskModel>((task) => UpdateTask(task));
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
                //if (!IsEdit)
                //{
                //    TaskManager.UpdateTask(x);
                //}
            });
            if (e.Action != NotifyCollectionChangedAction.Move)
            {
                RaisePropertyChanged(nameof(HasTodo));
            }
        }

        /// <summary>
        /// 点击某个任务
        /// </summary>
        public void ClickTask(TaskModel task)
        {
            task.IsEnabled = !task.IsEnabled;
            if (task.IsEnabled)
                Tasks.Move(Tasks.IndexOf(task), 0);
            else
                Tasks.Move(Tasks.IndexOf(task), Tasks.Where(x => x.IsEnabled == true).Count());
        }

        /// <summary>
        /// 添加新任务
        /// </summary>
        /// <param name="newTask"></param>
        public async void AddNewTask(TaskModel newTask)
        {
            Tasks.Insert(0, newTask);
            TaskManager.AddNewTask(Tasks[0]);
            var res = await HttpManager.Instance.PostAsync<HttpResponse>(new AddTaskRequest(Tasks[0]));
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

        ~TodoControlViewModel()
        {
            Tasks.CollectionChanged -= Tasks_CollectionChanged;
            Messenger.Default.Unregister<TaskModel>(this, "AddNewTask", (task) => AddNewTask(task));
        }
    }
}
