using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRemindReport.Models
{
    /// <summary>
    /// 产区实体
    /// </summary>
    public class AkProduceReportModel
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
        /// 订单来源Id
        /// </summary>
        public int OrderMode { get; set; }

        /// <summary>
        /// 超时订单
        /// </summary>
        public string TimeOut { get; set; }
        /// <summary>
        /// 营业额
        /// </summary>
        public string SalesTurnover { get; set; }
        /// <summary>
        /// 分线健康率
        /// </summary>
        public string RemindHealth { get; set; }

        #region ---------1线----------
        /// <summary>
        /// 1线订单数量
        /// </summary>
        public string OrderNum1 { get; set; }
        /// <summary>
        /// 1线产品数量
        /// </summary>
        public string ProductNum1 { get; set; }
        /// <summary>
        /// 1线产品平均数
        /// </summary>
        public string ProductAvgNum1 { get; set; }
        /// <summary>
        /// 1线生产时间
        /// </summary>
        public string ProduceTime1 { get; set; }
        #endregion

        #region ---------2线----------
        /// <summary>
        /// 2线订单数量
        /// </summary>
        public string OrderNum2 { get; set; }
        /// <summary>
        /// 2线产品数量
        /// </summary>
        public string ProductNum2 { get; set; }

        /// <summary>
        /// 2线产品平均数
        /// </summary>
        public string ProductAvgNum2 { get; set; }

        /// <summary>
        /// 2线生产时间
        /// </summary>
        public string ProduceTime2 { get; set; }
        #endregion

        #region ---------3线----------
        /// <summary>
        /// 3线订单数量
        /// </summary>
        public string OrderNum3 { get; set; }
        /// <summary>
        /// 3线产品数量
        /// </summary>
        public string ProductNum3 { get; set; }

        /// <summary>
        /// 3线产品平均数
        /// </summary>
        public string ProductAvgNum3 { get; set; }

        /// <summary>
        /// 3线生产时间
        /// </summary>
        public string ProduceTime3 { get; set; }
        #endregion

        #region ---------4线----------
        /// <summary>
        /// 4线订单数量
        /// </summary>
        public string OrderNum4 { get; set; }
        /// <summary>
        /// 4线产品数量
        /// </summary>
        public string ProductNum4 { get; set; }

        /// <summary>
        /// 4线产品平均数
        /// </summary>
        public string ProductAvgNum4 { get; set; }

        /// <summary>
        /// 4线生产时间
        /// </summary>
        public string ProduceTime4 { get; set; }
        #endregion

        #region ---------5线----------
        /// <summary>
        /// 5线订单数量
        /// </summary>
        public string OrderNum5 { get; set; }
        /// <summary>
        /// 5线产品数量
        /// </summary>
        public string ProductNum5 { get; set; }

        /// <summary>
        /// 5线产品平均数
        /// </summary>
        public string ProductAvgNum5 { get; set; }

        /// <summary>
        /// 5线生产时间
        /// </summary>
        public string ProduceTime5 { get; set; }
        #endregion

        #region ---------各线合计----------
        /// <summary>
        /// 5线订单数量
        /// </summary>
        public string OrderNumTotal { get; set; }
        /// <summary>
        /// 5线产品数量
        /// </summary>
        public string ProductNumTotal { get; set; }

        /// <summary>
        /// 5线产品平均数
        /// </summary>
        public string ProductAvgNumTotal { get; set; }

        /// <summary>
        /// 5线生产时间
        /// </summary>
        public string ProduceTimeTotal { get; set; }
        #endregion
        /// <summary>
        /// 行位置
        /// </summary>
        public int RowIndex { get; set; }
    }
}
