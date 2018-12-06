using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
using AKRemindClient.UDP;
using AKRemindServer.Dao;
using AKRemindServer.Models;

namespace AKRemindServer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static MainWindow mainWindow;

        int locIPLast;
        string locIPHead;
        private UDPListener udpListener;
        private Socket socket;

        /// <summary>
        /// 分线客户端列表
        /// </summary>
        List<AkNetworkModel> akremind = new List<AkNetworkModel>();

        public MainWindow()
        {
            InitializeComponent();

            MainWindow.mainWindow = this;
            //计算在Calculation类中编写

            SetNotifyIcon();
        }

        #region UDP
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            //开启udp监听负责接收局域网连接用户并添加到分线客户端列表 端口9124
            udpListener = new UDPListener();
            udpListener.onAddMessage += new EventHandler<AddMessageEventArgs>(this.Message);
            udpListener.StartListen();

            //广播分析提醒 端口9114
            SartFind();

            TextBlock1.Text = "正常工作中...";

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
                Calculation calculation = new Calculation();
                calculation.Start();
            });

            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new System.Timers.ElapsedEventHandler(aTimer_Elapsed);
            // 设置引发时间的时间间隔 此处设置为１秒
            aTimer.Interval = 1000;
            aTimer.Enabled = true;
            aTimer.Start();

            this.Hide();
        }

        void aTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // 得到 hour minute second  如果等于某个值就开始执行
            int intHour = e.SignalTime.Hour;
            int intMinute = e.SignalTime.Minute;
            int intSecond = e.SignalTime.Second;

            // 定制时间,在07：00：00 的时候执行
            int iHour = 07;
            int iMinute = 00;
            int iSecond = 00;

            // 设置 每天的00：00：00开始执行程序
            if (intHour == iHour && intMinute == iMinute && intSecond == iSecond)
            {
                CheckData();
            }

            // 定制时间,在00：00：00 的时候执行
            int iHour1 = 16;
            int iMinute1 = 00;
            int iSecond1 = 00;
            // 设置 每天的00：00：00开始执行程序
            if (intHour == iHour1 && intMinute == iMinute1 && intSecond == iSecond1)
            {
                CheckData();
            }
        }

        /// <summary>
        /// 检查阀值和当前连接数
        /// </summary>
        void CheckData()
        {
            string mess = string.Empty;
            if (akremind.Count < 3)
            {
                //mess += "当前连接数为" + akremind.Count;
                mess += "  分线程序连接不正确，请联系400报修！";
            }

            AkThresholdModel akThreshold = AkDaoHelper.Instance_Threshold.GetNewThreshold();
            //if (akThreshold.BreakFastValue == 0)
            //{
            //    mess += "当前早餐阀值为0";
            //}
            //if (akThreshold.LunchValue == 0)
            //{
            //    mess += "当前午餐阀值为0";
            //}
            //if (akThreshold.AfternoonTeaValue == 0)
            //{
            //    mess += "当前下午茶阀值为0";
            //}
            //if (akThreshold.SupperValue == 0)
            //{
            //    mess += "当前晚餐阀值为0";
            //}
            if (akThreshold.BreakFastValue == 0 || akThreshold.LunchValue == 0 || akThreshold.AfternoonTeaValue == 0 || akThreshold.SupperValue == 0)
            {
                mess += "餐厅分线阀值数值异常，请打开基本设置，点击保存！";
            }

            if (!string.IsNullOrEmpty(mess))
            {
                MessageBox.Show(mess);
            }
        }

        /// <summary>
        /// 接收分析提醒方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Message(object sender, AddMessageEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.mess))
            {
                //客户端退出后将连接数据重置
                if (e.mess.Contains("AK"))
                {
                    var ip = e.mess.Replace("AK", "");
                    if (akremind.Any(x => x.IP == ip))
                    {
                        akremind.Remove(akremind.First(x => x.IP == ip));
                    }
                }
                else
                {
                    if (akremind.Any(x => x.IP == e.mess))
                    {
                        akremind.Remove(akremind.First(x => x.IP == e.mess));
                    }
                    akremind.Add(new AkNetworkModel { IP = e.mess });
                }

                this.Dispatcher.Invoke(new Action(() =>
                {
                    TextBlock8.Text = string.Format("客户端连接数:{0}", akremind.Count);
                    TextBlock23.Text = string.Empty;
                    foreach (var akRemindModel in akremind)
                    {
                        TextBlock23.Text += akRemindModel.IP + "\r\n";
                    }
                    SendMess("OK");
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
            TextBlockIP.Text = string.Format("服务IP：{0}.{1}.{2}.{3}  端口：9114", strs[0], strs[1], strs[2], strs[3]);

            //用UDP协议发送广播
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        /// <summary>
        /// UDP发送
        /// </summary>
        /// <param name="mes"></param>
        public void SendMess(string mes)
        {
            if (socket == null)
            {
                SartFind();
            }
            try
            {
                for (int i = 0; i < akremind.Count; i++)
                {
                    string ip = akremind[i].IP;

                    //设置端口号为9114
                    IPEndPoint iep = new IPEndPoint(IPAddress.Parse(ip), 9114);
                    //设置broadcast值为1，允许套接字发送广播信息
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                    //将发送内容转换为字节数组
                    byte[] bytes = System.Text.Encoding.Unicode.GetBytes(mes);
                    //向子网发送信息
                    socket.SendTo(bytes, iep);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
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
                SendMess("Error");

                if (udpListener != null)
                {
                    udpListener.Stop();
                }
                if (socket != null)
                {
                    socket.Close();
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
        /// 弹出基本设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SetUpWindow setUpWindow = new SetUpWindow();
            setUpWindow.Show();
        }

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
            notifyIcon.BalloonTipText = @"分线服务端已开启";
            notifyIcon.Text = @"分线服务端";
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
