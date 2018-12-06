using AKRemindServer.Common;
using AKRemindServer.Dao;
using AKRemindServer.Models;
using Common.NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AKRemindServer
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

        public static string StoreID = string.Empty;

        static AkConfig()
        {
            //StoreID = AkDaoHelper.Instance_SystemParam.GetStoreNO();
        }


        /// <summary>
        /// 周期值
        /// </summary>
        public static int PeriodNum = 7;

        /// <summary>
        /// 周期更新频率值.
        /// </summary>
        public static int UpdateNum = 7;

        /// <summary>
        /// 开始日期
        /// </summary>
        public static string StartDate
        {
            get
            {
                //前一天日期(结束日期)
                return DateTime.Now.AddDays(-AkConfig.PeriodNum).ToString("yyyy-MM-dd");
            }
        }

        public static bool IsConnSuccess
        {
            get
            {
                return AkDaoHelper.Instance_SystemParam.IsConnSuccess();
            }
        }

        /// <summary>
        /// 结束日期
        /// </summary>
        public static string EndDate
        {
            get
            {
                return DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            }
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
            if (!AkConfig.IsConnSuccess)
            {
                LogHelper.Error(typeof(Calculation) + ".Start Exception error=", "连接数据库失败");
                return;
            }

            AkSettingModel model = FileHelper.Instance.Read();

            SysParam = AkSysParamHelper.GetConfig();

            PeriodNum = model.PeriodNum;
            UpdateNum = model.UpdateNum;
        }
    }
}
