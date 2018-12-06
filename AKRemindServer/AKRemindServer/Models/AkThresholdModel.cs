using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRemindServer.Models
{
    /// <summary>
    /// 阀值
    /// </summary>
    public class AkThresholdModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 数据日期
        /// </summary>
        public string DataDate { get; set; }
        /// <summary>
        /// 阀值
        /// </summary>
        public int ThresholdValue { get; set; }
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
        /// 周期值
        /// </summary>
        public int PeriodNum { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public string PeriodStartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string PeriodEndDate { get; set; }
        /// <summary>
        /// 阀值创建时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 阀值创建时间
        /// </summary>
        public string UpdateTime { get; set; }
    }
}
