using GalaSoft.MvvmLight.Messaging;
using LToDo;
using LToDo.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LTodo.Test.DesktopTest
{
    [TestClass]
    public class MainTest
    {
        private MainWindowViewModel _main;

        [TestInitialize]
        public void Init()
        {
            _main = new MainWindowViewModel();
        }

        [TestMethod]
        public void Change_Edit_State_Test()
        {
            var newEdit = _main.IsEdit;
            Messenger.Default.Register<bool>(this, "EditStateChanged", (edit) => { newEdit = edit; });
            var oldEdit = _main.IsEdit;
            _main.ChangeEditState();
            Assert.IsTrue(!oldEdit == _main.IsEdit);
            Assert.IsTrue(!oldEdit == newEdit);
        }

        [TestMethod]
        public void Change_Topmost_State_Test()
        {
            var oldTop = _main.IsTopmost;
            _main.ChangeTopmostState();
            Assert.IsTrue(!oldTop == _main.IsTopmost);
        }

        [TestMethod]
        public void Update_Task_Content_Test()
        {
            var newTask = DataSeed.GetTask();
            _main.UpdateTaskContent(newTask);
            Assert.AreEqual(newTask, _main.CurrentTask);
            Assert.AreEqual(newTask.Content, _main.UpdateContent);
            Assert.IsTrue(_main.IsUpdateTask);
        }

        [TestMethod]
        public void Save_Update_Test()
        {
            var ct = DataSeed.GetTask();
            TaskModel msgTask = ct;
            Messenger.Default.Register<TaskModel>(this, "SaveTaskContent", (task) => { msgTask = task; });
            _main.CurrentTask = ct;
            _main.UpdateContent = "New Test";
            _main.SaveUpdate();
            Assert.IsFalse(_main.IsUpdateTask);
            Assert.AreEqual(_main.CurrentTask.Content, "New Test");
            Assert.AreEqual(_main.CurrentTask, msgTask);
        }

        [TestMethod]
        public void Cancel_Update_Test()
        {
            _main.CancelUpdate();
            Assert.IsFalse(_main.IsUpdateTask);
        }
    }
}
