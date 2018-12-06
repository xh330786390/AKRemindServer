using AKRemindServer.Dao;
using AKRemindServer.DB;
using AKRemindServer.Models;
using Common.NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKRemindServer
{
    /// <summary>
    /// Ak阀值
    /// 1.计算最新阀值：每天已设置的周期只计算一次，存储在sqlite中
    ///   周期重设置后，则阀值须重新计算
    /// 2.读取阀值：从Sqlite中取最新阀值
    /// </summary>
    public class AkThreshold
    {
        /// <summary>
        /// 初始化
        /// </summary>
        static AkThreshold()
        {
            //建库
            //remindDao.CreateDataBase(dbName);

            //建表
            AkDaoHelper.Instance_Threshold.CreateTable();

            //建索引
            AkDaoHelper.Instance_Threshold.CreateIndex();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AkThreshold()
        {

        }

        /// <summary>
        /// 开辟线程：计算阀值
        /// 计算阀值情况：
        ///         1).前一天dbf文件已存在
        ///         2).Sqlite中，无记录
        ///         3).Sqlite有记录，但是最新记录的数据日期 小于 前一天的日期
        /// </summary>
        public void Excute(string dateTime)
        {
            DateTime dt = DateTime.Parse(dateTime);
            if (dt.Hour < 5) return;

            AkThresholdModel akThreshold = null;
            Task.Factory.StartNew(() =>
            {
                akThreshold = AkDaoHelper.Instance_Threshold.GetNewThreshold();

                DateTime newDate = DateTime.Now;
                if (akThreshold != null)
                {
                    newDate = DateTime.Parse(akThreshold.DataDate + " 00:00:00").AddDays(AkConfig.UpdateNum);
                }

                if (akThreshold == null || newDate <= DateTime.Now.AddDays(-1))
                {
                    SaveThreshold(dateTime);
                }
            });

            akThreshold = AkDaoHelper.Instance_Threshold.GetNewThreshold();
            if (akThreshold.BreakFastValue == 0 ||
                akThreshold.BreakFastValue == 0 ||
                akThreshold.AfternoonTeaValue == 0 ||
                akThreshold.SupperValue == 0)
            {
                SaveThreshold(dateTime);
            }
        }


        public void SaveThreshold(string dateTime)
        {
            //计算阀值
            int breakFastValue = CalculateThresholdValue(AkConfig.SysParam.BreakFast);
            int lunchValue = CalculateThresholdValue(AkConfig.SysParam.Lunch);
            int afternoonTeaValue = CalculateThresholdValue(AkConfig.SysParam.AfternoonTea);
            int supperValue = CalculateThresholdValue(AkConfig.SysParam.Supper);

            AkThresholdModel newModel = new AkThresholdModel()
            {
                DataDate = AkConfig.EndDate,
                CreateTime = dateTime,
                UpdateTime = dateTime,

                BreakFastValue = breakFastValue,
                LunchValue = lunchValue,
                AfternoonTeaValue = afternoonTeaValue,
                SupperValue = supperValue,

                PeriodNum = AkConfig.PeriodNum,
                PeriodStartDate = AkConfig.StartDate,
                PeriodEndDate = AkConfig.EndDate
            };

            AkDaoHelper.Instance_Threshold.Save(newModel);
        }


        #region ---------------计算阀值------------

        /// <summary>
        /// 计算周期内的阀值
        /// </summary>
        /// <returns></returns>
        public int CalculateThresholdValue(AkRepastModel model)
        {
            double dbValue = 0;
            int doorValue = 0;
            try
            {
                dbValue = GetItemStandCooktime(model);

                if (dbValue > 0)
                {
                    //阀值：四色五入
                    doorValue = (int)Math.Round(AkConfig.SysParam.PerTime / dbValue, MidpointRounding.AwayFromZero);
                }
            }
            catch (Exception er)
            {
                LogHelper.Error(typeof(AkThreshold) + ".CalculateThresholdValue Exception error=", er.ToString());
            }
            return doorValue;
        }

        /// <summary>
        /// 求出单个Item的标准制作时间
        /// </summary>
        /// <returns></returns>
        private double GetItemStandCooktime(AkRepastModel model)
        {
            //@1.获取周期内，字段“Item”所有类型，各自的Qulity总数
            List<ItemQuantity> itemQuantitys = AkDaoHelper.Instance_Threshold.GetItemsQulity(model);

            //@2.求出item的数量总数
            double totalQuantitys = AkDaoHelper.Instance_Threshold.GetTotalQulitys(itemQuantitys);

            //@3.获取每个Item的Cooktime
            List<ItemCookTime> temCookTimes = AkDaoHelper.Instance_Threshold.GetItemsCookTime();

            //@4.求出单个Item的标准制作时间
            var query = from quantitys in itemQuantitys
                        join cookTimes in temCookTimes
                        on quantitys.Item equals cookTimes.Item
                        select new ItemStandardCookTime
                        {
                            Item = quantitys.Item,
                            Quantity = quantitys.Quantity,
                            CookTime = cookTimes.CookTime,
                            ItemPercent = quantitys.Quantity / totalQuantitys,
                            ItemStandCooktime = quantitys.Quantity / totalQuantitys *
                            cookTimes.CookTime
                        };

            if (query != null && query.Count() > 0)
            {
                var lt = query.ToList();
                return query.Sum(s => s.ItemStandCooktime);
            }
            return 0;
        }
        #endregion
    }
}
