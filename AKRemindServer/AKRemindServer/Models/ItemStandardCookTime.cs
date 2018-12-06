using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRemindServer.Models
{
    /// <summary>
    /// Item标准制作时间
    /// </summary>
    public class ItemStandardCookTime
    {
        /// <summary>
        /// Item
        /// </summary>
        public int Item { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public double Quantity { get; set; }
        /// <summary>
        /// 制作时间
        /// </summary>
        public int CookTime { get; set; }
        /// <summary>
        /// 单个Item数量，与，总Item数量的百分比
        /// </summary>
        public double ItemPercent { get; set; }
        /// <summary>
        /// Item标准制作时间
        /// </summary>
        public double ItemStandCooktime { get; set; }
    }
}
