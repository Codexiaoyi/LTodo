using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace LToDo
{
    public class LTextBlock : TextBlock
    {
        public LTextBlock()
        {
            Focusable = true;
        }

        public static readonly DependencyProperty LTextProperty =
    DependencyProperty.Register("LText", typeof(string), typeof(LTextBlock),
        new FrameworkPropertyMetadata(new PropertyChangedCallback(OnLTextPropertyChanged)));

        public string LText
        {
            get { return (string)GetValue(LTextProperty); }
            set { SetValue(LTextProperty, value); }
        }

        static void OnLTextPropertyChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender != null && sender is LTextBlock)
            {
                LTextBlock view = (LTextBlock)sender;
                if (args != null && args.NewValue != null)
                {
                    string value = args.NewValue.ToString();
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        view.Text = "";
                        view.Inlines.Clear();
                    }
                    else
                    {
                        view.AddInlines(value);
                    }
                }
                else
                {
                    view.Text = "";
                    view.Inlines.Clear();
                }
            }
        }

        public void AddInlines(string value)
        {
            Regex urlregex = new Regex(@"(([hH][tT]{2}[pP]://|[hH][tT]{2}[pP][sS]://|[wW]{3}.|[wW][aA][pP].|[fF][tT][pP].|[fF][iI][lL][eE].)[A-Za-z0-9+&@#/%?=~_|!:,.;^-]+[A-Za-z0-9+&@#/%=~_|]|[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3})");
            var ss = urlregex.Matches(value);
            List<Tuple<int, int, string>> urlList = new List<Tuple<int, int, string>>();
            foreach (Match item in ss)
            {
                Tuple<int, int, string> urlIndex = new Tuple<int, int, string>(value.IndexOf(item.Value), item.Value.Length, item.Value);
                urlList.Add(urlIndex);
            }

            if (urlList.Count > 0)
            {
                this.Inlines.Clear();
                for (int i = 0; i < urlList.Count; i++)
                {
                    if (i == 0)
                    {
                        string startValue = value.Substring(0, urlList[0].Item1);
                        if (string.IsNullOrEmpty(startValue))
                            startValue = " ";
                        this.Inlines.Add(new Run() { Text = startValue });

                        AddHyperlink(urlList[0].Item3);
                    }
                    else
                    {
                        int stratIndex = urlList[i - 1].Item1 + urlList[i - 1].Item2;
                        this.Inlines.Add(new Run() { Text = value.Substring(stratIndex, urlList[i].Item1 - stratIndex) });

                        AddHyperlink(urlList[i].Item3);
                    }

                    if (i == urlList.Count - 1)
                    {
                        string endValue = value.Substring(urlList[i].Item1 + urlList[i].Item2);
                        if (string.IsNullOrEmpty(endValue))
                            endValue = " ";
                        this.Inlines.Add(new Run() { Text = endValue });
                    }
                }
            }
            else
            {
                this.Inlines.Clear();
                this.Text = value;
            }
        }

        private void AddHyperlink(string value)
        {
            try
            {
                Hyperlink link = new Hyperlink();
                link.Tag = value;
                //link.IsEnabled = false;
                link.Click += link_Click;
                //link.MouseEnter += Link_MouseEnter;
                //link.MouseLeave += Link_MouseLeave;
                link.Inlines.Add(new Run() { Text = value });
                this.Inlines.Add(link);
            }
            catch
            {
                this.Inlines.Add(new Run() { Text = value });
            }
        }

        private void Link_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Hyperlink link)
            {
                link.IsEnabled = false;
            }
        }

        private void Link_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Hyperlink link)
            {
                if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    link.IsEnabled = true;
                }
                else
                {
                    link.IsEnabled = false;
                }
            }
        }

        private void link_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Hyperlink link && link.DataContext is TaskModel task && App.Current.MainWindow is MainWindow window)
            {
                if (task.IsEnabled && Keyboard.Modifiers == ModifierKeys.Control)
                {
                    Process p = new Process();
                    p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.UseShellExecute = false;    //不使用shell启动
                    p.StartInfo.RedirectStandardInput = true;//喊cmd接受标准输入
                    p.StartInfo.RedirectStandardOutput = false;//不想听cmd讲话所以不要他输出
                    p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
                    p.StartInfo.CreateNoWindow = true;//不显示窗口
                    p.Start();

                    //向cmd窗口发送输入信息 后面的&exit告诉cmd运行好之后就退出
                    p.StandardInput.WriteLine("start " + link.Tag as string + "&exit");
                    p.StandardInput.AutoFlush = true;
                    p.WaitForExit();//等待程序执行完退出进程
                    p.Close();
                    return;
                }
                if (task.IsEnabled)
                {
                    link.Foreground = System.Windows.Media.Brushes.Gray;
                    task.IsEnabled = !task.IsEnabled;
                    window._mainWindowViewModel.Tasks.Move(window._mainWindowViewModel.Tasks.IndexOf(task), window._mainWindowViewModel.Tasks.Where(x => x.IsEnabled == true).Count());
                }
                else
                {
                    link.Foreground = System.Windows.Media.Brushes.White;
                    task.IsEnabled = !task.IsEnabled;
                    window._mainWindowViewModel.Tasks.Move(window._mainWindowViewModel.Tasks.IndexOf(task), 0);
                }
            }
        }
    }
}