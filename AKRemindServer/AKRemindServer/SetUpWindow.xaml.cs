using AKRemindServer.Dao;
using AKRemindServer.DB;
using AKRemindServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using AKRemindServer.Common;
using Common.NLog;

namespace AKRemindServer
{
    /// <summary>
    /// SetUpWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SetUpWindow : Window
    {
        public SetUpWindow()
        {
            InitializeComponent();
            if (AkConfig.SysParam == null)
            {
                AkConfig.ReadParam();
            }
        }

        private void SetUpWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            //初始化
            if (AkConfig.SysParam != null)
            {
                //textBoxPeriod.Text = AkConfig.PeriodNum.ToString();
                textBoxTime.Text = AkConfig.SysParam.PerTime.ToString();
                //textBoxJgTime.Text = AkConfig.SysParam.TaskTime.ToString();
                textBoxMin.Text = AkConfig.SysParam.MinLine.ToString();
                textBoxMax.Text = AkConfig.SysParam.MaxLine.ToString();
                //textBlockConfig.Text = AkIniHelper.inifilepath;
            }

            //@1.计算阀值的日期区间，比如：1,7,10,14天，则取设置区间的天数进行阀值计算
            List<string> ltPeriod = AkDaoHelper.Instance_SystemParam.GetPeriod("1");

            //@2.计算阀值的频率，比如：1，3，5天，则计算阀值的频率则为设置的频率
            List<string> ltUpdatePeriod = AkDaoHelper.Instance_SystemParam.GetPeriod("2");

            if (ltPeriod != null && ltPeriod.Any())
            {
                int index = 0;
                for (int i = 0; i < ltPeriod.Count; i++)
                {
                    var pd = ltPeriod[i];
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    comboBoxItem.Content = pd;
                    cmboxPeriod.Items.Add(comboBoxItem);
                    if (int.Parse(pd) == AkConfig.PeriodNum)
                    {
                        index = i;
                    }
                }
                cmboxPeriod.SelectedIndex = index;
            }

            if (ltUpdatePeriod != null && ltUpdatePeriod.Any())
            {
                int index = 0;
                for (int i = 0; i < ltUpdatePeriod.Count; i++)
                {
                    var pd = ltUpdatePeriod[i];
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    comboBoxItem.Content = pd;
                    cmboxTime.Items.Add(comboBoxItem);
                    if (int.Parse(pd) == AkConfig.UpdateNum)
                    {
                        index = i;
                    }
                }
                cmboxTime.SelectedIndex = index;
            }

            //读取每个阶段的阀值
            AkThresholdModel akThreshold = AkDaoHelper.Instance_Threshold.GetNewThreshold();
            if (akThreshold != null)
            {
                int breakFastValue = akThreshold.BreakFastValue;
                int itemFastValue = breakFastValue == 0 ? 0 : AkConfig.SysParam.PerTime / breakFastValue;
                cmbdoxitemZc1.Text = itemFastValue.ToString();
                cmbdoxfzZc1.Text = breakFastValue.ToString();

                int lunchValue = akThreshold.LunchValue;
                int itemLunchValue = lunchValue == 0 ? 0 : AkConfig.SysParam.PerTime / lunchValue;
                cmbdoxitemZc2.Text = itemLunchValue.ToString();
                cmbdoxfzZc2.Text = lunchValue.ToString();

                int afternoonTeaValue = akThreshold.AfternoonTeaValue;
                int itemAfternoonTeaValue = afternoonTeaValue == 0 ? 0 : AkConfig.SysParam.PerTime / afternoonTeaValue;
                cmbdoxitemZc3.Text = itemAfternoonTeaValue.ToString();
                cmbdoxfzZc3.Text = afternoonTeaValue.ToString();

                int supperValue = akThreshold.SupperValue;
                int itemSupperValue = supperValue == 0 ? 0 : AkConfig.SysParam.PerTime / supperValue;
                cmbdoxitemZc4.Text = itemSupperValue.ToString();
                cmbdoxfzZc4.Text = supperValue.ToString();
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!AkConfig.IsConnSuccess)
            {
                LogHelper.Error(typeof(Calculation) + ".Start Exception error=", "连接数据库失败");
            }
            else
            {
                SaveConfig();
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        void SaveConfig()
        {
            if (cmboxPeriod.SelectedItem != null)
            {
                AkConfig.PeriodNum = int.Parse(cmboxPeriod.Text.ToString());
            }
            if (cmboxTime.SelectedItem != null)
            {
                AkConfig.UpdateNum = int.Parse(cmboxTime.Text.ToString());
            }

            AkSettingModel model = new AkSettingModel();
            model.PeriodNum = AkConfig.PeriodNum;
            model.UpdateNum = AkConfig.UpdateNum;
            string json = JsonConvert.SerializeObject(model);
            FileHelper.Instance.Write(json);

            //保存后从新更新阀值
            AkThreshold akThreshold = new AkThreshold();
            akThreshold.SaveThreshold(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            MessageBox.Show("保存成功！");
            this.Close();
        }
    }
}
