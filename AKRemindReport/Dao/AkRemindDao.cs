using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using AKRemindReport.Models;
using AKRemindReport.DB;

namespace AKRemindReport.Dao
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

        #region ---------------读取分线提醒记录------------
        /// <summary>
        /// 分线健康率
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public Dictionary<int, int> GetRemind(string startTime, string endTime)
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            string sql = " select HasRemind,count(*) Value from " + tableName + " where HasRemind >=0  ";
            sql += "and CONVERT(varchar(100), cast(tasktime as datetime)  , 23) between '" + AkConfig.StartDate + "' and '" + AkConfig.EndDate + "' ";
            sql += "and CONVERT(varchar(100), cast(tasktime as datetime), 108) between   '" + startTime + "' and '" + endTime + "' ";
            sql += " group by HasRemind";

            DataTable dt = SqlServerHelper.Instance.GetDataTable(AkConfig.ConnAlohaReporting, sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    dict[dr.Field<int>("HasRemind")] = dr.Field<int>("Value");
                }
            }
            return dict;
        }
        #endregion
    }
}
