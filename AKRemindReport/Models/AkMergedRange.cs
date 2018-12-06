using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRemindReport.Models
{
    /// <summary>
    /// 合并区域
    /// </summary>
    public class AkMergedRange
    {
        /// <summary>
        /// 起始行
        /// </summary>
        public int StartRow { get; set; }
        /// <summary>
        /// 结束行
        /// </summary>
        public int EndRow { get; set; }
        /// <summary>
        /// 起始列
        /// </summary>
        public int StartColumn { get; set; }
        /// <summary>
        /// 结束列
        /// </summary>
        public int EndColumn { get; set; }
    }
}
