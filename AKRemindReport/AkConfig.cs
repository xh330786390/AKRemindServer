using AKRemindReport.Models;
using Common.NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRemindReport
{
    public class AkConfig
    {
        public static readonly string ConnCFCInStoreDB = @"Data Source=(local)\SQLExpress;Initial Catalog=CFCInStoreDB;Integrated Security=True server = .\sqlexpress;integrated security = true;database = CFCInStoreDB";
        public static readonly string ConnAlohaReporting = @"Data Source=(local)\SQLExpress;Initial Catalog=AlohaReporting;Integrated Security=True server = .\sqlexpress;integrated security = true;database = AlohaReporting";

        //public static readonly string ConnCFCInStoreDB = @"server=192.168.20.233;database=CFCInStoreDB;user id=sa;password=111111";
        //public static readonly string ConnAlohaReporting = @"server=192.168.20.233;database=AlohaReporting;user id=sa;password=111111";

        //public static readonly string ConnCFCInStoreDB = @"server=106.14.123.223;database=CFCInStoreDB;user id=sa;password=Sa111111";
        //public static readonly string ConnAlohaReporting = @"server=106.14.123.223;database=AlohaReporting;user id=sa;password=Sa111111";

        //public static readonly string ConnCFCInStoreDB = @"server=(local);database=CFCInStoreDB;user id=sa;password=123456";
        //public static readonly string ConnAlohaReporting = @"server=(local);database=AlohaReporting;user id=sa;password=123456";

        /// <summary>
        /// 通过环境遍历读取目录
        /// </summary>
        public static string AkDir
        {
            get
            {
                //配置需重启
                string path = Environment.GetEnvironmentVariable("iberdir");
                if (string.IsNullOrEmpty(path))
                {
                    LogHelper.Info(typeof(AkConfig) + " =", "iberdir 环境变量为空");
                }
                return path;
            }
        }

        /// <summary>
        /// 开始日期
        /// </summary>
        public static string StartDate
        {
            //get
            //{
            //    return DateTime.Now.ToString("yyyy-MM-dd");
            //}
            get;
            set;
        }

        /// <summary>
        /// 结束日期
        /// </summary>
        public static string EndDate
        {
            //get
            //{
            //    return DateTime.Now.ToString("yyyy-MM-dd");
            //}
            get;
            set;
        }

        /// <summary>
        /// 系统配置信息
        /// </summary>
        public static AkSystemParamModel SysParam { get; set; }

        /// <summary>
        /// 读取系统配置信息
        /// </summary>
        public static void ReadParam()
        {
            SysParam = AkSysParamHelper.GetConfig();
        }
    }
}
