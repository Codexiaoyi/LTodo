using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LToDo.Database
{
    public class TaskManager
    {
        public static ObservableCollection<TaskModel> GetAllTasks()
        {
            return new ObservableCollection<TaskModel>(SqliteManager.Instance.Query<TaskModel>("Select * from TaskModel"));
        }

        public static int AddNewTask(TaskModel task)
        {
            return SqliteManager.Instance.Add(task);
        }

        public static int RemoveTask(TaskModel task)
        {
            return SqliteManager.Instance.Delete(task);
        }
    }
}
