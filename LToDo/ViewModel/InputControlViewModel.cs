using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LToDo.ViewModel
{
    public class InputControlViewModel : ViewModelBase
    {
        public InputControlViewModel()
        {
            AddCommand = new RelayCommand<string>((param) => AddNewTask(bool.Parse(param)));
            ChangeSizeCommand = new RelayCommand(ChangeSize);
        }

        #region Property
        private bool _isMultiInput;
        private string _newTaskContent;

        /// <summary>
        /// 是否多行输入框
        /// </summary>
        public bool IsMultiInput
        {
            get { return _isMultiInput; }
            set { _isMultiInput = value; RaisePropertyChanged(nameof(IsMultiInput)); }
        }

        /// <summary>
        /// 新任务的内容
        /// </summary>
        public string NewTaskContent
        {
            get { return _newTaskContent; }
            set
            {
                _newTaskContent = value; RaisePropertyChanged(nameof(NewTaskContent));
            }
        }
        #endregion

        public RelayCommand<string> AddCommand { get; set; }
        public RelayCommand ChangeSizeCommand { get; set; }

        #region Method
        /// <summary>
        /// 添加新任务
        /// </summary>
        /// <param name="newTask"></param>
        public void AddNewTask(bool isKey)
        {
            if (!string.IsNullOrEmpty(NewTaskContent))
            {
                if (isKey && IsMultiInput)
                    return;
                Messenger.Default.Send(new TaskModel() { Content = NewTaskContent }, "AddNewTask");
                NewTaskContent = string.Empty;
                if (IsMultiInput)
                    ChangeSize();
            }
        }

        /// <summary>
        /// 修改输入框大小
        /// </summary>
        public void ChangeSize()
        {
            IsMultiInput = !IsMultiInput;
            Messenger.Default.Send(IsMultiInput, "ChangeInputSize");
        }
        #endregion
    }
}
