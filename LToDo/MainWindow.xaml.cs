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
        private MainWindowViewModel _mainWindowViewModel;
        public MainWindow()
        {
            InitializeComponent();
            _mainWindowViewModel = new MainWindowViewModel();
            DataContext = _mainWindowViewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

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
                _mainWindowViewModel.Tasks.Insert(0, new TaskModel() { Content = NewTask.Text });
                NewTask.Text = string.Empty;
            }
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _mainWindowViewModel.Topmost = !_mainWindowViewModel.Topmost;
            IsTopmost.Source = !_mainWindowViewModel.Topmost ? new BitmapImage(new Uri("/Top.png", UriKind.Relative)) : new BitmapImage(new Uri("/TopBlue.png", UriKind.Relative));
            IsTopmost.ToolTip = !_mainWindowViewModel.Topmost ? "置顶" : "取消置顶";
        }
    }
}
