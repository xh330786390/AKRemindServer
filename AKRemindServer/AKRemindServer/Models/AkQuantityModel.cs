using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRemindServer.Models
{
    /// <summary>
    /// gnditem.dbf 文件数量相关内容
    /// </summary>
    public class AkQuantityModel
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Item 起始字符为1的数量
        /// </summary>
        public double Item1Quantity { get; set; }

        /// <summary>
        /// 所有数量
        /// </summary>
        public double TotalQuantity { get; set; }
    }
}
