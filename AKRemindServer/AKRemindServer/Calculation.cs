using AKRemindServer.Dao;
using AKRemindServer.DB;
using AKRemindServer.Models;
using Common.NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace AKRemindServer
{
    public class Calculation
    {
        //UDP发送分线提醒方法
        //MainWindow.mainWindow.SendMess();
        /// <summary>
        /// 开始计算
        /// </summary>
        public void Start()
        {
            AkThreshold akThreshold = new AkThreshold();

            ///上一次任务执行的时间
            string lastTaskTime = string.Empty;

            while (true)
            {
                //清除日志
                clearLog();

                //@1.读取系统配置信息
                AkConfig.ReadParam();

                //AkConfig.SysParam = new AkSystemParamModel();
                //AkConfig.SysParam.TaskTime = 300;

                //@2.间隔时间,单位 ms
                int waitetime = AkTask.WatiTime(lastTaskTime);
                Thread.Sleep(waitetime);

                lastTaskTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                LogHelper.Info("开始任务时间 = ", lastTaskTime);

                //@3.线程跑阀值
                akThreshold.Excute(lastTaskTime);

                //@4是否分线提醒
                bool bl = new AkRemind().Remind();

                if (bl)
                {
                    MainWindow.mainWindow.SendMess("分线提醒");
                    LogHelper.Info("分线提醒: ", DateTime.Now.ToLongTimeString());
                }
                else
                {
                    MainWindow.mainWindow.SendMess("不分线提醒");
                    LogHelper.Info("不分线提醒: ", DateTime.Now.ToLongTimeString());
                }
                LogHelper.Info(string.Empty, string.Empty);
            }
        }


        private void clearLog()
        {
            string basePath = System.AppDomain.CurrentDomain.BaseDirectory;

            string errPath = Path.Combine(basePath, "Logs/Error");
            List<string> errFiles = getAllFiles(errPath);
            if (errFiles.Count > 2)
            {
                deletFile(errFiles.Skip(2).ToList());
            }

            string infoPath = Path.Combine(basePath, "Logs/Info");
            List<string> infoFiles = getAllFiles(infoPath);
            if (infoFiles.Count > 2)
            {
                deletFile(infoFiles.Skip(2).ToList());
            }
        }

        private void deletFile(List<string> files)
        {
            files.ForEach(f =>
            {
                File.Delete(f);
            });
        }

        /// <summary>
        /// 获取所有日志文件列表
        /// </summary>
        /// <param name="rootPath"></param>
        private List<string> getAllFiles(string path)
        {
            List<string> lt_files = new List<string>();
            if (Directory.Exists(path))
            {
                string[] files = System.IO.Directory.GetFiles(path, "*.log");
                foreach (string file in files)
                {
                    lt_files.Add(file);
                }
            }

            return lt_files.OrderByDescending(l => l).ToList();
        }
    }
}
