using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using AKRemindServer.Models;
using AKRemindServer.DB;

namespace AKRemindServer.Dao
{
    /// <summary>
    /// 分线提醒Dao操作
    /// </summary>
    public class AkRemindDao
    {
        /// <summary>
        /// 分线提醒记录表名
        /// </summary>
        private string tableName = "[CFCInStoreDB].[dbo].[tb_remind]";

        /// <summary>
        /// 索引名称
        /// </summary>
        private string indexName = "tb_remind_index_id";

        #region ----------------分线提醒------------------
        /// <summary>
        /// 创建分线提醒记录表
        /// </summary>
        /// <param name="tableName"></param>
        public void CreateTable()
        {
            if (!SqlServerHelper.Instance.ExistTable(AkConfig.ConnCFCInStoreDB, "tb_remind"))
            {
                string sql = string.Format("CREATE TABLE {0} ( " +
                            @"Id                INT  IDENTITY(1,1)," +
                            //@"StoreID                CHAR( 20 )    NOT NULL,	     " +
                            @"Tasktime          CHAR( 23 )     NOT NULL,	     " +
                    //@"BreakFastValue    INT            NOT NULL,	     " +
                    //@"LunchValue        INT            NOT NULL,	     " +
                    //@"AfternoonTeaValue INT            NOT NULL,	     " +
                    //@"SupperValue       INT            NOT NULL,	     " +
                            @"IsRemind          INT            NOT NULL,	     " +
                            @"HasRemind         INT            NOT NULL,	     " +
                            @"PeriodNum         INT            NOT NULL)", this.tableName);

                SqlServerHelper.Instance.ExecuteNonQuery(AkConfig.ConnCFCInStoreDB, sql);
            }
            //else
            //{
            //    if (!SqlServerHelper.Instance.ExistColumn(AkConfig.ConnCFCInStoreDB, "tb_remind", "StoreID"))
            //    {
            //        string sqlColumn = "alter table tb_remind add StoreID char(20) NOT NULL DEFAULT ''";
            //        SqlServerHelper.Instance.ExecuteNonQuery(AkConfig.ConnCFCInStoreDB, sqlColumn);
            //    }
            //}
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="tableName"></param>
        public void CreateIndex()
        {
            if (!SqlServerHelper.Instance.ExistIndex(AkConfig.ConnCFCInStoreDB, "tb_remind_index_id"))
            {
                object[] param = new object[] { this.indexName, this.tableName };
                string sql = string.Format("CREATE INDEX {0} ON {1} (Id)", param);
                SqlServerHelper.Instance.ExecuteNonQuery(AkConfig.ConnCFCInStoreDB, sql);
            }
        }

        #region ---------------读取分线提醒记录------------
        /// <summary>
        /// 获取最大id
        /// </summary>
        /// <returns></returns>
        public AkRemindModel GetNewData()
        {
            AkRemindModel mode = null;
            string sql = string.Format("select top (1) Id,IsRemind from {0} order by id desc", this.tableName);
            DataTable dt = SqlServerHelper.Instance.GetDataTable(AkConfig.ConnCFCInStoreDB, sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                mode = new AkRemindModel();
                mode.Id = dr.Field<int>("Id");
                mode.IsRemind = dr.Field<int>("IsRemind");
            }
            return mode;
        }
        #endregion

        #region ---------------新增或修改分线提醒------------
        /// <summary>
        /// 新增分线提醒记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Save(AkRemindModel model)
        {
            object[] param = new object[] { this.tableName, model.Tasktime, model.IsRemind, model.HasRemind, model.PeriodNum };
            string sql = string.Format("insert into {0}(Tasktime,IsRemind,HasRemind,PeriodNum) " +
                                                  @"VALUES('{1}',{2},{3},{4})", param);
            return SqlServerHelper.Instance.ExecuteNonQuery(AkConfig.ConnCFCInStoreDB, sql) > 0 ? true : false;
        }

        /// <summary>
        /// 回写：是否已分线
        /// 1.如果上一条记录已提醒分线，则进行回写(0 已分线，1 未分线)
        ///    如果上一条未提醒分线，则不会写
        /// 2.回写：已提醒分线
        ///    1).如果本次提醒分线，则回写 为：1(未分线)
        ///    2).如果本次未提醒分线，但是并未开两条线，则回写 为：1(未分线)
        ///    3).其它情况：则回写为：0(已分线)
        /// </summary>
        /// <param name="isRemind"></param>
        /// <param name="lineCount"></param>
        /// <returns></returns>
        public bool ResetHasRemind(bool isHealth)
        {
            //获取最大Id
            AkRemindModel model = GetNewData();
            if (model != null && model.IsRemind == 1)
            {
                object[] param = new object[] { this.tableName, isHealth ? 0 : 1, model.Id };
                string sql = string.Format("Update {0} set  " +
                                        "HasRemind    = {1} " +
                                        "where Id     = {2} ", param);
                return SqlServerHelper.Instance.ExecuteNonQuery(AkConfig.ConnCFCInStoreDB, sql) > 0 ? true : false;
            }
            return true;
        }
        #endregion
        #endregion
    }
}
