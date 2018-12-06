using Common.NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AKRemindReport.Common
{
    public class FetchDbfData
    {
        /// <summary>
        /// 更新数据
        /// </summary>
        public static void StartGrindq()
        {
            if (string.IsNullOrEmpty(AkConfig.AkDir)) return;

            string path = Path.Combine(AkConfig.AkDir, @"BIN");
            if (!Directory.Exists(path))
            {
                LogHelper.Info("进程对应的目录不存在 ", string.Empty);
                return;
            }
            string file = System.IO.Path.Combine(path, "grindq.exe");
            if (!File.Exists(file))
            {
                LogHelper.Info("grindq.exe 进程不存在 ", string.Empty);
                return;
            }
            try
            {
                Process current = Process.GetCurrentProcess();
                Process[] processes = Process.GetProcessesByName("grindq");
                if (processes.Length > 0)
                {
                    return;
                }

                LogHelper.Info("正在启动 程序：grindq.exe ", string.Empty);
                ProcessStartInfo psi = new ProcessStartInfo();

                psi.FileName = System.IO.Path.Combine(path, "grindq.exe");

                psi.UseShellExecute = false;

                psi.WorkingDirectory = path;

                psi.CreateNoWindow = true;

                //psi.Arguments = value;

                Process process = Process.Start(psi);
                process.WaitForExit();
                LogHelper.Info("grindq.exe 更新数据完毕", string.Empty);
                LogHelper.Info(string.Empty, string.Empty);
            }
            catch (Exception er)
            {
                LogHelper.Error(typeof(FetchDbfData) + " 程序：grindq.exe 更新数据出错 error=", er.ToString());
            }
        }
    }
}
