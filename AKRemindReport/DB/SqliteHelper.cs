//using Common.NLog;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data;
//using System.Data.SQLite;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AKRemindReport.DB
//{
//    /// <summary>
//    /// Sqlite操作
//    /// </summary>
//    public sealed class SQLiteHelper
//    {
//        /// <summary>
//        /// 文件基本目录
//        /// </summary>
//        private static string basePath = @"E:\Ak";//Environment.CurrentDirectory;
//        /// <summary>
//        /// 文件名
//        /// </summary>
//        private static string dbName = "aloha.sqlite3";
//        /// <summary>
//        /// 数据源
//        /// </summary>
//        private static string dataSource = string.Empty;

//        private static SQLiteHelper _instance;
//        private static readonly object Locker = new object();

//        /// <summary>
//        /// 静态实例，初始化信息
//        /// </summary>
//        static SQLiteHelper()
//        {
//            if (!Directory.Exists(basePath))
//            {
//                Directory.CreateDirectory(basePath);
//            }

//            string filePath = Path.Combine(basePath, dbName);

//            if (!File.Exists(filePath))
//            {
//                SQLiteConnection.CreateFile(filePath);
//            }

//            dataSource = "data source=" + filePath;
//        }

//        /// <summary>
//        /// 获取实例
//        /// </summary>
//        /// <returns></returns>
//        public static SQLiteHelper Instance
//        {
//            get
//            {
//                if (_instance == null)
//                {
//                    lock (Locker)
//                    {
//                        if (_instance == null)
//                        {
//                            _instance = new SQLiteHelper();
//                        }
//                    }
//                }
//                return _instance;
//            }
//        }

//        /// <summary>
//        /// 执行非查询命令
//        /// </summary>
//        /// <param name="cmdText"> 需要执行的命令文本 </param>
//        /// <returns> 返回更新的行数 </returns>
//        public int ExecuteNonQuery(string cmdText)
//        {
//            try
//            {
//                using (var conn = new SQLiteConnection(dataSource))
//                using (var cmd = conn.CreateCommand())
//                {
//                    conn.Open();
//                    cmd.CommandText = cmdText;
//                    var rowsUpdated = cmd.ExecuteNonQuery();
//                    return rowsUpdated;
//                }
//            }
//            catch (Exception er)
//            {
//                LogHelper.Error(typeof(SQLiteHelper) + ".ExecuteNonQuery Exception error=", er.ToString());
//            }

//            return -1;
//        }

//        /// <summary>
//        /// 执行检索单项命令
//        /// </summary>
//        /// <param name="cmdText"> 需要执行的命令文本 </param>
//        /// <returns> 一个字符串 </returns>
//        public string ExecuteScalar(string cmdText)
//        {
//            try
//            {
//                using (var conn = new SQLiteConnection(dataSource))
//                using (var cmd = conn.CreateCommand())
//                {
//                    conn.Open();
//                    cmd.CommandText = cmdText;
//                    var value = cmd.ExecuteScalar();

//                    if (value != null)
//                    {
//                        return value.ToString();
//                    }
//                }
//            }
//            catch (Exception er)
//            {
//                LogHelper.Error(typeof(SQLiteHelper) + ".ExecuteScalar Exception error=", er.ToString());
//            }
//            return null;
//        }

//        /// <summary>
//        /// 判断表是否存在
//        /// </summary>
//        /// <param name="cmdText"></param>
//        /// <returns></returns>
//        public bool ExistTable(string tableName)
//        {
//            bool bl = false;
//            string sql = string.Format("SELECT count(1) FROM sqlite_master where type='table' and name='{0}'", tableName);
//            string strCount = ExecuteScalar(sql);
//            if (!string.IsNullOrEmpty(strCount))
//            {
//                bl = Convert.ToInt32(strCount) > 0 ? true : false;
//            }
//            return bl;
//        }

//        /// <summary>
//        /// 判断索引是否创建
//        /// </summary>
//        /// <param name="cmdText"></param>
//        /// <returns></returns>
//        public bool ExistIndex(string indexName)
//        {
//            bool bl = false;
//            string sql = string.Format("SELECT count(1) FROM sqlite_master where type='index' and name='{0}'", indexName);
//            string strCount = ExecuteScalar(sql);
//            if (!string.IsNullOrEmpty(strCount))
//            {
//                bl = Convert.ToInt32(strCount) > 0 ? true : false;
//            }
//            return bl;
//        }



//        /// <summary>
//        /// 判断记录是否存在
//        /// </summary>
//        /// <param name="cmdText"></param>
//        /// <returns></returns>
//        public bool ExistRecode(string cmdText)
//        {
//            bool bl = false;
//            string strCount = ExecuteScalar(cmdText);
//            if (!string.IsNullOrEmpty(strCount))
//            {
//                bl = Convert.ToInt32(strCount) > 0 ? true : false;
//            }
//            return bl;
//        }

//        /// <summary>
//        /// 获取数据表
//        /// </summary>
//        /// <param name="cmdText"> 需要执行的命令文本 </param>
//        /// <returns> 一个数据表集合 </returns>
//        public DataTable GetDataTable(string cmdText)
//        {
//            var dt = new DataTable();

//            try
//            {
//                using (var conn = new SQLiteConnection(dataSource))
//                using (var cmd = conn.CreateCommand())
//                {
//                    conn.Open();
//                    cmd.CommandText = cmdText;
//                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
//                    {
//                        dt.Load(reader);
//                    }
//                }
//            }
//            catch (Exception er) { LogHelper.Error(typeof(SQLiteHelper) + ".GetDataTable Exception error=", er.ToString()); }
//            return dt;
//        }
//    }
//}
