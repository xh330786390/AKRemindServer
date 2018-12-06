using AKRemindReport.DB;
using AKRemindReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Diagnostics;
using Common.NLog;

namespace AKRemindReport.Dao
{
    public class AkProductDao
    {
        static AkProductDao()
        {

        }

        #region ----------------产区报表数据------------------

        #region 【营业额】
        /// <summary>
        /// 营业额
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public decimal GetSalesTurnover(string startTime, string endTime)
        {
            List<AkOrderModel> ltResult = new List<AkOrderModel>();
            DataTable dtSales = GetSales(startTime, endTime);

            DataTable dtDbf = AkDbfDao.DbfTable;
            if (dtDbf != null && dtDbf.Rows.Count > 0 && dtSales != null && dtSales.Rows.Count > 0)
            {
                var query =
                     (from sales in dtSales.AsEnumerable()
                      from dbf in dtDbf.AsEnumerable()
                      where sales.Field<int>("PosId") == dbf.Field<int>("CHECK")
                      select new
                      {
                          AMOUNT = dbf.Field<double>("AMOUNT"),
                      });

                if (query != null)
                {
                    return (decimal)query.Sum(l => l.AMOUNT);
                }
            }
            return 0;
        }

        public DataTable GetSales(string startTime, string endTime)
        {
            string sql = "select OrderMode,Terminal,posid ";
            sql += "from [AlohaReporting].[dbo].[KITCHEN_Order] ";
            sql += "where CONVERT(varchar(100), dateofbusiness, 23) between  '" + AkConfig.StartDate + "' and '" + AkConfig.EndDate + "' ";
            sql += " and CONVERT(varchar(100), TimeStarted, 108) between  '" + startTime + "' and '" + endTime + "' ";

            var dt = SqlServerHelper.Instance.GetDataTable(AkConfig.ConnAlohaReporting, sql);
            return dt;
        }

        /// <summary>
        /// 获取周期内字段“ITEM”起始字符为1的所有dbf记录
        /// </summary>
        /// <returns></returns>
        //private DataTable GetDbfFile()
        //{
        //    DataTable dtResult = NewDataTable();

        //    string date = string.Empty;

        //    //时间以前一天为基点（不含当天），取周期内的数据。
        //    //比如：周期设为1周，当前为 2018-3-10，则时间取：2018-3-03 至 2018-3-09 日，7天时间
        //    string StartDate = DateTime.Now.AddDays(-AkConfig.PeriodNum).ToString("yyyyMMdd");
        //    string EndDate = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");

        //    for (int i = 1; i <= AkConfig.PeriodNum; i++)
        //    {
        //        date = DateTime.Now.AddDays(-i).ToString("yyyyMMdd");
        //        DataTable curTable = DbfHelper.OpenDbfFile(date);
        //        if (curTable != null)
        //        {
        //            UnitTable(curTable, dtResult);
        //        }
        //    }
        //    return dtResult;
        //}

        /// <summary>
        /// 获取周期内字段“ITEM”起始字符为1的所有dbf记录
        /// </summary>
        /// <returns></returns>
        private DataTable GetDbfFile()
        {
            DataTable dtResult = NewDataTable();
            DataTable curTable = DbfHelper.OpenDbfFile();
            if (curTable != null)
            {
                UnitTable(curTable, dtResult);
            }
            return dtResult;
        }



        /// <summary>
        /// 创建目标表结构
        /// </summary>
        /// <returns></returns>
        private DataTable NewDataTable()
        {
            DataTable dtTable = new DataTable();

            dtTable.TableName = "GNDSALE";
            dtTable.Columns.Add("CHECK", typeof(int));
            dtTable.Columns.Add("AMOUNT", typeof(double));
            return dtTable;
        }

        /// <summary>
        /// 周期内的dbf 数据合并
        /// 1).只合并使用到的“ITEM”、“QUANTITY”数据列
        /// 2).只取“ITEM”字段起始字符为1的记录
        /// </summary>
        /// <param name="curTable">需被合并的表</param>
        /// <param name="dtResult">结果表，目标表</param>
        /// <param name="itemStartChart">item 起始字符 值</param>
        private void UnitTable(DataTable curTable, DataTable dtResult)
        {
            String filter = "TYPE=4";
            DataRow[] rows = curTable.Select(filter);
            if (rows != null && rows.Length > 0)
            {
                foreach (DataRow row in rows)
                {
                    DataRow dr = dtResult.NewRow();
                    dr["CHECK"] = row["CHECK"];
                    dr["AMOUNT"] = row["AMOUNT"];
                    dtResult.Rows.Add(dr);
                }
            }
        }
        #endregion

        #region 【分线健康率】
        /// <summary>
        /// 分线健康率
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public string GetHealthNumber(string startTime, string endTime)
        {
            Dictionary<int, int> dict = AkDaoHelper.Instance_Remind.GetRemind(startTime, endTime);

            decimal result = 1m;
            int total = dict.Sum(l => l.Value);
            int health = 0;
            if (dict.ContainsKey(0))
            {
                health = dict[0];
            }

            if (total > 0)
            {
                result = health / (decimal)total;
            }

            string strRresult = "N/A";
            if (dict.Count > 0)
            {
                strRresult = (result * 100).ToString("#0.00") + "%";
            }
            return strRresult;
        }
        #endregion

        #region 【超时订单%】
        /// <summary>
        /// 超时订单
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public string GetTimeOut(decimal allOrderNum, string startTime, string endTime)
        {
            string sql = "select count(*) Value";
            sql += " FROM (SELECT distinct bb.posorderid ,cc.station ,(cc.timebumped-cc.timeactivated)ActualTime,aa.dateofbusiness  ";
            sql += "from [AlohaReporting].[dbo].[KITCHEN_Order]aa,[AlohaReporting].[dbo].[KITCHEN_Item]bb  ";
            sql += ",[AlohaReporting].[dbo].[KITCHEN_Routedorder] cc  ";
            sql += "where aa.posid=bb.posorderid and aa.dateofbusiness=bb.dateofbusiness  ";
            sql += " and CONVERT(varchar(100), aa.TimeStarted, 108) between '" + startTime + "' and '" + endTime + "'";
            sql += "  and bb.canceled=0 and bb.dateofbusiness=cc.dateofbusiness  ";
            sql += "   and aa.id=cc.orderid and cc.station in(" + AkConfig.SysParam.Station + ")  ";
            sql += "    and convert(varchar(100),aa.dateofbusiness,23) between '" + AkConfig.StartDate + "' and '" + AkConfig.EndDate + "' ";
            sql += ") aaa, CFCInStoreDB.[dbo].[Course] bbb  ";
            sql += "Where aaa.ActualTime>bbb.FirstAlertSeconds and bbb.number=4  ";

            var obj = SqlServerHelper.Instance.ExecuteScalar(AkConfig.ConnAlohaReporting, sql);

            if (obj != null)
            {
                int timeOut = Convert.ToInt32(obj);
                return ((timeOut / allOrderNum) * 100).ToString("0.00") + "%";
            }
            return null;
        }
        #endregion

        #region 【订单数量】
        /// <summary>
        /// 订单数量
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Dictionary<string, decimal> GetOrderNumber(string startTime, string endTime)
        {
            string sql = "select aaa.station,count(*) value from ";
            sql += "(SELECT distinct bb.posorderid ,cc.station ,aa.dateofbusiness ";
            sql += "from [AlohaReporting].[dbo].[KITCHEN_Order]aa,[AlohaReporting].[dbo].[KITCHEN_Item]bb ";
            sql += ",[AlohaReporting].[dbo].[KITCHEN_Routedorder] cc ";
            sql += "where aa.posid=bb.posorderid and aa.dateofbusiness=bb.dateofbusiness ";
            sql += "and CONVERT(varchar(100), aa.TimeStarted, 108) between '" + startTime + "' and '" + endTime + "'  ";
            sql += "and bb.canceled=0 and bb.dateofbusiness=cc.dateofbusiness ";
            sql += "and aa.id=cc.orderid and cc.station in(" + AkConfig.SysParam.Station + ")  ";
            sql += "and convert(varchar(100),aa.dateofbusiness,23) between  '" + AkConfig.StartDate + "' and '" + AkConfig.EndDate + "' ";
            sql += ")aaa ";
            sql += "group by aaa.station ";
            return getValues(sql);
        }
        #endregion

        #region 【产品数量】
        /// <summary>
        /// 产品数量
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Dictionary<string, decimal> GetProductNumber(string startTime, string endTime)
        {
            string sql = " SELECT cc.station,count(*) value ";
            sql += " from [AlohaReporting].[dbo].[KITCHEN_Order]aa,[AlohaReporting].[dbo].[KITCHEN_Item]bb,[AlohaReporting].[dbo].[KITCHEN_RoutedItem] cc ";
            sql += " where aa.posid=bb.posorderid and aa.dateofbusiness=bb.dateofbusiness ";
            sql += " and CONVERT(varchar(100), aa.TimeStarted, 108) between '" + startTime + "' and '" + endTime + "'  ";
            sql += " and bb.canceled=0 and bb.dateofbusiness=cc.dateofbusiness and bb.id=cc.itemid and cc.station in(" + AkConfig.SysParam.Station + ")  ";
            sql += " and convert(varchar(100),aa.dateofbusiness,23) between  '" + AkConfig.StartDate + "' and '" + AkConfig.EndDate + "' ";
            sql += " group by cc.station ";
            return getValues(sql);
        }
        #endregion

        #region【生产时间】
        /// <summary>
        /// 生产时间合计
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Dictionary<string, decimal> GetProduceTime(Dictionary<string, decimal> dict_order, string startTime, string endTime)
        {
            string sql = " select  aaa.station,sum(aaa.timebumped-aaa.timeactivated) value ";
            sql += " FROM (SELECT distinct bb.posorderid ,cc.station,cc.TimeBumped,cc.TimeActivated,aa.dateofbusiness ";
            sql += " from [AlohaReporting].[dbo].[KITCHEN_Order]aa,[AlohaReporting].[dbo].[KITCHEN_Item]bb ";
            sql += " ,[AlohaReporting].[dbo].[KITCHEN_Routedorder] cc ";
            sql += " where aa.posid=bb.posorderid and aa.dateofbusiness=bb.dateofbusiness ";
            sql += " and CONVERT(varchar(100), aa.TimeStarted, 108) between '" + startTime + "' and '" + endTime + "'";
            sql += " and bb.canceled=0 and bb.dateofbusiness=cc.dateofbusiness ";
            sql += " and aa.id=cc.orderid and cc.station in(" + AkConfig.SysParam.Station + ")  ";
            sql += " and convert(varchar(100),aa.dateofbusiness,23) between  '" + AkConfig.StartDate + "' and '" + AkConfig.EndDate + "'  ) aaa ";
            sql += " group by aaa.station ";
            return CalProduceTime(dict_order, sql);
        }
        #endregion

        #region 【私有方法】

        /// <summary>
        /// 计算生产时间
        /// </summary>
        /// <param name="dict_order"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        private Dictionary<string, decimal> getValues(string sql)
        {
            Dictionary<string, decimal> result = new Dictionary<string, decimal>();

            var dt = SqlServerHelper.Instance.GetDataTable(AkConfig.ConnAlohaReporting, sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                var query = (from t in dt.AsEnumerable()
                             select new AkOrderModel
                             {
                                 Terminal = t.Field<int>("station"),
                                 Value = t.Field<int>("Value")
                             }).ToList();

                if (query != null && query.Count() > 0)
                {
                    result["StationA"] = query.Where(l => l.Terminal == AkConfig.SysParam.StationA).Sum(s => s.Value);

                    result["StationB"] = query.Where(l => l.Terminal == AkConfig.SysParam.StationB).Sum(s => s.Value);

                    result["StationC"] = query.Where(l => l.Terminal == AkConfig.SysParam.StationC).Sum(s => s.Value);

                    result["StationD"] = query.Where(l => l.Terminal == AkConfig.SysParam.StationD).Sum(s => s.Value);

                    result["All"] = query.Sum(s => s.Value);
                }
            }

            return result;
        }

        /// <summary>
        /// 计算生产时间
        /// </summary>
        /// <param name="dict_order"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        private Dictionary<string, decimal> CalProduceTime(Dictionary<string, decimal> dict_order, string sql)
        {
            Dictionary<string, decimal> result = new Dictionary<string, decimal>();

            var dt = SqlServerHelper.Instance.GetDataTable(AkConfig.ConnAlohaReporting, sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                var query = (from t in dt.AsEnumerable()
                             select new AkOrderModel
                             {
                                 Terminal = t.Field<int>("station"),
                                 Value = t.Field<int>("Value")
                             }).ToList();

                if (query != null && query.Count() > 0)
                {
                    result["StationA"] = query.Where(l => l.Terminal == AkConfig.SysParam.StationA).Sum(s => s.Value);
                    if (dict_order["StationA"] > 0) result["StationA"] = result["StationA"] / dict_order["StationA"];
                    else result["StationA"] = 0;

                    result["StationB"] = query.Where(l => l.Terminal == AkConfig.SysParam.StationB).Sum(s => s.Value);
                    if (dict_order["StationB"] > 0) result["StationB"] = result["StationB"] / dict_order["StationB"];
                    else result["StationB"] = 0;

                    result["StationC"] = query.Where(l => l.Terminal == AkConfig.SysParam.StationC).Sum(s => s.Value);
                    if (dict_order["StationC"] > 0) result["StationC"] = result["StationC"] / dict_order["StationC"];
                    else result["StationC"] = 0;

                    result["StationD"] = query.Where(l => l.Terminal == AkConfig.SysParam.StationD).Sum(s => s.Value);
                    if (dict_order["StationD"] > 0) result["StationD"] = result["StationD"] / dict_order["StationD"];
                    else result["StationD"] = 0;

                    result["All"] = query.Sum(s => s.Value);
                    if (dict_order["All"] > 0) result["All"] = result["All"] / dict_order["All"];
                    else result["All"] = 0;
                }
            }

            return result;
        }
        #endregion
        #endregion
    }
}