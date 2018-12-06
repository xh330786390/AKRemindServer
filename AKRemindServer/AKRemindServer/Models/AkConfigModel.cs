using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRemindServer.Models
{
    public class AkConfigModel
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
    }
}
