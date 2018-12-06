using AKRemindReport.DB;
using AKRemindReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;
using Common.NLog;

namespace AKRemindReport.Dao
{
    public class AkServerDao
    {
        private string tb_item = "[AlohaReporting].[dbo].[KITCHEN_Item]";
        private string tb_order = "[AlohaReporting].[dbo]..[KITCHEN_Order]";
        private string tb_routedorder = "[AlohaReporting].[dbo].[kitchen_routedorder]";

        static Dictionary<string, int[]> dict_1 = new Dictionary<string, int[]>();
        static Dictionary<string, string> dict_2 = new Dictionary<string, string>();

        static AkServerDao()
        {
            dict_1["auto"] = new int[] { 101, 102, 103, 104, 9 };
            dict_1["wais"] = new int[] { 4, 6 };
            dict_1["guit"] = new int[] { 101, 102, 103, 104, 9, 4, 6 };

            dict_2["auto"] = "101,102,103,104,9";
            dict_2["wais"] = "4,6";
            dict_2["guit"] = "101,102,103,104,9,4,6";
        }

        #region ----------------服务区报表数据------------------
        #region 【营业额】
        /// <summary>
        /// 营业额
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public int GetSalesTurnover(string startTime, string endTime)
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
                    return (int)query.Sum(l => l.AMOUNT);
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

        #region 【订单数量】
        /// <summary>
        /// 添加行数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="header"></param>
        /// <param name="model"></param>
        private void addRowData(DataTable dt, AkOrderModel model)
        {
            try
            {
                DataRow dr = dt.NewRow();
                dr["OrderMode"] = model.OrderMode;
                dr["Terminal"] = model.Terminal;
                dr["Value"] = model.Value;
                dt.Rows.Add(dr);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(model.UserId + "\n" + ex.ToString());
            }
        }


        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="lt_source"></param>
        /// <returns></returns>
        public DataTable GetTable()
        {
            DataTable dt = new DataTable();
            dt.TableName = "table";
            dt.Columns.Add(new DataColumn() { Caption = "OrderMode", ColumnName = "OrderMode", DataType = System.Type.GetType("System.Int32") });
            dt.Columns.Add(new DataColumn() { Caption = "Terminal", ColumnName = "Terminal", DataType = System.Type.GetType("System.Int32") });
            dt.Columns.Add(new DataColumn() { Caption = "Value", ColumnName = "Value", DataType = System.Type.GetType("System.Int32") });


            List<AkOrderModel> lt_result = new List<AkOrderModel>();
            lt_result.Add(new AkOrderModel() { OrderMode = 1, Terminal = 1, Value = 10 });
            lt_result.Add(new AkOrderModel() { OrderMode = 1, Terminal = 5, Value = 10 });
            lt_result.Add(new AkOrderModel() { OrderMode = 1, Terminal = 7, Value = 10 });
            lt_result.Add(new AkOrderModel() { OrderMode = 1, Terminal = 9, Value = 10 });
            lt_result.Add(new AkOrderModel() { OrderMode = 2, Terminal = 1, Value = 10 });
            lt_result.Add(new AkOrderModel() { OrderMode = 2, Terminal = 5, Value = 10 });
            lt_result.Add(new AkOrderModel() { OrderMode = 2, Terminal = 7, Value = 10 });
            lt_result.Add(new AkOrderModel() { OrderMode = 2, Terminal = 9, Value = 10 });
            lt_result.Add(new AkOrderModel() { OrderMode = 3, Terminal = 1, Value = 10 });
            lt_result.Add(new AkOrderModel() { OrderMode = 3, Terminal = 5, Value = 10 });
            lt_result.Add(new AkOrderModel() { OrderMode = 3, Terminal = 7, Value = 10 });
            lt_result.Add(new AkOrderModel() { OrderMode = 3, Terminal = 9, Value = 10 });

            foreach (var item in lt_result)
            {
                addRowData(dt, item);
            }

            return dt;
        }

        /// <summary>
        /// 获取订单数
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Dictionary<string, decimal> GetOrderNumber(string startTime, string endTime)
        {
            Dictionary<string, decimal> result = new Dictionary<string, decimal>();

            string sql = "select aa.terminal, count(*) Value ";
            sql += " from (select distinct a.posorderid, b.Terminal,b.dateofbusiness";
            sql += " from [AlohaReporting].[dbo].[KITCHEN_Item] a, [AlohaReporting].[dbo].[KITCHEN_Order] b";
            sql += " where a.posorderid=b.posid and a.DateOfBusiness=b.DateOfBusiness ";
            sql += " and convert(varchar(100),a.dateofbusiness,23) between  '" + AkConfig.StartDate + "' and '" + AkConfig.EndDate + "' ";
            sql += " and CONVERT(varchar(100), b.TimeStarted, 108) between '" + startTime + "' and '" + endTime + "'  and a.Canceled=0 ";
            sql += " ) aa ";
            sql += " group by aa.Terminal ";

            var dt = SqlServerHelper.Instance.GetDataTable(AkConfig.ConnAlohaReporting, sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                var query = (from t in dt.AsEnumerable()
                             select new AkOrderModel
                             {
                                 Terminal = t.Field<int>("Terminal"),
                                 Value = t.Field<int>("Value")
                             }).ToList();

                if (query != null && query.Count() > 0)
                {
                    result["auto"] = query.Where(l => dict_1["auto"].Contains(l.Terminal)).Sum(s => s.Value);
                    result["wais"] = query.Where(l => dict_1["wais"].Contains(l.Terminal)).Sum(s => s.Value);
                    result["guit"] = query.Where(l => !dict_1["guit"].Contains(l.Terminal)).Sum(s => s.Value);
                    result["all"] = query.Sum(s => s.Value);
                }
            }
            return result;
        }
        #endregion

        #region【产品平均数】
        /// <summary>
        /// 产品平均数
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public int GetAvgProduct(Dictionary<string, decimal> dict_order, string startTime, string endTime)
        {
            string sql = " select count(*) Value";
            sql += " from [AlohaReporting].[dbo].[KITCHEN_Order]aa,[AlohaReporting].[dbo].[KITCHEN_Item]bb ";
            sql += " where aa.posid=bb.posorderid and aa.dateofbusiness=bb.dateofbusiness ";
            sql += " and bb.canceled=0 and not bb.itemnumber=800032 ";
            sql += "  and CONVERT(varchar(100), aa.TimeStarted, 108) between '" + startTime + "' and '" + endTime + "'";
            sql += " and convert(varchar(100),aa.dateofbusiness,23) between '" + AkConfig.StartDate + "' and '" + AkConfig.EndDate + "' ";

            var obj = SqlServerHelper.Instance.ExecuteScalar(AkConfig.ConnAlohaReporting, sql);
            int avg = 0;
            if (obj != null)
            {
                avg = Convert.ToInt32(obj);
            }
            return avg;
        }
        #endregion

        #region 【点餐时间】
        /// <summary>
        /// 点餐时间
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Dictionary<string, decimal> GetOrderTime(Dictionary<string, decimal> dict_order, string startTime, string endTime)
        {
            string sql = "select   aa.Terminal,sum(aa.lasttimesent-aa.firsttimesent) value FROM (select distinct a.posorderid, b.Terminal,b.lasttimesent,b.firsttimesent,b.dateofbusiness ";
            sql += " from [AlohaReporting].[dbo].[KITCHEN_Item] a, [AlohaReporting].[dbo].[KITCHEN_Order] b ";
            sql += " where a.posorderid=b.posid and a.DateOfBusiness=b.DateOfBusiness ";
            sql += " and convert(varchar(100),a.dateofbusiness,23) between  '" + AkConfig.StartDate + "' and '" + AkConfig.EndDate + "' ";
            sql += " and CONVERT(varchar(100), b.TimeStarted, 108) between '" + startTime + "' and '" + endTime + "'   and a.Canceled=0 ";
            sql += " ) aa ";
            sql += " group by aa.Terminal ";

            return getValues(dict_order, sql);

        }
        #endregion

        #region 【收银时间】
        /// <summary>
        /// 收银时间
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Dictionary<string, decimal> GetCashTime(Dictionary<string, decimal> dict_order, string startTime, string endTime)
        {
            Dictionary<string, decimal> result = new Dictionary<string, decimal>();

            string sql = " select  aa.Terminal,sum(aa.TimePaid-aa.LastTimeSent+aa.firsttimesent) value ";
            sql += " FROM (select distinct a.posorderid, b.Terminal,b.lasttimesent,b.firsttimesent,b.timepaid,b.dateofbusiness ";

            sql += " from [AlohaReporting].[dbo].[KITCHEN_Item] a, [AlohaReporting].[dbo].[KITCHEN_Order] b ";
            sql += " where a.posorderid=b.posid and a.DateOfBusiness=b.DateOfBusiness  ";
            sql += " and convert(varchar(100),a.dateofbusiness,23) between  '" + AkConfig.StartDate + "' and '" + AkConfig.EndDate + "' ";
            sql += " and CONVERT(varchar(100), b.TimeStarted, 108) between '" + startTime + "' and '" + endTime + "' and a.Canceled=0  and b.timepaid>0 ";
            sql += ") aa ";
            sql += "group by aa.Terminal ";

            return getValues(dict_order, sql);
        }
        #endregion

        #region 【备餐时间】
        /// <summary>
        /// 备餐时间
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Dictionary<string, decimal> GetPrepareTime(Dictionary<string, decimal> dict_order, string startTime, string endTime)
        {
            string sql = " select  aa.Terminal,sum(aa.timebumped-aa.timeactivated) Value ";
            sql += " FROM (select distinct a.posorderid, b.Terminal,c.timebumped,c.timeactivated,b.dateofbusiness ";
            sql += " from [AlohaReporting].[dbo].[KITCHEN_Item] a, [AlohaReporting].[dbo].[KITCHEN_Order] b, [AlohaReporting].[dbo] ";
            sql += " .[kitchen_routedorder] c ";
            sql += " where a.posorderid=b.posid and a.DateOfBusiness=b.DateOfBusiness  ";
            sql += " and convert(varchar(100),a.dateofbusiness,23) between  '" + AkConfig.StartDate + "' and '" + AkConfig.EndDate + "' ";
            sql += " and CONVERT(varchar(100), b.TimeStarted, 108) between '" + startTime + "' and '" + endTime + "' and a.Canceled=0  ";
            sql += " and b.id=c.orderid and c.station in(1,10) and b.dateofbusiness=c.dateofbusiness ";
            sql += " ) aa ";
            sql += " group by aa.Terminal ";

            return getValues(dict_order, sql);
        }
        #endregion

        #region 【汇餐时间】
        /// <summary>
        /// 汇餐时间
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Dictionary<string, decimal> GetRemitTime(Dictionary<string, decimal> dict_order, string startTime, string endTime)
        {
            //string sql = " select aaa.Terminal,sum(aaa.timebumped-bbb.timebumped) Value from ";
            //sql += " (select distinct a.posorderid,b.id, b.Terminal,c.timebumped,b.dateofbusiness ";
            //sql += "  from [AlohaReporting].[dbo].[KITCHEN_Item] a, [AlohaReporting].[dbo].[KITCHEN_Order] b, [AlohaReporting].[dbo].[kitchen_routedorder] c ";
            //sql += "  where a.posorderid=b.posid and a.DateOfBusiness=b.DateOfBusiness  ";
            //sql += "  and convert(varchar(100),b.dateofbusiness,23) between  '" + AkConfig.StartDate + "' and '" + AkConfig.EndDate + "' ";
            //sql += "  and CONVERT(varchar(100), b.TimeStarted, 108) between '" + startTime + "' and '" + endTime + "' and a.Canceled=0  ";
            //sql += "  and b.id=c.orderid and c.station=5 and b.dateofbusiness=c.dateofbusiness ";
            //sql += " )aaa,(select distinct a.posorderid,b.id, b.Terminal,c.timebumped,b.dateofbusiness ";
            //sql += "  from [AlohaReporting].[dbo].[KITCHEN_Item] a, [AlohaReporting].[dbo].[KITCHEN_Order] b, [AlohaReporting].[dbo].[kitchen_routedorder] c ";
            //sql += "  where a.posorderid=b.posid and a.DateOfBusiness=b.DateOfBusiness  ";
            //sql += "  and convert(varchar(100),b.dateofbusiness,23) between  '" + AkConfig.StartDate + "' and '" + AkConfig.EndDate + "' ";
            //sql += "  and CONVERT(varchar(100), b.TimeStarted, 108) between '" + startTime + "' and '" + endTime + "' and a.Canceled=0  ";
            //sql += "  and b.id=c.orderid and c.station=1 and b.dateofbusiness=c.dateofbusiness ";
            //sql += " )bbb ";
            //sql += " where aaa.id=bbb.id ";
            //sql += " group by aaa.Terminal ";

            string sql = " select distinct c.station,a.posorderid,b.id OrderId, b.Terminal,c.timebumped,b.dateofbusiness ";
            sql += "  from [AlohaReporting].[dbo].[KITCHEN_Item] a, [AlohaReporting].[dbo].[KITCHEN_Order] b, [AlohaReporting].[dbo].[kitchen_routedorder] c ";
            sql += "  where a.posorderid=b.posid and a.DateOfBusiness=b.DateOfBusiness  ";
            sql += "  and convert(varchar(100),b.dateofbusiness,23) between  '" + AkConfig.StartDate + "' and '" + AkConfig.EndDate + "' ";
            sql += "  and CONVERT(varchar(100), b.TimeStarted, 108) between '" + startTime + "' and '" + endTime + "' and a.Canceled=0  ";
            sql += "  and b.id=c.orderid and c.station in(1,5) and b.dateofbusiness=c.dateofbusiness ";

            Dictionary<string, decimal> dict_order_back = new Dictionary<string, decimal>();
            if(dict_order.ContainsKey("all"))
            {
                dict_order_back["auto"] = dict_order["auto"];
                dict_order_back["guit"] = dict_order["guit"];
                dict_order_back["wais"] = 0;
                dict_order_back["all"] = dict_order_back["auto"] + dict_order_back["guit"];
            }
  
            return remitTime(dict_order_back, sql);
        }
        #endregion

        #region 【整体时间】
        /// <summary>
        /// 整体时间
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Dictionary<string, decimal> GetWholeTime(Dictionary<string, decimal> dict_order, string startTime, string endTime)
        {
            string sql = " select  aa.Terminal,sum(aa.timebumped-aa.timeactivated) Value";
            sql += " FROM (select distinct a.posorderid, b.Terminal,c.timebumped,c.timeactivated,b.dateofbusiness  ";
            sql += " from [AlohaReporting].[dbo].[KITCHEN_Item] a, [AlohaReporting].[dbo].[KITCHEN_Order] b, [AlohaReporting].[dbo].[kitchen_routedorder] c  ";
            sql += " where a.posorderid=b.posid and a.DateOfBusiness=b.DateOfBusiness   ";
            sql += " and convert(varchar(100),a.dateofbusiness,23) between '" + AkConfig.StartDate + "' and '" + AkConfig.EndDate + "' ";
            sql += " and CONVERT(varchar(100), b.TimeStarted, 108) between '" + startTime + "' and '" + endTime + "' and a.Canceled=0   ";
            sql += " and b.id=c.orderid and c.station in(5,10) and b.dateofbusiness=c.dateofbusiness  ";
            sql += " ) aa ";
            sql += " group by aa.Terminal ";

            return getValues(dict_order, sql);
        }
        #endregion

        #region 【超时订单%】
        /// <summary>
        /// 超时订单
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public string GetTimeOut(Dictionary<string, decimal> dict_order, string startTime, string endTime)
        {
            string sql = " select count(*) Value";
            sql += " FROM (SELECT distinct bb.posorderid ,(cc.timebumped-cc.timeactivated)ActualTime,bb.dateofbusiness ";
            sql += " from [AlohaReporting].[dbo].[KITCHEN_Order]aa, [AlohaReporting].[dbo].[KITCHEN_Item] bb ";
            sql += " , [AlohaReporting].[dbo].[kitchen_routedorder] cc ";
            sql += " where aa.posid=bb.posorderid and aa.dateofbusiness=bb.dateofbusiness ";
            sql += " and CONVERT(varchar(100), aa.TimeStarted, 108) between '" + startTime + "' and '" + endTime + "'";
            sql += " and bb.canceled=0 and bb.dateofbusiness=cc.dateofbusiness ";
            sql += " and aa.id=cc.orderid and cc.station in(5,10) ";
            sql += " and convert(varchar(100),aa.dateofbusiness,23) between '" + AkConfig.StartDate + "' and '" + AkConfig.EndDate + "' ";
            sql += " ) aaa, CFCInStoreDB.[dbo].[Course] bbb ";
            sql += " Where aaa.ActualTime>bbb.FirstAlertSeconds and bbb.number=4 ";

            var obj = SqlServerHelper.Instance.ExecuteScalar(AkConfig.ConnAlohaReporting, sql);

            if (obj != null)
            {
                int timeOut = Convert.ToInt32(obj);
                int timeOut2 = GetTimeOutStation1(startTime, endTime);

                if (dict_order != null && dict_order.ContainsKey("all"))
                {
                    return (((timeOut + timeOut2) / dict_order["all"]) * 100).ToString("0.00") + "%";
                }
            }
            return null;
        }

        public int GetTimeOutStation1(string startTime, string endTime)
        {
            string sql = " select count(*) Value";
            sql += " FROM (SELECT distinct bb.posorderid ,(cc.timebumped-cc.timeactivated)ActualTime,bb.dateofbusiness ";
            sql += " from [AlohaReporting].[dbo].[KITCHEN_Order]aa, [AlohaReporting].[dbo].[KITCHEN_Item] bb ";
            sql += " , [AlohaReporting].[dbo].[kitchen_routedorder] cc ";
            sql += " where aa.posid=bb.posorderid and aa.dateofbusiness=bb.dateofbusiness ";
            sql += " and CONVERT(varchar(100), aa.TimeStarted, 108) between '" + startTime + "' and '" + endTime + "'";
            sql += " and bb.canceled=0 and bb.dateofbusiness=cc.dateofbusiness ";
            sql += " and aa.id=cc.orderid and cc.station=1 and aa.ordermode=3 ";
            sql += " and convert(varchar(100),aa.dateofbusiness,23) between '" + AkConfig.StartDate + "' and '" + AkConfig.EndDate + "' ";
            sql += " ) aaa, CFCInStoreDB.[dbo].[Course] bbb ";
            sql += " Where aaa.ActualTime>bbb.FirstAlertSeconds and bbb.number=4 ";

            var obj = SqlServerHelper.Instance.ExecuteScalar(AkConfig.ConnAlohaReporting, sql);

            int result = 0;
            if (obj != null)
            {
                result = Convert.ToInt32(obj);
            }
            return result;
        }
        #endregion

        #region 【私有方法】
        private Dictionary<string, decimal> getValues(Dictionary<string, decimal> dict_order, string sql)
        {
            Dictionary<string, decimal> result = new Dictionary<string, decimal>();

            var dt = SqlServerHelper.Instance.GetDataTable(AkConfig.ConnAlohaReporting, sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                var query = (from t in dt.AsEnumerable()
                             select new AkOrderModel
                             {
                                 Terminal = t.Field<int>("Terminal"),
                                 Value = t.Field<int>("Value")
                             }).ToList();

                return getValues(dict_order, query);
            }

            return null;
        }

        private Dictionary<string, decimal> getValues(Dictionary<string, decimal> dict_order, List<AkOrderModel> query)
        {
            Dictionary<string, decimal> result = new Dictionary<string, decimal>();
            if (query != null && query.Count() > 0)
            {
                result["auto"] = query.Where(l => dict_1["auto"].Contains(l.Terminal)).Sum(s => s.Value);
                if (dict_order["auto"] > 0) result["auto"] = result["auto"] / dict_order["auto"];
                else result["auto"] = 0;

                result["wais"] = query.Where(l => dict_1["wais"].Contains(l.Terminal)).Sum(s => s.Value);
                if (dict_order["wais"] > 0) result["wais"] = result["wais"] / dict_order["wais"];
                else result["wais"] = 0;

                result["guit"] = query.Where(l => !dict_1["guit"].Contains(l.Terminal)).Sum(s => s.Value);
                if (dict_order["guit"] > 0) result["guit"] = result["guit"] / dict_order["guit"];
                else result["guit"] = 0;

                result["all"] = query.Sum(s => s.Value);
                if (dict_order["all"] > 0) result["all"] = result["all"] / dict_order["all"];
                else result["all"] = 0;
            }
            return result;
        }

        /// <summary>
        /// 汇餐时间
        /// </summary>
        /// <param name="dict_order"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        private Dictionary<string, decimal> remitTime(Dictionary<string, decimal> dict_order, string sql)
        {
            Dictionary<string, decimal> result1 = new Dictionary<string, decimal>();
            Dictionary<string, decimal> result5 = new Dictionary<string, decimal>();
            Dictionary<string, decimal> result = new Dictionary<string, decimal>();

            var dt = SqlServerHelper.Instance.GetDataTable(AkConfig.ConnAlohaReporting, sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                var query = (from t in dt.AsEnumerable()
                             select new AkTempModel
                             {
                                 Station = t.Field<int>("Station"),
                                 OrderId = t.Field<System.Guid>("OrderId").ToString("D"),
                                 Terminal = t.Field<int>("Terminal"),
                                 Timebumped = t.Field<int>("Timebumped")
                             }).ToList();

                if (query != null && query.Count() > 0)
                {
                    Dictionary<string, AkTempModel> remit1 = new Dictionary<string, AkTempModel>();
                    Dictionary<string, AkTempModel> remit2 = new Dictionary<string, AkTempModel>();

                    var list1 = query.Where(l => l.Station == 5);
                    if (list1 != null && list1.Count() > 0)
                    {
                        remit1 = list1.ToDictionary(key => key.OrderId, value => value);
                    }

                    var list2 = query.Where(l => l.Station == 1);
                    if (list2 != null && list2.Count() > 0)
                    {
                        remit2 = list2.ToDictionary(key => key.OrderId, value => value);
                    }

                    List<AkOrderModel> list_result = new List<AkOrderModel>();

                    if (remit1.Count > 0 && remit2.Count > 0)
                    {
                        foreach (string key in remit1.Keys)
                        {
                            if (remit2.Keys.Contains(key))
                            {
                                AkOrderModel model = new AkOrderModel();
                                AkTempModel temp1 = remit1[key];
                                AkTempModel temp2 = remit2[key];

                                model.Terminal = temp1.Terminal;
                                model.Value = temp1.Timebumped - temp2.Timebumped;
                                list_result.Add(model);
                            }
                        }

                        if (list_result.Count > 0)
                        {
                            return getValues(dict_order, list_result);
                        }
                    }
                }
            }
            return null;
        }
        #endregion
        #endregion
    }
}