using AKRemindServer.Dao;
using AKRemindServer.DB;
using AKRemindServer.Models;
using Common.NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using AKRemindServer.Common;

namespace AKRemindServer
{
    /// <summary>
    /// 提醒操作
    /// </summary>
    public class AkRemind
    {
        /// <summary>
        /// 初始化
        /// </summary>
        static AkRemind()
        {
            //建表
            AkDaoHelper.Instance_Remind.CreateTable();

            //建索引
            AkDaoHelper.Instance_Remind.CreateIndex();
        }

        /// <summary>
        /// 测试
        /// </summary>
        public void Test()
        {
            AkThresholdModel model = new AkThresholdModel()
            {
                DataDate = "20180311",
                CreateTime = "2018-03-12 15:03:04 111",
                UpdateTime = "2018-03-12 15:03:04 111",
                ThresholdValue = 10,
                PeriodNum = 14,
                PeriodStartDate = "20180226",
                PeriodEndDate = "20180311"
            };

            //AkThreshold akThreshold = new AkThreshold();
            //var ak1 = AkThreshold.GetThreshold();
            //var ak2 = AkThreshold.Save(model);


            AkRemindModel model1 = new AkRemindModel()
            {
                Tasktime = "2018-03-12 15:03:04 111",

                PeriodNum = 14,
                IsRemind = 1,
                HasRemind = -1
            };

            AkRemind akRemind = new AkRemind();
            //var akRemind1 = akRemind.Insert(model1);
            //var akRemind2 = akRemind.ResetHasRemind(true, 1);
        }

        #region ---------------计算是否提醒分线------------

        /// <summary>
        /// 分线提醒
        /// </summary>
        /// <returns></returns>
        public bool Remind()
        {
            bool remind = false;
            bool isHealth = false;
            try
            {
                //@1.计算是否提醒分线
                remind = CalculateRemind(out isHealth);

                //@2.回填是否,已分线
                AkDaoHelper.Instance_Remind.ResetHasRemind(isHealth);

                AkThresholdModel thresholdModel = AkDaoHelper.Instance_Threshold.GetNewThreshold();

                //@3.新增提醒记录
                AkRemindModel model = new AkRemindModel()
                {
                    Tasktime = DateTime.Now.ToLongTime(),
                    BreakFastValue = thresholdModel.BreakFastValue,
                    LunchValue = thresholdModel.LunchValue,
                    AfternoonTeaValue = thresholdModel.AfternoonTeaValue,
                    SupperValue = thresholdModel.SupperValue,
                    PeriodNum = AkConfig.PeriodNum,
                    IsRemind = remind ? 1 : 0,
                    HasRemind = -1
                };
                bool success = AkDaoHelper.Instance_Remind.Save(model);
            }
            catch (Exception er)
            {
                LogHelper.Error(typeof(AkRemind) + ".Remind Exception error=", er.ToString());
            }

            //Random random = new Random();
            //int r = random.Next(100);
            //if (r % 3 == 0)
            //{
            //    remind = true;
            //}
            //else
            //{
            //    remind = false;
            //}
            return remind;
        }


        /// <summary>
        /// 计算是否提醒分线
        /// </summary>
        /// <param name="lineCount">启动线数</param>
        /// <returns></returns>
        private bool CalculateRemind(out bool isHealth)
        {
            isHealth = true;

            bool bl = false;
            int min = 0;//较小值
            int max = 0;//较大值

            int threshold = getCurThreshold();
            if (threshold == 0)
            {
                LogHelper.Info("当前时间不属于【餐段时间】，不存在阀值 ", string.Empty);
                return bl;
            }

            int quantity = 0;
            //@2.获取标准逻辑值，用于与阀值对比
            string dateTiime = DateTime.Now.AddSeconds(-AkConfig.SysParam.TaskTime).ToLongTime();
            LogHelper.Info("timestarted=", dateTiime);

            var squantitys = AkDaoHelper.Instance_Threshold.GetStationQuantity(dateTiime);
            if (squantitys == null || squantitys.Count == 0)
            {
                LogHelper.Info("上次任务结束至当前任务时间内：不存在【标准值】 ", string.Empty);
                return bl;
            }

            //所有线路总和
            quantity = squantitys.Sum(l => l.Quantity);

            //大于阀值，则提醒分线，否则不提醒
            if (quantity >= threshold)
            {
                bl = true;
            }

            if (bl)
            {
                if (squantitys.Count == 1)
                {
                    isHealth = false;
                }
                else if (squantitys.Count == 2) //双线
                {
                    min = squantitys.Min(l => l.Quantity); //较小值
                    max = squantitys.Max(l => l.Quantity); //较大值

                    if (bl && ((min / (double)max) < AkConfig.SysParam.MinLine / (double)AkConfig.SysParam.MaxLine))
                    {
                        //非健康
                        isHealth = false;
                    }
                }
            }

            return bl;
        }

        /// <summary>
        /// 获取当前餐段阀值
        /// </summary>
        /// <returns></returns>
        private int getCurThreshold()
        {
            int threshold = 0;
            AkThresholdModel model = AkDaoHelper.Instance_Threshold.GetNewThreshold();
            if (model != null)
            {
                string curTime = DateTime.Now.ToString("HH:mm:ss");
                AkSystemParamModel reastTime = AkConfig.SysParam;
                if (reastTime != null)
                {
                    if (curTime.CompareTo(reastTime.BreakFast.StartTime) > 0 && curTime.CompareTo(reastTime.BreakFast.EndTime) < 0)
                    {
                        threshold = model.BreakFastValue;
                    }
                    else if (curTime.CompareTo(reastTime.Lunch.StartTime) > 0 && curTime.CompareTo(reastTime.Lunch.EndTime) < 0)
                    {
                        threshold = model.LunchValue;
                    }
                    else if (curTime.CompareTo(reastTime.AfternoonTea.StartTime) > 0 && curTime.CompareTo(reastTime.AfternoonTea.EndTime) < 0)
                    {
                        threshold = model.AfternoonTeaValue;
                    }
                    else if (curTime.CompareTo(reastTime.Supper.StartTime) > 0 && curTime.CompareTo(reastTime.Supper.EndTime) < 0)
                    {
                        threshold = model.SupperValue;
                    }
                }
            }

            return threshold;
        }
        #endregion
    }
}
