using Common.NLog;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AKRemindReport.DB
{
    /// <summary>
    /// SqlServer 操作类
    /// </summary>
    public class SqlServerHelper
    {
        private static SqlServerHelper _instance;
        private static readonly object Locker = new object();

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        public static SqlServerHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new SqlServerHelper();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 执行非查询命令
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>有多少语句执行成功</returns>
        public int ExecuteNonQuery(string connstr, string cmdText)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connstr))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = cmdText;
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception er)
            {
                LogHelper.Error(typeof(SqlServerHelper) + ".ExecuteNonQuery Exception error=", er.ToString());
            }
            return -1;
        }

        /// <summary>
        /// 执行语句后，返回第一行第一列的数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>object类型的值</returns>
        public string ExecuteScalar(string connstr, string cmdText)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connstr))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = cmdText;
                    var value = cmd.ExecuteScalar();

                    if (value != null)
                    {
                        return value.ToString();
                    }
                }
            }
            catch (Exception er)
            {
                LogHelper.Error(typeof(SqlServerHelper) + ".ExecuteScalar Exception error=", er.ToString());
            }
            return null;
        }

        /// <summary>
        /// 在数据库中，进行数据库的查询操作
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string connstr, string cmdText)
        {
            var dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connstr))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = cmdText;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            catch (Exception er)
            {
                LogHelper.Error(typeof(SqlServerHelper) + ".GetDataTable Exception error=", er.ToString());
            }
            return dt;
        }

        /// <summary>
        /// 判断库是否存在
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public bool ExistDatabase(string connstr, string DbName)
        {
            bool bl = false;
            string sql = string.Format("SELECT count(1)  FROM sysdatabases WHERE name='{0}'", DbName);
            string strCount = ExecuteScalar(connstr, sql);
            if (!string.IsNullOrEmpty(strCount))
            {
                bl = Convert.ToInt32(strCount) > 0 ? true : false;
            }
            return bl;
        }

        /// <summary>
        /// 判断表是否存在
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public bool ExistTable(string connstr, string tableName)
        {
            bool bl = false;
            string sql = string.Format("SELECT count(1) FROM dbo.sysobjects where name='{0}'", tableName);
            string strCount = ExecuteScalar(connstr, sql);
            if (!string.IsNullOrEmpty(strCount))
            {
                bl = Convert.ToInt32(strCount) > 0 ? true : false;
            }
            return bl;
        }

        /// <summary>
        /// 判断索引是否创建
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public bool ExistIndex(string connstr, string indexName)
        {
            bool bl = false;
            string sql = string.Format("SELECT count(1) FROM sys.indexes where name='{0}'", indexName);
            string strCount = ExecuteScalar(connstr, sql);
            if (!string.IsNullOrEmpty(strCount))
            {
                bl = Convert.ToInt32(strCount) > 0 ? true : false;
            }
            return bl;
        }

        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public bool ExistRecode(string connstr, string cmdText)
        {
            bool bl = false;
            string strCount = ExecuteScalar(connstr, cmdText);
            if (!string.IsNullOrEmpty(strCount))
            {
                bl = Convert.ToInt32(strCount) > 0 ? true : false;
            }
            return bl;
        }
    }
}
