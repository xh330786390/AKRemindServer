using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AKRemindClient.UDP;

namespace AKRemindClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ShowWindow show;
        const int SendSleep = 100;//ms
        int locIPLast;
        string locIPHead;
        private UDPListener udpListener;
        private Thread thread;
        private bool isok;

        public MainWindow()
        {
            InitializeComponent();

            SetNotifyIcon();
        }

        /// <summary>
        /// 分析提醒成功
        /// </summary>
        void AKRemindOK()
        {
            if (show != null)
            {
                show.Close();
            }
            show = new ShowWindow();
            show.Show();
        }

        #region UDP

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            //广播查找服务器 端口9124
            isok = false;
            SartFind();

            //开启udp监听负责接收广播分析提醒消息 端口9114
            udpListener = new UDPListener();
            udpListener.onAddMessage += new EventHandler<AddMessageEventArgs>(this.Message);
            udpListener.StartListen();

            this.Hide();
        }

        /// <summary>
        /// 接收分析提醒方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Message(object sender, AddMessageEventArgs e)
        {
            if (e.mess == "OK")
            {
                isok = true;
                this.Dispatcher.Invoke(new Action(() =>
                {
                    TextBlock1.Text = "正常工作中...";
                }));
            }
            else if (e.mess == "Error")
            {
                this.Dispatcher.Invoke(new Action(() =>
                   {
                       TextBlock1.Text = "正在连接服务器，请稍等...";
                   }));
                isok = false;
                SartFind();
            }
            else
            {
                this.Dispatcher.Invoke(new Action(() =>
                      {
                          if (e.mess == "分线提醒")
                          {
                              AKRemindOK();
                          }
                          else
                          {
                              if (show != null)
                              {
                                  show.Close();
                              }
                          }
                      }));
            }
        }

        private void SartFind()
        {
            string locIP = GetLocIP();
            if (string.IsNullOrEmpty(locIP)) { return; }
            string[] strs = locIP.Split('.');
            this.locIPHead = string.Format("{0}.{1}.{2}.", strs[0], strs[1], strs[2]);
            this.locIPLast = int.Parse(strs[3]);
            this.Dispatcher.Invoke(new Action(() =>
            {
                TextBlockIP.Text = string.Format("本地默认IP：{0}.{1}.{2}.{3}  监听端口：9114", strs[0], strs[1], strs[2], strs[3]);
            }));

            //用UDP协议发送广播
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            if (thread != null)
            {
                try
                {
                    thread.Abort();
                }
                catch (Exception)
                { }
            }
            thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    while (!isok)
                    {
                        try
                        {
                            for (int i = 2; i < 256; i++)
                            {
                                if (this.locIPLast != i)
                                {
                                    string ip = this.locIPHead + i.ToString();

                                    //设置端口号为9124
                                    IPEndPoint iep = new IPEndPoint(IPAddress.Parse(ip), 9124);
                                    //设置broadcast值为1，允许套接字发送广播信息
                                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                                    //将发送内容转换为字节数组
                                    byte[] bytes = System.Text.Encoding.Unicode.GetBytes(locIPHead + locIPLast.ToString());
                                    //向子网发送信息
                                    socket.SendTo(bytes, iep);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        Thread.Sleep(SendSleep);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    socket.Close();
                }

            }));
            thread.IsBackground = true;
            thread.Start();
        }

        public static string GetLocIP()
        {
            try
            {
                string AddressIP = String.Empty;
                foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                {
                    if (_IPAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (AddressIP == string.Empty)
                        {
                            AddressIP = _IPAddress.ToString();
                        }
                    }
                }
                return AddressIP;
            }
            catch (Exception e)
            {
                MessageBox.Show("请检测网络设置，获取本地IP失败 原因：" + e.Message);
                return string.Empty;
            }
        }

        private void ClosedSend()
        {
            try
            {
                //用UDP协议发送广播
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                try
                {
                    for (int i = 2; i < 256; i++)
                    {
                        if (this.locIPLast != i)
                        {
                            string ip = this.locIPHead + i.ToString();

                            //设置端口号为9124
                            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(ip), 9124);
                            //设置broadcast值为1，允许套接字发送广播信息
                            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                            //将发送内容转换为字节数组
                            byte[] bytes = System.Text.Encoding.Unicode.GetBytes("AK" + locIPHead + locIPLast.ToString());
                            //向子网发送信息
                            socket.SendTo(bytes, iep);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    socket.Close();
                }

                if (udpListener != null)
                {
                    udpListener.Stop();
                }
                if (thread != null)
                {
                    try
                    {
                        thread.Abort();
                    }
                    catch (Exception)
                    { }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        #endregion

        #region 弹出基本设置、关闭窗口、设置托盘
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            //if (MessageBox.Show("是否关闭窗口？", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                this.Hide();
                //ClosedSend();
                //System.Diagnostics.Process.GetCurrentProcess().Kill();
                //Environment.Exit(0);
            }
            //else
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 设置托盘
        /// </summary>
        void SetNotifyIcon()
        {
            //设置托盘的各个属性
            var notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.BalloonTipText = @"分线客户端已开启";
            notifyIcon.Text = @"分线客户端";
            notifyIcon.Icon = new System.Drawing.Icon(System.Windows.Forms.Application.StartupPath + @"\AKRemind.ico");
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(2000);
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);

            //设置菜单项
            System.Windows.Forms.MenuItem about = new System.Windows.Forms.MenuItem(@"打开");
            about.Click += new EventHandler(notifyIcon_MouseClick);

            //退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem(@"退出");
            exit.Click += new EventHandler(exit_Click);

            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { about, exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);
        }

        /// <summary>
        /// 托盘打开程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void notifyIcon_MouseClick(object sender, EventArgs e)
        {
            try
            {
                this.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            this.Activate();
        }

        /// <summary>
        /// 托盘关闭程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void exit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否关闭窗口？", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ClosedSend();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                Environment.Exit(0);
            }
        }

        #endregion
    }
}
