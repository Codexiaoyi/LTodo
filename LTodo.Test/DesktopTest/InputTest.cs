using GalaSoft.MvvmLight.Messaging;
using LToDo.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTodo.Test.DesktopTest
{
    [TestClass]
    public class InputTest : BaseTest<InputControlViewModel>
    {
        [TestMethod]
        public void Add_New_Task_Test()
        {
            _testClass.NewTaskContent = string.Empty;
        }
    }
}
