using AKRemindReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRemindReport
{
    public class AkBase
    {
        /// <summary>
        /// 所有餐段时间
        /// </summary>
        public List<AkRepastModel> AllRepast = new List<AkRepastModel>();

        /// <summary>
        /// 默认值
        /// </summary>
        //public string DefaultValue = "N/A";
        public string DefaultValue = string.Empty;

        public AkBase()
        {
            AkSystemParamModel model = AkConfig.SysParam;

            if (model.BreakFast != null)
            {
                AllRepast.Add(new AkRepastModel() { RepastType = 1, RepastName = "早餐", StartTime = model.BreakFast.StartTime, EndTime = model.BreakFast.EndTime });
            }

            if (model.Lunch != null)
            {
                AllRepast.Add(new AkRepastModel() { RepastType = 2, RepastName = "午餐", StartTime = model.Lunch.StartTime, EndTime = model.Lunch.EndTime });
            }

            if (model.AfternoonTea != null)
            {
                AllRepast.Add(new AkRepastModel() { RepastType = 3, RepastName = "下午茶", StartTime = model.AfternoonTea.StartTime, EndTime = model.AfternoonTea.EndTime });
            }

            if (model.Supper != null)
            {
                AllRepast.Add(new AkRepastModel() { RepastType = 4, RepastName = "晚餐", StartTime = model.Supper.StartTime, EndTime = model.Supper.EndTime });
            }
        }

        /// 获取一个餐段的重发次数，默认以：30分钟 一个段
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        public int RepeatNum(string startTime, string endTime, int minute = 30)
        {
            string strStartTime = DateTime.Now.ToString("yyyy-MM-dd ") + startTime;
            string strEndTime = DateTime.Now.ToString("yyyy-MM-dd ") + endTime;
            TimeSpan time = Convert.ToDateTime(strEndTime) - Convert.ToDateTime(strStartTime);
            return ((int)time.TotalMinutes / minute);
        }

        /// <summary>
        /// 获取需导出的餐段时间
        /// </summary>
        /// <param name="lt_repast"></param>
        /// <returns></returns>
        public List<AkRepastModel> GetRepast(List<int> lt_repast)
        {
            List<AkRepastModel> result = new List<AkRepastModel>();
            lt_repast.ForEach(item =>
            {
                var repast = this.AllRepast.FirstOrDefault(l => l.RepastType == item);
                if (repast != null)
                {
                    result.Add(repast);
                }
            });
            return result;
        }

        /// <summary>
        /// 今天日期
        /// </summary>
        public string Today
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
    }
}
