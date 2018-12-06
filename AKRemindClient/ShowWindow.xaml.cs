using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AKRemindClient
{
    /// <summary>
    /// ShowWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ShowWindow : Window
    {
        private int countSecond = 90; //记录秒数

        private DispatcherTimer disTimer = new DispatcherTimer();
        private Thread thread;
        public ShowWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设置窗体在屏幕的位置
        /// </summary>
        void WinPosition()
        {
            double ScreenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;//WPF
            double ScreenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;//WPF
            this.Top = ScreenHeight - this.ActualHeight;
            this.Left = ScreenWidth - this.ActualWidth;
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    if (disTimer != null)
        //    {
        //        disTimer.Stop();
        //    }
        //    this.Close();
        //}

        private void ShowWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            //textBlockTime.Text = countSecond.ToString();

            //disTimer.Interval = new TimeSpan(0, 0, 0, 1); //参数分别为：天，小时，分，秒。此方法有重载，可根据实际情况调用。

            //disTimer.Tick += new EventHandler(disTimer_Tick); //每一秒执行的方法

            //disTimer.Start();

            WinPosition();

            thread = new Thread(Vise);
            thread.Start();
        }

        void Vise()
        {
            while (true)
            {
                Thread.Sleep(100);
                this.Dispatcher.Invoke(new Action(() =>
                {
                    this.Topmost = true;
                }));
            }
        }

        //void disTimer_Tick(object sender, EventArgs e)
        //{

        //    if (countSecond == 0)
        //    {
        //        //MessageBox.Show("结束");
        //        if (disTimer != null)
        //        {
        //            disTimer.Stop();
        //        }
        //        this.Close();
        //    }
        //    else
        //    {
        //        //判断lblSecond是否处于UI线程上
        //        if (textBlockTime.Dispatcher.CheckAccess())
        //        {
        //            textBlockTime.Text = countSecond.ToString();
        //        }
        //        else
        //        {
        //            textBlockTime.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
        //            {

        //                textBlockTime.Text = countSecond.ToString();

        //            }));
        //        }

        //        countSecond--;
        //    }
        //}
        private void ShowWindow_OnClosed(object sender, EventArgs e)
        {
            try
            {
                 thread.Abort(); 
            }
            catch (Exception)
            {
            }
        }
    }
}
