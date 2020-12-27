using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

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
    }
}
