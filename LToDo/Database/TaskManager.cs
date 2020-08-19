using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace LToDo.Database
{
    public class TaskManager
    {
        public static ObservableCollection<TaskModel> GetAllTasks()
        {
            var tasks = SqliteManager.Instance.Query<TaskModel>("Select * from TaskModel");
            var result = tasks.Where(x => x.IsEnabled).OrderBy(x => x.Number).Concat(tasks.Where(x => !x.IsEnabled));
            return new ObservableCollection<TaskModel>(result);
        }

        public static TaskModel GetTaskById(string id)
        {
            return SqliteManager.Instance.Query<TaskModel>($"Select * from TaskModel where Id='{id}'").FirstOrDefault();
        }

        public static int AddNewTask(TaskModel task)
        {
            return SqliteManager.Instance.Add(task);
        }

        public static int RemoveTask(TaskModel task)
        {
            return SqliteManager.Instance.Delete(task);
        }

        public static int UpdateTask(TaskModel task)
        {
            return SqliteManager.Instance.Update(task);
        }

        public static void UpdateTasks(TaskModel[] models)
        {
            SqliteManager.Instance.UpdateTransaction(models);
        }
    }
}
