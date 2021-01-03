using GalaSoft.MvvmLight.Messaging;
using LToDo;
using LToDo.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LTodo.Test.DesktopTest
{
    [TestClass]
    public class MainTest : BaseTest<MainWindowViewModel>
    {
        [TestMethod]
        public void Change_Edit_State_Test()
        {
            var newEdit = _testClass.IsEdit;
            Messenger.Default.Register<bool>(this, "EditStateChanged", (edit) => { newEdit = edit; });
            var oldEdit = _testClass.IsEdit;
            _testClass.ChangeEditState();
            Assert.IsTrue(!oldEdit == _testClass.IsEdit);
            Assert.IsTrue(!oldEdit == newEdit);
        }

        [TestMethod]
        public void Change_Topmost_State_Test()
        {
            var oldTop = _testClass.IsTopmost;
            _testClass.ChangeTopmostState();
            Assert.IsTrue(!oldTop == _testClass.IsTopmost);
        }

        [TestMethod]
        public void Update_Task_Content_Test()
        {
            var newTask = DataSeed.GetTask();
            _testClass.UpdateTaskContent(newTask);
            Assert.AreEqual(newTask, _testClass.CurrentTask);
            Assert.AreEqual(newTask.Content, _testClass.UpdateContent);
            Assert.IsTrue(_testClass.IsUpdateTask);
        }

        [TestMethod]
        public void Save_Update_Test()
        {
            var ct = DataSeed.GetTask();
            TaskModel msgTask = ct;
            Messenger.Default.Register<TaskModel>(this, "SaveTaskContent", (task) => { msgTask = task; });
            _testClass.CurrentTask = ct;
            _testClass.UpdateContent = "New Test";
            _testClass.SaveUpdate();
            Assert.IsFalse(_testClass.IsUpdateTask);
            Assert.AreEqual(_testClass.CurrentTask.Content, "New Test");
            Assert.AreEqual(_testClass.CurrentTask, msgTask);
        }

        [TestMethod]
        public void Cancel_Update_Test()
        {
            _testClass.CancelUpdate();
            Assert.IsFalse(_testClass.IsUpdateTask);
        }
    }
}
