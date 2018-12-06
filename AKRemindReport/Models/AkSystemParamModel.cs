using AKRemindReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRemindReport.Models
{
    public class AkSystemParamModel
    {
        /// <summary>
        /// 单位时间，单位秒
        /// </summary>
        public int PerTime { get; set; }
        /// <summary>
        /// 任务间隔,单位秒
        /// </summary>
        public int TaskTime { get; set; }
        /// <summary>
        /// 线路较小值
        /// </summary>
        public int MinLine { get; set; }
        /// <summary>
        /// 线路较大值
        /// </summary>
        public int MaxLine { get; set; }

        /// <summary>
        /// 早餐
        /// </summary>
        public AkRepastModel BreakFast { get; set; }
        /// <summary>
        /// 中餐
        /// </summary>
        public AkRepastModel Lunch { get; set; }
        /// <summary>
        /// 下午茶
        /// </summary>
        public AkRepastModel AfternoonTea { get; set; }
        /// <summary>
        /// 晚餐
        /// </summary>
        public AkRepastModel Supper { get; set; }

        /// <summary>
        /// 所有屏
        /// </summary>
        public string Station { get; set; }

        /// <summary>
        /// A屏
        /// </summary>
        public int StationA { get; set; }
        /// <summary>
        /// A屏
        /// </summary>
        public int StationB { get; set; }
        /// <summary>
        /// A屏
        /// </summary>
        public int StationC { get; set; }
        /// <summary>
        /// A屏
        /// </summary>
        public int StationD { get; set; }
    }
}
