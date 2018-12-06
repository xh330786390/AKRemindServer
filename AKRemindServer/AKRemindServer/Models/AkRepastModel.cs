using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRemindServer.Models
{
    /// <summary>
    /// 用餐信息
    /// </summary>
    public class AkRepastModel
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 用餐类型
        /// </summary>
        public int RepastType { get; set; }
        /// <summary>
        /// 用餐名
        /// </summary>
        public string RepastName { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
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
