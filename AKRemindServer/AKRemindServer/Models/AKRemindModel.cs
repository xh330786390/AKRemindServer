using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRemindServer.Models
{
    /// <summary>
    /// 分线（id，日期时间点，阀值，逻辑对比值，是否提醒分线，有无分线，周期值，周期开始时间，周期结束时间）
    /// </summary>
    public class AkRemindModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 任务时间
        /// </summary>
        public string Tasktime { get; set; }
        /// <summary>
        /// 早餐阀值
        /// </summary>
        public int BreakFastValue { get; set; }
        /// <summary>
        /// 中餐阀值
        /// </summary>
        public int LunchValue { get; set; }
        /// <summary>
        /// 下午茶阀值
        /// </summary>
        public int AfternoonTeaValue { get; set; }
        /// <summary>
        /// 晚餐阀值
        /// </summary>
        public int SupperValue { get; set; }

        /// <summary>
        /// 是否分线 1 分线，0 不分线
        /// </summary>
        public int IsRemind { get; set; }

        /// <summary>
        /// 有无分线 1 已分线，0 未分线
        /// </summary>
        public int HasRemind { get; set; }

        /// <summary>
        /// 周期值
        /// </summary>
        public int PeriodNum { get; set; }
    }
}
