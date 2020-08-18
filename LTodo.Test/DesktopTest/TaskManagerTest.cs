using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using LToDo;
using LToDo.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LTodo.Test.DesktopTest
{
    [TestClass]
    public class TaskManagerTest
    {
        private string Id;

        [TestMethod]
        public void GetAllTasksTest()
        {
            var tasks = TaskManager.GetAllTasks();
            var newTasks = tasks.ToList().Where((x, i) => tasks.ToList().FindIndex(z => z.Number == x.Number) == i).ToList();
            Assert.IsNotNull(tasks);
            Assert.IsNotNull(newTasks);
            Assert.IsTrue(tasks.Count == newTasks.Count);
        }

        [TestMethod]
        public void AddTask()
        {
            var tasks = TaskManager.GetAllTasks();
            int number = 1;
            if (tasks != null && tasks.Count != 0)
            {
                number = tasks.OrderBy(x => x.Number).LastOrDefault().Number + 1;
            }
            var newTask = new TaskModel() { Number = number, Content = Guid.NewGuid().ToString(), IsEnabled = true };
            Id = newTask.Id.ToString();
            Assert.IsNull(TaskManager.GetTaskById(Id));
            TaskManager.AddNewTask(newTask);
            Assert.IsNotNull(TaskManager.GetTaskById(Id));
        }
    }
}
