using AKRemindServer.Dao;
using AKRemindServer.DB;
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
    public class AkSysParamHelper
    {
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        public static AkSystemParamModel GetConfig()
        {
            AkSystemParamModel config = new AkSystemParamModel();

            //单位时间、任务间隔时间
            config.PerTime = AkDaoHelper.Instance_SystemParam.GetPerTime("3");
            config.TaskTime = config.PerTime;

            int minLine;
            int maxLine;
            //小值、大值
            AkDaoHelper.Instance_SystemParam.GetMinOrMaxinValue(out minLine, out maxLine, "4");
            config.MinLine = minLine;
            config.MaxLine = maxLine;

            //早餐时段
            config.BreakFast = AkDaoHelper.Instance_SystemParam.GetRepastTime("5");

            //中餐时段
            config.Lunch = AkDaoHelper.Instance_SystemParam.GetRepastTime("6");

            //下午茶时段
            config.AfternoonTea = AkDaoHelper.Instance_SystemParam.GetRepastTime("7");

            //晚餐时段
            config.Supper = AkDaoHelper.Instance_SystemParam.GetRepastTime("8");
            return config;
        }
    }
}
