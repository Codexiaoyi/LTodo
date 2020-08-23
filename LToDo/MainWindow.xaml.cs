using LToDo.Database;
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

namespace LToDo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel _mainWindowViewModel;
        public MainWindow()
        {
            InitializeComponent();
            _mainWindowViewModel = new MainWindowViewModel();
            DataContext = _mainWindowViewModel;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var data = (sender as FrameworkElement).DataContext;
            if (data is TaskModel task)
            {
                _mainWindowViewModel.Tasks.Move(_mainWindowViewModel.Tasks.IndexOf(task), 0);
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var data = (sender as FrameworkElement).DataContext;
            if (data is TaskModel task)
            {
                _mainWindowViewModel.Tasks.Move(_mainWindowViewModel.Tasks.IndexOf(task), _mainWindowViewModel.Tasks.Where(x => x.IsEnabled == true).Count());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddNewTask();
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddNewTask();
            }
        }

        public void AddNewTask()
        {
            if (!string.IsNullOrEmpty(NewTask.Text))
            {
                var newTask = new TaskModel() { Content = NewTask.Text };
                _mainWindowViewModel.Tasks.Insert(0, newTask);
                TaskManager.AddNewTask(_mainWindowViewModel.Tasks[0]);
                NewTask.Text = string.Empty;
            }
        }

        private void TodoList_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var pos = e.GetPosition(TodoList);
                HitTestResult result = VisualTreeHelper.HitTest(TodoList, pos);
                if (result == null)
                {
                    return;
                }
                var listBoxItem = Utils.FindVisualParent<ListBoxItem>(result.VisualHit);
                if (listBoxItem == null || listBoxItem.Content != TodoList.SelectedItem)
                {
                    return;
                }
                DataObject dataObj = new DataObject(listBoxItem.Content as TaskModel);
                DragDrop.DoDragDrop(TodoList, dataObj, DragDropEffects.Move);
            }
        }

        private void TodoList_Drop(object sender, DragEventArgs e)
        {
            TaskManager.UpdateTasks(_mainWindowViewModel.Tasks.ToArray());
        }

        private void TodoList_DragEnter(object sender, DragEventArgs e)
        {
            var pos = e.GetPosition(TodoList);
            var result = VisualTreeHelper.HitTest(TodoList, pos);
            if (result == null)
            {
                return;
            }
            //查找元数据
            var sourcePerson = e.Data.GetData(typeof(TaskModel)) as TaskModel;
            if (sourcePerson == null)
            {
                return;
            }
            //查找目标数据
            var listBoxItem = Utils.FindVisualParent<ListBoxItem>(result.VisualHit);
            if (listBoxItem == null)
            {
                return;
            }
            var targetPerson = listBoxItem.Content as TaskModel;
            if (ReferenceEquals(targetPerson, sourcePerson))
            {
                return;
            }
            _mainWindowViewModel.Tasks.Move(_mainWindowViewModel.Tasks.IndexOf(sourcePerson), _mainWindowViewModel.Tasks.IndexOf(targetPerson));
        }

        private void IsTopmost_Click(object sender, RoutedEventArgs e)
        {
            _mainWindowViewModel.Topmost = !_mainWindowViewModel.Topmost;
        }

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            _mainWindowViewModel.Sort = !_mainWindowViewModel.Sort;
        }
    }
    internal static class Utils
    {
        //根据子元素查找父元素
        public static T FindVisualParent<T>(DependencyObject obj) where T : class
        {
            while (obj != null)
            {
                if (obj is T)
                    return obj as T;

                obj = VisualTreeHelper.GetParent(obj);
            }
            return null;
        }
    }

}
