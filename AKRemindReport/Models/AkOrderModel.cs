using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRemindReport.Models
{
    public class AkOrderModel
    {
        /// <summary>
        /// 订单来源类型
        /// </summary>
        public int OrderMode { get; set; }

        /// <summary>
        /// 订单来源类型名称
        /// </summary>
        public string OrderModeName { get; set; }

        /// <summary>
        /// 订单来源类型
        /// </summary>
        public int Terminal { get; set; }

        /// <summary>
        /// 订单来源类型名称
        /// </summary>
        public string TerminalName { get; set; }

        /// <summary>
        /// 对应值
        /// </summary>
        public decimal Value { get; set; }
    }
}
