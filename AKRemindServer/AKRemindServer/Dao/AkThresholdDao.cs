using AKRemindServer.DB;
using AKRemindServer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AKRemindServer.Dao
{
    /// <summary>
    /// 阀值Dao操作
    /// </summary>
    public class AkThresholdDao
    {
        /// <summary>
        /// 数据库名称
        /// </summary>
        private string dbName = "AlohaRemind";

        /// <summary>
        /// 阀值记录表名
        /// </summary>
        private string tableName = "[CFCInStoreDB].[dbo].[tb_threshold]";

       

        /// <summary>
        /// 索引名称
        /// </summary>
        private string indexName = "tb_threshold_index_datadate";


        #region ----------------获取周期内所有类型，订单数 及每个 item的制作时间----------------------
        /// <summary>
        /// 获取周期内，字段“Item”所有类型，各自的Qulity总数
        /// </summary>
        public List<ItemQuantity> GetItemsQulity(AkRepastModel model)
        {
            List<ItemQuantity> ltResult = new List<ItemQuantity>();

            string sql = string.Empty;
            sql += "SELECT bb.ItemNumber ITEM,count(*)QUANTITY ";
            sql += "from [AlohaReporting].[dbo].[KITCHEN_Order]aa,[AlohaReporting].[dbo].[KITCHEN_Item]bb,[AlohaReporting].[dbo].[KITCHEN_RoutedItem] cc ";
            sql += "where aa.posid=bb.posorderid and aa.dateofbusiness=bb.dateofbusiness and bb.dateofbusiness=cc.dateofbusiness ";
            sql += "and convert(varchar(100),aa.dateofbusiness,23) between '" + AkConfig.StartDate + "' and '" + AkConfig.EndDate + "'";
            sql += "and CONVERT(varchar(100), aa.TimeStarted, 108) between  '" + model.StartTime + "' and '" + model.EndTime + "'";
            sql += "and bb.canceled=0 and bb.id=cc.itemid and cc.station in(2,3) ";
            sql += "group by bb.itemnumber";

            //@1.获取周期内字段“ITEM”起始字符为1的所有dbf记录
            DataTable dt = SqlServerHelper.Instance.GetDataTable(AkConfig.ConnAlohaReporting, sql);

            //@2.分组出每个"Item"的 QUANTITY 之和
            var result = from t in dt.AsEnumerable()
                         //group t by new { item = t.Field<int>("ITEM") } into m
                         select new ItemQuantity
                         {
                             Item = t.Field<int>("ITEM"),
                             Quantity = (double)t.Field<int>("QUANTITY")
                         };
            if (result != null && result.Count() > 0)
            {
                ltResult = result.ToList();
            }
            return ltResult;
        }

        /// <summary>
        /// 求出item的数量总数
        /// </summary>
        /// <param name="itemQuantitys"></param>
        /// <returns></returns>
        public double GetTotalQulitys(List<ItemQuantity> itemQuantitys)
        {
            return itemQuantitys.Sum(l => l.Quantity);
        }

        /// <summary>
        /// 获取每个Item的Cooktime
        /// </summary>
        /// <param name="dateTiime"></param>
        /// <returns></returns>
        public List<ItemCookTime> GetItemsCookTime()
        {
            List<ItemCookTime> ltResult = new List<ItemCookTime>();
            Dictionary<int, int> dict = new Dictionary<int, int>();

            string sql = " select item.number item, itemcooktimeitem.cooktime ";
            sql += "from CFCInStoreDB.dbo.item, CFCInStoreDB.dbo.itemcooktimeitem ";
            sql += "where item.id= itemcooktimeitem.fk_itemid";
            DataTable dt = SqlServerHelper.Instance.GetDataTable(AkConfig.ConnCFCInStoreDB, sql);

            //模拟数据
            //DataTable dt = new DataTable();
            //dt.Columns.Add("Item", typeof(Int32));
            //dt.Columns.Add("cooktime", typeof(Int32));
            //DataRow dr1 = dt.NewRow();
            //dr1["Item"] = 110001;
            //dr1["cooktime"] = 120;
            //dt.Rows.Add(dr1);

            //DataRow dr2 = dt.NewRow();
            //dr2["Item"] = 110005;
            //dr2["cooktime"] = 100;
            //dt.Rows.Add(dr2);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ltResult.Add(new ItemCookTime()
                    {
                        Item = dr.Field<int>("Item"),
                        CookTime = dr.Field<int>("Cooktime")
                    });
                }
            }
            return ltResult;
        }
        #endregion

        #region -------------取值逻辑-----------------
        /// <summary>
        /// 取 各分屏的 quantity
        /// </summary>
        /// <param name="dateTiime"></param>
        /// <returns></returns>
        public List<StationQuantity> GetStationQuantity(string dateTiime)
        {
            List<StationQuantity> ltResult = new List<StationQuantity>();
            Dictionary<int, int> dict = new Dictionary<int, int>();
            string sql = "SELECT cc.station,count(*)quantity  ";
            sql += "from [AlohaReporting].[dbo].[KITCHEN_Order]aa,";
            sql += "[AlohaReporting].[dbo].[KITCHEN_Item]  bb,";
            sql += "[AlohaReporting].[dbo].[KITCHEN_RoutedItem] cc ";
            sql += "where aa.posid=bb.posorderid and aa.dateofbusiness=bb.dateofbusiness ";
            sql += " and aa.timestarted >= '" + dateTiime + "' and bb.canceled=0 and bb.dateofbusiness=cc.dateofbusiness and bb.id=cc.itemid and cc.station in(2,3) group by cc.station ";
            DataTable dt = SqlServerHelper.Instance.GetDataTable(AkConfig.ConnAlohaReporting, sql);


            //DataTable dt = new DataTable();
            //dt.TableName = "QUANTITY";
            //dt.Columns.Add("station", typeof(int));
            //dt.Columns.Add("quantity", typeof(int));

            //DataRow dr1 = dt.NewRow();
            //dr1["station"] = 2;
            //dr1["quantity"] = 30;
            //dt.Rows.Add(dr1);

            //DataRow dr2 = dt.NewRow();
            //dr2["station"] = 3;
            //dr2["quantity"] = 70;
            //dt.Rows.Add(dr2);

            var result = from t in dt.AsEnumerable()
                         select new StationQuantity
                         {
                             Station = t.Field<int>("station"),

                             Quantity = t.Field<int>("quantity")
                         };
            if (result != null && result.Count() > 0)
            {
                ltResult = result.ToList();
            }
            return ltResult;
        }
        #endregion

        #region ----------------阀值------------------

        #region 创建库
        /// <summary>
        /// 创建库
        /// </summary>
        /// <param name="tableName"></param>
        public void CreateDataBase(string dbName)
        {
            //建表
            if (!SqlServerHelper.Instance.ExistDatabase(AkConfig.ConnAlohaReporting, dbName))
            {
                string sql = string.Format("create database {0}", dbName);
                SqlServerHelper.Instance.ExecuteNonQuery(AkConfig.ConnAlohaReporting, sql);
            }
        }
        #endregion

        #region 创建阀值表
        /// <summary>
        /// 创建阀值表
        /// </summary>
        /// <param name="tableName"></param>
        public void CreateTable()
        {
            //建表
            if (!SqlServerHelper.Instance.ExistTable(AkConfig.ConnCFCInStoreDB, "tb_threshold"))
            {
                string sql = string.Format("CREATE TABLE {0} ( " +
                                           @"Id            INTEGER  IDENTITY(1,1)," +
                                           //@"StoreID                CHAR( 20 )    NOT NULL,	     " +
                                           @"DataDate          CHAR( 10 )      NOT NULL," +
                                           @"BreakFastValue    INT            NOT NULL," +
                                           @"LunchValue    INT            NOT NULL," +
                                           @"AfternoonTeaValue    INT            NOT NULL," +
                                           @"SupperValue    INT            NOT NULL," +
                                           @"PeriodNum         INT            NOT NULL," +
                                           @"PeriodStartDate   CHAR( 10 )      NOT NULL," +
                                           @"PeriodEndDate     CHAR( 10 )      NOT NULL," +
                                           @"CreateTime        CHAR( 23 )     NOT NULL," +
                                           @"UpdateTime        CHAR( 23 )     NOT NULL)", this.tableName);
                SqlServerHelper.Instance.ExecuteNonQuery(AkConfig.ConnCFCInStoreDB, sql);
            }
            else
            {
                //if (!SqlServerHelper.Instance.ExistColumn(AkConfig.ConnCFCInStoreDB, "tb_threshold","StoreID"))
                //{
                //    string sqlColumn = "alter table tb_threshold add StoreID char(20) NOT NULL DEFAULT ''";
                //    SqlServerHelper.Instance.ExecuteNonQuery(AkConfig.ConnCFCInStoreDB, sqlColumn);
                //}
            }
        }
        #endregion

        #region 创建索引
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="tableName"></param>
        public void CreateIndex()
        {
            if (!SqlServerHelper.Instance.ExistIndex(AkConfig.ConnCFCInStoreDB, this.indexName))
            {
                object[] param = new object[] { this.indexName, this.tableName };
                string sql = string.Format("CREATE INDEX {0} ON {1} (DataDate)", param);
                SqlServerHelper.Instance.ExecuteNonQuery(AkConfig.ConnCFCInStoreDB, sql);
            }
        }
        #endregion

        #region ---------------读取阀值------------
        /// <summary>
        /// 获取最新阀值数据
        /// </summary>
        /// <returns></returns>
        public AkThresholdModel GetNewThreshold()
        {
            AkThresholdModel mode = null;
            string sql = string.Format("select top(1) * from {0} order by DataDate desc", this.tableName);
            DataTable dt = SqlServerHelper.Instance.GetDataTable(AkConfig.ConnAlohaReporting, sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                mode = new AkThresholdModel()
                {
                    Id = dr.Field<int>("Id"),
                    DataDate = dr.Field<string>("DataDate"),
                    BreakFastValue = dr.Field<int>("BreakFastValue"),
                    LunchValue = dr.Field<int>("LunchValue"),
                    AfternoonTeaValue = dr.Field<int>("AfternoonTeaValue"),
                    SupperValue = dr.Field<int>("SupperValue"),

                    PeriodNum = dr.Field<int>("PeriodNum"),
                    PeriodStartDate = dr.Field<string>("PeriodStartDate"),
                    PeriodEndDate = dr.Field<string>("PeriodEndDate"),
                    CreateTime = dr.Field<string>("CreateTime"),
                    UpdateTime = dr.Field<string>("UpdateTime")
                };
            }
            return mode;
        }
        #endregion

        #region ---------------新增或修改阀值------------
        /// <summary>
        /// 新增、修改阀值
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Save(AkThresholdModel model)
        {
            string sql = string.Format("SELECT count(1) FROM {0} where DataDate ='{1}'", this.tableName, model.DataDate);

            //记录存在则修改、否则新增
            bool exist = SqlServerHelper.Instance.ExistRecode(AkConfig.ConnAlohaReporting, sql);

            object[] param = null;
            if (exist)
            {
                param = new object[] { this.tableName, model.BreakFastValue, model.LunchValue, model.AfternoonTeaValue, model.SupperValue, model.PeriodNum, model.PeriodStartDate, model.PeriodEndDate, model.UpdateTime, model.DataDate };
                sql = string.Format("Update {0} set  " +
                                    "BreakFastValue     ={1}," +
                                    "LunchValue         ={2}," +
                                    "AfternoonTeaValue  ={3}," +
                                    "SupperValue        ={4}," +
                                    "PeriodNum          ={5}," +
                                    "PeriodStartDate    ='{6}'," +
                                    "PeriodEndDate      ='{7}'," +
                                    "UpdateTime         ='{8}'" +
                                    "where DataDate     ='{9}'", param);
            }
            else
            {
                param = new object[] { this.tableName, model.DataDate, model.BreakFastValue, model.LunchValue, model.AfternoonTeaValue, model.SupperValue, model.PeriodNum, model.PeriodStartDate, model.PeriodEndDate, model.CreateTime, model.UpdateTime };
                sql = string.Format("insert into {0}(DataDate,BreakFastValue,LunchValue,AfternoonTeaValue,SupperValue,PeriodNum,PeriodStartDate,PeriodEndDate,CreateTime,UpdateTime) " +
                                      @"VALUES('{1}',{2},{3},{4},{5},{6},'{7}','{8}','{9}','{10}')", param);
            }
            return SqlServerHelper.Instance.ExecuteNonQuery(AkConfig.ConnAlohaReporting, sql) > 0 ? true : false;
        }
        #endregion
        #endregion
    }
}
