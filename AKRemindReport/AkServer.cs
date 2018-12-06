using AKRemindReport.Dao;
using AKRemindReport.Models;
using Common.NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AKRemindReport.Common;

namespace AKRemindReport
{
    /// <summary>
    /// 服务区报表
    /// </summary>
    public class AkServer : AkBase
    {
        /// <summary>
        /// 行索引
        /// </summary>
        private int rowIndex = 0;

        /// <summary>
        /// 添加数据记录
        /// </summary>
        /// <param name="repastName"></param>
        /// <param name="orderMode"></param>
        /// <param name="terminalType"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="ltResult"></param>
        private void AddRecode(string repastName, string startTime, string endTime, List<AkServerReportModel> ltResult, int type = 0)
        {
            AkServerReportModel model = new AkServerReportModel();

            if (type == 0)
            {
                model.TimeInterval = repastName;
                model.HourTime = startTime + "-" + endTime;
            }
            else if (type == 1)
            {
                model.TimeInterval = repastName + "合计";
                model.HourTime = startTime + "-" + endTime;
            }
            else if (type == 2)
            {
                model.TimeInterval = repastName + "合计";
                model.HourTime = startTime + "-" + endTime;
            }

            model.SalesTurnover = this.DefaultValue;
            model.OrderNum = this.DefaultValue;
            model.AvgProduct = this.DefaultValue;
            model.Item1 = this.DefaultValue;
            model.Item2 = this.DefaultValue;
            model.Item3 = this.DefaultValue;
            model.Item4 = this.DefaultValue;
            model.Item5 = this.DefaultValue;
            model.Item6 = this.DefaultValue;

            model.Item11 = this.DefaultValue;
            model.Item12 = this.DefaultValue;
            model.Item13 = this.DefaultValue;
            model.Item14 = this.DefaultValue;
            model.Item15 = this.DefaultValue;
            model.Item16 = this.DefaultValue;

            model.Item21 = this.DefaultValue;
            model.Item22 = this.DefaultValue;
            model.Item23 = this.DefaultValue;

            model.Item31 = this.DefaultValue;
            model.Item32 = this.DefaultValue;
            model.RowIndex = rowIndex++;
            ltResult.Add(model);
        }

        #region --------服务区数据------------
        #region[数据初始化]
        public List<AkServerReportModel> GetDatas(List<int> lt_repast)
        {
            //导出的餐段时间
            List<AkRepastModel> allRepast = this.GetRepast(lt_repast);

            List<AkServerReportModel> ltResult = new List<AkServerReportModel>();
            string dateNow = DateTime.Now.ToString("yyyy-MM-dd ");
            foreach (var item in allRepast)
            {
                //开始时分
                string startTime = item.StartTime.Substring(0, 5);

                //结束时分
                string endTime = string.Empty;
                int reapeat = this.RepeatNum(item.StartTime, item.EndTime);

                #region [餐段的每半小时]
                for (int i = 0; i < reapeat; i++)
                {
                    string strStartTime = DateTime.Now.ToString("yyyy-MM-dd ") + startTime;
                    endTime = Convert.ToDateTime(strStartTime).AddMinutes(30).ToString("HH:mm");

                    #region [小时合计]
                    List<AkServerReportModel> lt_resultTotal1 = new List<AkServerReportModel>();
                    AddRecode(item.RepastName, startTime, endTime, lt_resultTotal1);
                    setTotalServerData(startTime, endTime, lt_resultTotal1[0]);
                    ltResult.AddRange(lt_resultTotal1);
                    #endregion

                    //本次结束时分数，作为下次开始时分数
                    startTime = endTime;
                }
                #endregion

                #region [餐段合计]
                List<AkServerReportModel> lt_resultTotal2 = new List<AkServerReportModel>();
                AddRecode(item.RepastName, item.StartTime.Substring(0, 5), item.EndTime.Substring(0, 5), lt_resultTotal2, 1);
                setTotalServerData(item.StartTime.Substring(0, 5), item.EndTime.Substring(0, 5), lt_resultTotal2[0]);
                ltResult.AddRange(lt_resultTotal2);
                #endregion
            }

            if (allRepast != null && allRepast.Count == 4)
            {
                AkRepastModel repastStart = allRepast[0];
                AkRepastModel repastEnd = allRepast[allRepast.Count - 1];

                #region [全天合计]
                List<AkServerReportModel> lt_resultTotal3 = new List<AkServerReportModel>();
                AddRecode("全天", repastStart.StartTime.Substring(0, 5), repastEnd.EndTime.Substring(0, 5), lt_resultTotal3, 2);
                setTotalServerData(repastStart.StartTime.Substring(0, 5), repastEnd.EndTime.Substring(0, 5), lt_resultTotal3[0]);
                ltResult.AddRange(lt_resultTotal3);
                #endregion
            }
            return ltResult;
        }

        private void setTotalServerData(string startTime, string endTime, AkServerReportModel model)
        {
            startTime = startTime + ":00";
            endTime = endTime + ":00";

            if (model != null)
            {
                //@1.设置营业额
                int salesTurnover = AkDaoHelper.Instance_Server.GetSalesTurnover(startTime, endTime);
                if (salesTurnover > 0)
                {
                    model.SalesTurnover = salesTurnover.ToString();
                }

                //@2.订单数量
                Dictionary<string, decimal> orderNum = AkDaoHelper.Instance_Server.GetOrderNumber(startTime, endTime);
                if (orderNum != null && orderNum.Count > 0)
                {
                    model.OrderNum = orderNum["all"].ConvertDigits(0);
                    model.Item11 = orderNum["guit"].ConvertDigits(0);
                    model.Item21 = orderNum["auto"].ConvertDigits(0);
                    model.Item31 = orderNum["wais"].ConvertDigits(0);
                }

                //@3.点餐时间
                Dictionary<string, decimal> orderTime = AkDaoHelper.Instance_Server.GetOrderTime(orderNum, startTime, endTime);
                if (orderTime != null && orderTime.Count > 0)
                {
                    model.Item1 = orderTime["all"].ConvertDigits(0);
                    model.Item12 = orderTime["guit"].ConvertDigits(0);
                }

                //@4.收银时间
                Dictionary<string, decimal> cashTime = AkDaoHelper.Instance_Server.GetCashTime(orderNum, startTime, endTime);
                if (cashTime != null && cashTime.Count > 0)
                {
                    model.Item2 = cashTime["all"].ConvertDigits(0);
                    model.Item13 = cashTime["guit"].ConvertDigits(0);
                }

                //@5.备餐时间
                Dictionary<string, decimal> prepareTime = AkDaoHelper.Instance_Server.GetPrepareTime(orderNum, startTime, endTime);
                if (prepareTime != null && prepareTime.Count > 0)
                {
                    model.Item3 = prepareTime["all"].ConvertDigits(0);
                    model.Item14 = prepareTime["guit"].ConvertDigits(0);
                    model.Item22 = prepareTime["auto"].ConvertDigits(0);
                    model.Item32 = prepareTime["wais"].ConvertDigits(0);
                }

                //@6.汇餐时间
                Dictionary<string, decimal> remitTime = AkDaoHelper.Instance_Server.GetRemitTime(orderNum, startTime, endTime);
                if (remitTime != null && remitTime.Count > 0)
                {
                    model.Item4 = remitTime["all"].ConvertDigits(0);
                    model.Item15 = remitTime["guit"].ConvertDigits(0);
                    model.Item23 = remitTime["auto"].ConvertDigits(0);
                }

                //@7.整体时间
                Dictionary<string, decimal> wholeTime = AkDaoHelper.Instance_Server.GetWholeTime(orderNum, startTime, endTime);
                if (wholeTime != null && wholeTime.Count > 0)
                {
                    model.Item5 = wholeTime["all"].ConvertDigits(0);
                    model.Item16 = wholeTime["guit"].ConvertDigits(0);
                }

                //@8.超时订单%
                model.Item6 = AkDaoHelper.Instance_Server.GetTimeOut(orderNum, startTime, endTime);

                //产品平均数
                int avg = AkDaoHelper.Instance_Server.GetAvgProduct(orderNum, startTime, endTime);
                if (orderNum != null && orderNum.ContainsKey("all"))
                {
                    model.AvgProduct = orderNum["all"] > 0 ? (avg / orderNum["all"]).ConvertDigits(1) : string.Empty;
                }
            }
        }

        #endregion
        #endregion

        #region --------服务区合并单元格区域------------
        /// <summary>
        /// 服务区合并单元格区域
        /// </summary>
        /// <param name="lt_repast"></param>
        /// <returns></returns>
        public List<AkMergedRange> GetMergedRanges(List<int> lt_repast, ref List<int> ltStyle)
        {
            rowIndex = 0;
            List<AkRepastModel> allRepast = this.GetRepast(lt_repast);

            List<AkMergedRange> ltMergeds = new List<AkMergedRange>();

            foreach (var item in allRepast)
            {
                int reapeat = RepeatNum(item.StartTime, item.EndTime);

                AkMergedRange merged1 = new AkMergedRange();
                merged1.StartRow = rowIndex;
                merged1.StartColumn = 0;
                rowIndex += reapeat;
                merged1.EndRow = rowIndex - 1;
                merged1.EndColumn = 0;
                ltMergeds.Add(merged1);

                AkMergedRange merged2 = new AkMergedRange();
                merged2.StartRow = rowIndex;
                merged2.StartColumn = 0;
                rowIndex += 1;
                merged2.EndRow = rowIndex - 1;
                merged2.EndColumn = 1;
                ltMergeds.Add(merged2);
                ltStyle.Add(merged2.EndRow);
            }

            if (allRepast != null && allRepast.Count == 4)
            {
                AkMergedRange merged3 = new AkMergedRange();
                merged3.StartRow = rowIndex;
                merged3.StartColumn = 0;
                rowIndex += 1;
                merged3.EndRow = rowIndex - 1;
                merged3.EndColumn = 1;
                ltMergeds.Add(merged3);
                ltStyle.Add(merged3.EndRow);
            }

            return ltMergeds;
        }
        #endregion

    }
}

