﻿using AKRemindServer.DB;
using AKRemindServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRemindServer.Dao
{
    /// <summary>
    /// 系统配置Dao操作
    /// </summary>
    public class AkSystemParamDao
    {

        #region ----------------数据库获取，配置参数------------------
        //SQL中配置项的定义表：[CFCInStoreDB].[dbo].[Item]
        //分线周期：item.number=1（预定义，可能有变） shortname,chitname,longname,bohname四个字段分别表示四个周期可选值；
        //分线频率：item.number=2（预定义，可能有变） shortname,chitname,longname,bohname四个字段分别表示四个周期可选值；
        //单位时间：item.number=3 （预定义，可能有变）chitname的值表示单位时间；
        //分线健康比值：item.number=4 （预定义，可能有变） chitname 的值为小的数，longname的值为大的数。例如：chitname=1,longname=4,则比值为1/4.
        //早餐段：item.number=5 （预定义，可能有变）chitname的值表示开始时间，longname的值表示结束时间；
        //中餐段：item.number=6 （预定义，可能有变）chitname的值表示开始时间，longname的值表示结束时间；
        //下午茶餐段：item.number=7 （预定义，可能有变）chitname的值表示开始时间，longname的值表示结束时间；
        //晚餐段：item.number=8 （预定义，可能有变）chitname的值表示开始时间，longname的值表示结束时间；

        /// <summary>
        /// 获取可选周期
        /// </summary>
        /// <param name="type">item.number = 1:分线周期 2:分线频率</param>
        /// <returns></returns>
        public List<string> GetPeriod(string type)
        {
            List<string> ltResult = new List<string>();
            string sql = "select shortname,chitname,longname,bohname from [CFCInStoreDB].[dbo].[Item] where item.number=" + type;
            var dt = SqlServerHelper.Instance.GetDataTable(AkConfig.ConnCFCInStoreDB, sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (!dt.Rows[0].IsNull("shortname"))
                {
                    ltResult.Add(dt.Rows[0]["shortname"].ToString());
                }

                if (!dt.Rows[0].IsNull("chitname"))
                {
                    ltResult.Add(dt.Rows[0]["chitname"].ToString());
                }

                if (!dt.Rows[0].IsNull("longname"))
                {
                    ltResult.Add(dt.Rows[0]["longname"].ToString());
                }

                if (!dt.Rows[0].IsNull("bohname"))
                {
                    ltResult.Add(dt.Rows[0]["bohname"].ToString());
                }
            }
            return ltResult;
        }

        /// <summary>
        /// 单位时间、任务间隔时间
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetPerTime(string type = "3")
        {
            int time = 0;
            string sql = "select chitname from [CFCInStoreDB].[dbo].[Item] where item.number=" + type;
            var dt = SqlServerHelper.Instance.GetDataTable(AkConfig.ConnCFCInStoreDB, sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                time = int.Parse(dt.Rows[0]["chitname"].ToString());

            }
            return time;
        }

        /// <summary>
        /// 小值，大值比例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public void GetMinOrMaxinValue(out int minValue, out int maxValue, string type = "4")
        {
            minValue = 0;
            maxValue = 0;
            string sql = "select chitname,longname from [CFCInStoreDB].[dbo].[Item] where item.number=" + type;
            var dt = SqlServerHelper.Instance.GetDataTable(AkConfig.ConnCFCInStoreDB, sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                minValue = int.Parse(dt.Rows[0]["chitname"].ToString());
                maxValue = int.Parse(dt.Rows[0]["longname"].ToString());
            }
        }

        /// <summary>
        /// 获取餐段开始时间，结束时间
        /// <param name="type"></param>
        /// <returns></returns>
        public AkRepastModel GetRepastTime(string type)
        {
            AkRepastModel model = null;
            string sql = "select number,shortname, chitname,longname from [CFCInStoreDB].[dbo].[Item] where item.number=" + type;
            var dt = SqlServerHelper.Instance.GetDataTable(AkConfig.ConnCFCInStoreDB, sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                model = new AkRepastModel();
                model.RepastName = dt.Rows[0]["shortname"].ToString();
                model.StartTime = dt.Rows[0]["chitname"].ToString() + ":00";
                model.EndTime = dt.Rows[0]["longname"].ToString() + ":00";
            }
            return model;
        }

        /// <summary>
        /// 获取店号
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetStoreNO()
        {
            string sql = "SELECT top 1 number,name from cfcinstoredb.dbo.Store";
            var dt = SqlServerHelper.Instance.GetDataTable(AkConfig.ConnCFCInStoreDB, sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0]["number"].ToString();
            }
            return string.Empty;
        }
        #endregion


        #region 创建库
        /// <summary>
        /// 创建库
        /// </summary>
        /// <param name="tableName"></param>
        public bool IsConnSuccess(string dbName = "CFCInStoreDB")
        {
            return SqlServerHelper.Instance.ExistDatabase(AkConfig.ConnCFCInStoreDB, dbName);
        }
        #endregion
    }
}
