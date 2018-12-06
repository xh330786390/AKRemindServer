using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRemindReport.Models
{
    public class AkTempModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int Station { get; set; }

        /// <summary>
        /// 订单id
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 订单来源类型
        /// </summary>
        public int Terminal { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public int Timebumped { get; set; }
    }
}
