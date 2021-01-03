using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using GalaSoft.MvvmLight.Messaging;

namespace LToDo.View
{
    /// <summary>
    /// TodoControl.xaml 的交互逻辑
    /// </summary>
    public partial class TodoControl : UserControl
    {
        public TodoControl()
        {
            InitializeComponent();
        }

        private void _todoList_LogicalIndexChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            if (e.OriginalSource is FrameworkElement element && element.DataContext is TaskModel task)
            {
                task.Number = e.NewValue;
                Messenger.Default.Send(task, "EndDrag");
            }
        }
    }
}
