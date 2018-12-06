using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRemindServer.Models
{
    /// <summary>
    /// 逻辑数量，用于与阀值进行比较
    /// </summary>
    public class StationQuantity
    {
        /// <summary>
        ///线路 
        /// </summary>
        public int Station { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
    }
}
