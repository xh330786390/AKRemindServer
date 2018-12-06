using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRemindServer.Models
{
    /// <summary>
    /// 周期
    /// </summary>
    public enum AKEnumPeriod
    {
        /// <summary>
        /// 天
        /// </summary>
        [Description("天")]
        Day = 1,

        /// <summary>
        /// 周
        /// </summary>
        [Description("周")]
        Week = 2,

        /// <summary>
        /// 月
        /// </summary>
        [Description("月")]
        Month = 3,

        /// <summary>
        /// 季
        /// </summary>
        [Description("季")]
        Season = 4,

        /// <summary>
        /// 年
        /// </summary>
        [Description("年")]
        Year = 5
    }
}
