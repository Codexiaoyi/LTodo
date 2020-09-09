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
using System.Windows.Media.Animation;
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
                _mainWindowViewModel.Tasks.Move(_mainWindowViewModel.Tasks.IndexOf(task), _mainWindowViewModel.Tasks.Where(x => x.IsEnabled == true).Count() - 1);
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

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !_mainWindowViewModel.IsMultiInput)
            {
                AddNewTask();
            }
        }

        public void AddNewTask()
        {
            if (!string.IsNullOrEmpty(_newTask.Text))
            {
                var newTask = new TaskModel() { Content = _newTask.Text };
                var lastIndex = _mainWindowViewModel.Tasks.Where(x => x.IsEnabled == true).Count();
                _mainWindowViewModel.Tasks.Insert(lastIndex, newTask);
                TaskManager.AddNewTask(_mainWindowViewModel.Tasks[lastIndex]);
                _newTask.Text = string.Empty;
            }
        }

        private void IsTopmost_Click(object sender, RoutedEventArgs e)
        {
            _mainWindowViewModel.Topmost = !_mainWindowViewModel.Topmost;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            _mainWindowViewModel.Edit = !_mainWindowViewModel.Edit;
            if (!_mainWindowViewModel.Edit)
            {
                //从无到有
                var slideCollapsed = Resources["EditVisible"] as Storyboard;
                slideCollapsed?.Begin((sender as FrameworkElement));
                _edit.IsHitTestVisible = true;
                _editEnd.Visibility = Visibility.Collapsed;
            }
            else
            {
                //从有到无
                var slideVisible = Resources["EditCollapsed"] as Storyboard;
                slideVisible?.Begin((sender as FrameworkElement));
                _editEnd.Visibility = Visibility.Visible;
                _edit.IsHitTestVisible = false;
            }
        }

        private void Enlarge_Click(object sender, RoutedEventArgs e)
        {
            ChangeInputSize((sender as FrameworkElement));
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            AddNewTask();
            ChangeInputSize((sender as FrameworkElement));
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ChangeInputSize((sender as FrameworkElement));
        }

        /// <summary>
        /// 改变输入框大小
        /// </summary>
        private void ChangeInputSize(FrameworkElement element)
        {
            _mainWindowViewModel.IsMultiInput = !_mainWindowViewModel.IsMultiInput;
            if (_mainWindowViewModel.IsMultiInput)
            {
                //变大
                var slideEnlarge = Resources["EnlargeInput"] as Storyboard;
                slideEnlarge?.Begin(element);
            }
            else
            {
                //变小
                var slideReduce = Resources["ReduceInput"] as Storyboard;
                slideReduce?.Begin(element);
            }
            _newTask.Focus();
        }

        private void TodoList_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IsClick = false;
                var pos = e.GetPosition(_todoList);
                HitTestResult result = VisualTreeHelper.HitTest(_todoList, pos);
                if (result == null)
                {
                    return;
                }
                var listBoxItem = Utils.FindVisualParent<ListBoxItem>(result.VisualHit);
                if (listBoxItem == null || listBoxItem.Content != _todoList.SelectedItem)
                {
                    return;
                }
                DataObject dataObj = new DataObject(listBoxItem.Content as TaskModel);
                DragDrop.DoDragDrop(_todoList, dataObj, DragDropEffects.Move);
            }
        }

        private void TodoList_Drop(object sender, DragEventArgs e)
        {
            IsClick = false;
            TaskManager.UpdateTasks(_mainWindowViewModel.Tasks.ToArray());
        }

        private void TodoList_DragEnter(object sender, DragEventArgs e)
        {
            IsClick = false;
            var pos = e.GetPosition(_todoList);
            var result = VisualTreeHelper.HitTest(_todoList, pos);
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

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var data = (sender as FrameworkElement).DataContext;
            if (data is TaskModel task)
            {
                TaskManager.RemoveTask(task);
                _mainWindowViewModel.Tasks.Remove(task);
            }
        }

        bool IsClick;

        private void TodoList_ButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsClick = true;
        }

        private void TodoList_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsClick = false;
            var data = (sender as FrameworkElement).DataContext;
            if (data is TaskModel task)
            {
                task.IsEnabled = !task.IsEnabled;
            }
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
