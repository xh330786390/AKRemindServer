using System.ComponentModel;
using System.Threading;
using AKRemindReport.Common;
using AKRemindReport.Dao;
using AKRemindReport.Models;
using Common.NLog;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AKRemindReport
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 异步处理
        /// </summary>
        private BackgroundWorker backgroundWorker = null;
        string file = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
            //this.dtStart.SelectedDate = DateTime.Now.AddMonths(-5);
            this.dtStart.SelectedDate = DateTime.Now;
            this.dtEnd.SelectedDate = DateTime.Now;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToDateTime(this.dtEnd.Text) < Convert.ToDateTime(this.dtStart.Text))
            {
                MessageBox.Show("开始日期不能大于结束日期！");
                return;
            }

            string strCurDate = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
            DateTime cureDate = Convert.ToDateTime(strCurDate);

            if (Convert.ToDateTime(this.dtStart.Text) > cureDate)
            {
                MessageBox.Show("开始日期不能大于当天日期！");
                return;
            }

            AkConfig.StartDate = Convert.ToDateTime(this.dtStart.Text).ToString("yyyy-MM-dd");
            AkConfig.EndDate = Convert.ToDateTime(this.dtEnd.Text).ToString("yyyy-MM-dd");

            _loading.Visibility = Visibility.Visible;

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.WorkerReportsProgress = true;
            //backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// 后台处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                List<int> lt_repast = new List<int>();

                bool cb1 = false;
                bool cb2 = false;
                bool cb3 = false;
                bool cb4 = false;
                this.Dispatcher.Invoke(new Action(() =>
                {
                    if (checkBox1.IsChecked != null)
                    {
                        cb1 = (bool)checkBox1.IsChecked;
                    }
                    if (checkBox2.IsChecked != null)
                    {
                        cb2 = (bool)checkBox2.IsChecked;
                    }
                    if (checkBox3.IsChecked != null)
                    {
                        cb3 = (bool)checkBox3.IsChecked;
                    }
                    if (checkBox4.IsChecked != null)
                    {
                        cb4 = (bool)checkBox4.IsChecked;
                    }
                }));
                if (cb1 == false && cb2 == false && cb3 == false &&
                    cb4 == false)
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        _loading.Visibility = Visibility.Collapsed;
                    }));
                    MessageBox.Show("请选择导出条件！");
                    return;
                }

                //早餐勾选
                if (cb1 == true)
                {
                    lt_repast.Add((int)AkEnumRepast.BreakFast);
                }
                //午餐勾选
                if (cb2 == true)
                {
                    lt_repast.Add((int)AkEnumRepast.Lunch);
                }
                //下午茶勾选
                if (cb3 == true)
                {
                    lt_repast.Add((int)AkEnumRepast.AfternoonTea);
                }
                //晚餐勾选
                if (cb4 == true)
                {
                    lt_repast.Add((int)AkEnumRepast.Supper);
                }

                file = saveFile();
                if (!string.IsNullOrEmpty(file))
                {
                    if (lt_repast.Count > 0 &&
                        Convert.ToDateTime(AkConfig.EndDate).ToString("yyyy-MM-dd").CompareTo(DateTime.Now.ToString("yyyy-MM-dd")) >= 0)
                    {
                        FetchDbfData.StartGrindq();
                    }

                    AkExport.Export(lt_repast, file);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("导出数据异常 ", ex.ToString());
            }
        }

        /// <summary>
        /// 完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            this.Dispatcher.Invoke(new Action(() =>
            {
                _loading.Visibility = Visibility.Collapsed;
            }));

            if (!string.IsNullOrEmpty(file))
            {
                MessageBox.Show("导出完成");
            }
        }

        private string saveFile()
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Title = "";
            //sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            sfd.Filter = "Excel| *.xlsx";
            sfd.FileName = "EK-Report" + DateTime.Now.ToString("yyyyMMddHHmmss");
            Nullable<bool> result = sfd.ShowDialog();
            if (result.Value)
            {
                return sfd.FileName;
            }
            return null;
        }
    }
}
