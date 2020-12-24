using GalaSoft.MvvmLight.Messaging;
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
        public MainWindow()
        {
            InitializeComponent();
            Unloaded += (sender, e) =>
            {
                Messenger.Default.Unregister<bool>(this, "ChangeInputSize", (isMultiInput) => ChangeInputSize(isMultiInput));
                Messenger.Default.Unregister<bool>(this, "EditStateChanged", (isEdit) => OnEditStateChanged(isEdit));
            };
            Messenger.Default.Register<bool>(this, "ChangeInputSize", (isMultiInput) => ChangeInputSize(isMultiInput));
            Messenger.Default.Register<bool>(this, "EditStateChanged", (isEdit) => OnEditStateChanged(isEdit));
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
            if (e.ClickCount == 2)
            {
                WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            }
        }

        /// <summary>
        /// 改变编辑状态
        /// </summary>
        private void OnEditStateChanged(bool isEdit)
        {
            if (!isEdit)
            {
                //从无到有
                var slideCollapsed = Resources["EditVisible"] as Storyboard;
                slideCollapsed?.Begin(this);
            }
            else
            {
                //从有到无
                var slideVisible = Resources["EditCollapsed"] as Storyboard;
                slideVisible?.Begin(this);
            }
        }

        /// <summary>
        /// 改变输入框大小
        /// </summary>
        private void ChangeInputSize(bool isMultiInput)
        {
            if (isMultiInput)
            {
                //变大
                var slideEnlarge = Resources["EnlargeInput"] as Storyboard;
                slideEnlarge?.Begin(this);
            }
            else
            {
                //变小
                var slideReduce = Resources["ReduceInput"] as Storyboard;
                slideReduce?.Begin(this);
            }
        }

        //private void Update_Cancel_Click(object sender, RoutedEventArgs e)
        //{
        //    _update.Visibility = Visibility.Collapsed;
        //    _updateText.Text = string.Empty;
        //}

        //private void Update_Save_Click(object sender, RoutedEventArgs e)
        //{
        //    _update.Visibility = Visibility.Collapsed;
        //    var data = (sender as FrameworkElement).DataContext;
        //    if (!string.IsNullOrEmpty(_updateText.Text) && data is TaskModel task)
        //    {
        //        task.Content = _updateText.Text;
        //        //_mainWindowViewModel.UpdateTask(task);
        //    }
        //    _updateText.Text = string.Empty;
        //}

        //private void Update_Click(object sender, RoutedEventArgs e)
        //{
        //    _update.Visibility = Visibility.Visible;
        //    var data = (sender as FrameworkElement).DataContext;
        //    if (data is TaskModel task)
        //    {
        //        _updateText.Text = task.Content;
        //        _update.DataContext = task;
        //    }
        //    _updateText.Focus();
        //    _updateText.CaretIndex = _updateText.Text.Length;
        //}
    }
}
