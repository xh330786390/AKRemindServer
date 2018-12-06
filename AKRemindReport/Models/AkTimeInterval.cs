using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRemindReport.Models
{
    /// <summary>
    /// 日期时段
    /// 时间时段
    /// </summary>
    public class AkTimeInterval
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
    }
}
