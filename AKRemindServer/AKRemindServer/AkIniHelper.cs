using AKRemindServer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AKRemindServer
{
    /// <summary>
    /// Ini文件读写操作
    /// </summary>
    public class AkIniHelper
    {
        private static string basePath = Environment.GetEnvironmentVariable("AK");
        //private readonly string inifilepath = basePath + @"\aloha.ini";
        public static string inifilepath = Environment.CurrentDirectory + @"\aloha.ini";

        /// <summary>
        /// 声明API函数
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        public static AkConfigModel GetConfig()
        {
            if (!File.Exists(inifilepath))
            {
                File.Create(inifilepath);
            }

            AkConfigModel config = new AkConfigModel();
            config.PerTime = GetValue("pertime") != null ? Convert.ToInt32(GetValue("pertime")) : 0;
            config.TaskTime = GetValue("tasktime") != null ? Convert.ToInt32(GetValue("tasktime")) : 0;
            config.MinLine = GetValue("minline") != null ? Convert.ToInt32(GetValue("minline")) : 0;
            config.MaxLine = GetValue("maxline") != null ? Convert.ToInt32(GetValue("maxline")) : 0;
            //初始化周期
            AkConfig.PeriodNum = GetValue("period") != null ? Convert.ToInt32(GetValue("period")) : 0;
            return config;
        }

        /// <summary>
        /// 读取ini配置键值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(string key, string section = "CONFIG")
        {
            StringBuilder sb = new StringBuilder(1024);
            GetPrivateProfileString(section, key, string.Empty, sb, 1024, inifilepath);
            return sb.ToString();
        }

        /// <summary>
        /// 写入ini 键值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static bool SetValue(string key, string value, string section = "CONFIG")
        {
            try
            {
                WritePrivateProfileString(section, key, value, inifilepath);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
