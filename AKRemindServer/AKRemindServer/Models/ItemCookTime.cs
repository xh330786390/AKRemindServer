using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRemindServer.Models
{
    /// <summary>
    /// Item的制作时间实体
    /// </summary>
    public class ItemCookTime
    {
        /// <summary>
        /// Item
        /// </summary>
        public int Item { get; set; }

        /// <summary>
        /// 制作时间
        /// </summary>
        public int CookTime { get; set; }
    }
}
