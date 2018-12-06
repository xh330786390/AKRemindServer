using AKRemindReport.Dao;
using AKRemindReport.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AKRemindReport
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
            //config.PerTime = AkDaoHelper.Instance_SystemParam.GetPerTime("3");
            //config.TaskTime = config.PerTime;

            int minLine;
            int maxLine;
            //小值、大值
            //AkDaoHelper.Instance_SystemParam.GetMinOrMaxinValue(out minLine, out maxLine, "4");
            //config.MinLine = minLine;
            //config.MaxLine = maxLine;

            //早餐时段
            config.BreakFast = new AkRepastModel();
            initDateTime(config.BreakFast, "06:00", "10:00");

            ////中餐时段
            config.Lunch = new AkRepastModel();
            initDateTime(config.Lunch, "10:00", "14:00");

            ////下午茶时段
            config.AfternoonTea = new AkRepastModel();
            initDateTime(config.AfternoonTea, "14:00", "17:00");

            ////晚餐时段
            config.Supper = new AkRepastModel();
            initDateTime(config.Supper, "17:00", "22:00");


            //早餐时段
            config.BreakFast = AkDaoHelper.Instance_SystemParam.GetRepastTime("5");

            //中餐时段
            config.Lunch = AkDaoHelper.Instance_SystemParam.GetRepastTime("6");

            //下午茶时段
            config.AfternoonTea = AkDaoHelper.Instance_SystemParam.GetRepastTime("7");

            //晚餐时段
            config.Supper = AkDaoHelper.Instance_SystemParam.GetRepastTime("8");

            //显示的屏幕
            AkSystemParamModel model = new AkSystemParamModel();
            AkDaoHelper.Instance_SystemParam.GetStation("9", ref model);
            config.StationA = model.StationA;
            config.StationB = model.StationB;
            config.StationC = model.StationC;
            config.StationD = model.StationD;
            config.Station = model.Station;
            return config;
        }

        private static void initDateTime(AkRepastModel model, string startTime, string endTime)
        {
            model.StartTime = startTime;
            model.EndTime = endTime;
        }
    }
}
