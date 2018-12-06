using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRemindReport.Models
{
    /// <summary>
    /// 服务区实体
    /// </summary>
    public class AkServerReportModel
    {
        /// <summary>
        /// 时段（早餐、中餐、下午茶、晚餐）
        /// </summary>
        public string TimeInterval { get; set; }
        /// <summary>
        /// 小时，时间段：比如：11:00-11:30
        /// </summary>
        public string HourTime { get; set; }
        /// <summary>
        /// 订单来源
        /// </summary>
        public int OrderMode { get; set; }
        /// <summary>
        /// 订单来源
        /// </summary>
        public string OrderModeName { get; set; }

        /// <summary>
        /// 终端类型：柜台、手机自助、大屏自助
        /// </summary>
        public int Terminal { get; set; }
        /// <summary>
        /// 终端类型：柜台、手机自助、大屏自助
        /// </summary>
        public string TerminalName { get; set; }
        /// <summary>
        /// 营业额
        /// </summary>
        public string SalesTurnover { get; set; }
        /// <summary>
        /// 订单数量
        /// </summary>
        public string OrderNum { get; set; }
        /// <summary>
        /// 产品平均数
        /// </summary>
        public string AvgProduct { get; set; }

        /// <summary>
        /// 点餐时间
        /// </summary>
        public string Item1 { get; set; }
        /// <summary>
        /// 收银时间
        /// </summary>
        public string Item2 { get; set; }
        /// <summary>
        /// 备餐时间
        /// </summary>
        public string Item3 { get; set; }
        /// <summary>
        /// 汇餐时间
        /// </summary>
        public string Item4 { get; set; }
        /// <summary>
        /// 整体时间
        /// </summary>
        public string Item5 { get; set; }
        /// <summary>
        /// 超时订单100%
        /// </summary>
        public string Item6 { get; set; }
        #region【柜台】
        /// <summary>
        /// 柜台订单数量
        /// </summary>
        public string Item11 { get; set; }
        /// <summary>
        /// 点餐时间
        /// </summary>
        public string Item12 { get; set; }
        /// <summary>
        /// 收银时间
        /// </summary>
        public string Item13 { get; set; }
        /// <summary>
        /// 备餐时间
        /// </summary>
        public string Item14 { get; set; }
        /// <summary>
        /// 汇餐时间
        /// </summary>
        public string Item15 { get; set; }
        /// <summary>
        /// 整体时间
        /// </summary>
        public string Item16 { get; set; }
        #endregion

        #region【自助点餐】
        /// <summary>
        /// 自助点餐订单数量
        /// </summary>
        public string Item21 { get; set; }
        /// <summary>
        /// 备餐时间
        /// </summary>
        public string Item22 { get; set; }
        /// <summary>
        /// 汇餐时间
        /// </summary>
        public string Item23 { get; set; }
        #endregion

        #region【外送】
        /// <summary>
        /// 外送订单数量
        /// </summary>
        public string Item31 { get; set; }
        /// <summary>
        /// 备餐时间
        /// </summary>
        public string Item32 { get; set; }
        #endregion
        /// <summary>
        /// 行位置
        /// </summary>
        public int RowIndex { get; set; }
    }
}
