using LToDo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTodo.Test.DesktopTest
{
    public class DataSeed
    {
        public static TaskModel GetTask()
        {
            return new TaskModel()
            {
                Number = 1,
                Content = "Test Model(测试)",
                IsEnabled = true
            };
        }
    }
}
