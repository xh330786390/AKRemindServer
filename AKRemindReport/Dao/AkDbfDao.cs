using AKRemindReport.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AKRemindReport.Dao
{
    public class AkDbfDao
    {
        private static DataTable dbfTable;
        public static DataTable DbfTable
        {
            get { return dbfTable; }
            set { dbfTable = value; }
        }

        public static void Clear()
        {
            DbfTable = null;
            
        }

        public DataTable GetDbfFile()
        {
            DataTable dtResult = NewDataTable();
            DateTime endDate = Convert.ToDateTime(AkConfig.EndDate);
            DateTime startDate = Convert.ToDateTime(AkConfig.StartDate);
            int days = (endDate - startDate).Days;
            for (int day = 0; day <= days; day++)
            {
                DataTable curTable = null;
                string curDate = endDate.AddDays(-day).ToString("yyyyMMdd");
                if (curDate == DateTime.Now.ToString("yyyyMMdd"))
                {
                    curTable = DbfHelper.OpenDbfFile();
                }
                else
                {
                    curTable = DbfHelper.OpenDbfFile(curDate);
                }

                if (curTable != null)
                {
                    UnitTable(curTable, dtResult);
                }
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
    }
}
