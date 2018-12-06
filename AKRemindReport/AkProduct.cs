using AKRemindReport.Dao;
using AKRemindReport.Models;
using Common.NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AKRemindReport.Common;

namespace AKRemindReport
{
    /// <summary>
    /// 产区
    /// </summary>
    public class AkProduct : AkBase
    {
        /// <summary>
        /// 行索引
        /// </summary>
        private int rowIndex = 0;

        public AkProduct()
            : base()
        {
        }

        /// <summary>
        /// 添加数据记录
        /// </summary>
        /// <param name="repastName"></param>
        /// <param name="source"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="ltResult"></param>
        private void AddRecode(string repastName, string startTime, string endTime, List<AkProduceReportModel> lt_result, int type = 0)
        {
            AkProduceReportModel model = new AkProduceReportModel();

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
            model.RemindHealth = this.DefaultValue;

            //1线
            model.OrderNum1 = this.DefaultValue;
            model.ProductNum1 = this.DefaultValue;
            model.ProductAvgNum1 = this.DefaultValue;
            model.ProduceTime1 = this.DefaultValue;

            //2线
            model.OrderNum2 = this.DefaultValue;
            model.ProductNum2 = this.DefaultValue;
            model.ProductAvgNum2 = this.DefaultValue;
            model.ProduceTime2 = this.DefaultValue;

            //3线
            model.OrderNum3 = this.DefaultValue;
            model.ProductNum3 = this.DefaultValue;
            model.ProductAvgNum3 = this.DefaultValue;
            model.ProduceTime3 = this.DefaultValue;

            //4线
            model.OrderNum4 = this.DefaultValue;
            model.ProductNum4 = this.DefaultValue;
            model.ProductAvgNum4 = this.DefaultValue;
            model.ProduceTime4 = this.DefaultValue;

            //5线
            model.OrderNum5 = this.DefaultValue;
            model.ProductNum5 = this.DefaultValue;
            model.ProductAvgNum5 = this.DefaultValue;
            model.ProduceTime5 = this.DefaultValue;

            //合计
            model.OrderNumTotal = this.DefaultValue;
            model.ProductNumTotal = this.DefaultValue;
            model.ProductAvgNumTotal = this.DefaultValue;
            model.ProduceTimeTotal = this.DefaultValue;
            model.RowIndex = rowIndex++;

            lt_result.Add(model);
        }

        #region --------厂区数据------------
        #region[数据初始化]
        /// <summary>
        /// 获取厂区数据
        /// </summary>
        /// <param name="lt_repast"></param>
        /// <param name="ltMergeds"></param>
        /// <returns></returns>
        public List<AkProduceReportModel> GetDatas(List<int> lt_repast)
        {
            //导出的餐段时间
            List<AkRepastModel> allRepast = this.GetRepast(lt_repast);

            List<AkProduceReportModel> ltResult = new List<AkProduceReportModel>();

            //@1.餐段
            foreach (var item in allRepast)
            {
                //开始时分
                string startTime = item.StartTime.Substring(0, 5);

                //结束时分
                string endTime = string.Empty;

                int reapeat = RepeatNum(item.StartTime, item.EndTime);

                #region[餐段时间内，每半小时一段]
                //@2.一个餐段内的，总共半小时段
                for (int i = 0; i < reapeat; i++)
                {
                    string strStartTime = DateTime.Now.ToString("yyyy-MM-dd ") + startTime;
                    endTime = Convert.ToDateTime(strStartTime).AddMinutes(30).ToString("HH:mm");

                    //该阶段进行数据添加
                    LogHelper.Info(typeof(AkProduct) + "-> " + item.RepastName + "  ", startTime + "-" + endTime);

                    #region [半小时合计]
                    List<AkProduceReportModel> lt_resultTotal1 = new List<AkProduceReportModel>();
                    AddRecode(item.RepastName, startTime, endTime, lt_resultTotal1);

                    //设置数据值
                    setTotalProductData(startTime, endTime, lt_resultTotal1[0]);

                    //添加餐段数据
                    ltResult.AddRange(lt_resultTotal1);
                    #endregion

                    //本次结束时分数，作为下次开始时分数
                    startTime = endTime;
                }
                #endregion

                #region[餐段合计]
                List<AkProduceReportModel> lt_resultTotal2 = new List<AkProduceReportModel>();
                AddRecode(item.RepastName, item.StartTime.Substring(0, 5), item.EndTime.Substring(0, 5), lt_resultTotal2, 1);

                //设置数据值
                setTotalProductData(item.StartTime.Substring(0, 5), item.EndTime.Substring(0, 5), lt_resultTotal2[0]);

                //添加餐段数据
                ltResult.AddRange(lt_resultTotal2);
                #endregion
            }

            if (allRepast != null && allRepast.Count == 4)
            {
                AkRepastModel repastStart = allRepast[0];
                AkRepastModel repastEnd = allRepast[allRepast.Count - 1];

                #region[Total：合计]
                List<AkProduceReportModel> lt_resultTotal3 = new List<AkProduceReportModel>();
                AddRecode("全天", repastStart.StartTime.Substring(0, 5), repastEnd.EndTime.Substring(0, 5), lt_resultTotal3, 2);
                setTotalProductData(repastStart.StartTime.Substring(0, 5), repastEnd.EndTime.Substring(0, 5), lt_resultTotal3[0]);
                ltResult.AddRange(lt_resultTotal3);
                #endregion
            }

            return ltResult;
        }

        private void setTotalProductData(string startTime, string endTime, AkProduceReportModel model)
        {
            startTime = startTime + ":00";
            endTime = endTime + ":00";

            if (model != null)
            {
                //@1.设置营业额
                decimal salesTurnover = AkDaoHelper.Instance_Product.GetSalesTurnover(startTime, endTime);
                model.SalesTurnover = salesTurnover.ConvertDigits(0);

                //@2.分线健康率
                model.RemindHealth = AkDaoHelper.Instance_Product.GetHealthNumber(startTime, endTime);

                //@3.订单数量
                Dictionary<string, decimal> dict_order = AkDaoHelper.Instance_Product.GetOrderNumber(startTime, endTime);
                if (dict_order != null && dict_order.Count > 0)
                {
                    model.OrderNumTotal = dict_order["All"].ConvertDigits(0);
                    model.OrderNum1 = dict_order["StationA"].ConvertDigits(0);
                    model.OrderNum2 = dict_order["StationB"].ConvertDigits(0);
                    model.OrderNum3 = dict_order["StationC"].ConvertDigits(0);
                    model.OrderNum4 = dict_order["StationD"].ConvertDigits(0);

                    //@4.超时订单100%
                    model.TimeOut = AkDaoHelper.Instance_Product.GetTimeOut(dict_order["All"], startTime, endTime);
                }

                //@5.产品数量
                Dictionary<string, decimal> dict_product = AkDaoHelper.Instance_Product.GetProductNumber(startTime, endTime);
                if (dict_order != null && dict_order.Count > 0)
                {
                    model.ProductNumTotal = dict_product["All"].ConvertDigits(0);
                    model.ProductNum1 = dict_product["StationA"].ConvertDigits(0);
                    model.ProductNum2 = dict_product["StationB"].ConvertDigits(0);
                    model.ProductNum3 = dict_product["StationC"].ConvertDigits(0);
                    model.ProductNum4 = dict_product["StationD"].ConvertDigits(0);
                }

                //@6.产品平均数

                if (dict_order != null && dict_order.Count > 0 && dict_product != null && dict_product.Count > 0)
                {
                    model.ProductAvgNumTotal = dict_order["All"] > 0 ? (dict_product["All"] / dict_order["All"]).ConvertDigits(1) : string.Empty;
                    model.ProductAvgNum1 = dict_order["StationA"] > 0 ? (dict_product["StationA"] / dict_order["StationA"]).ConvertDigits(1) : string.Empty;
                    model.ProductAvgNum2 = dict_order["StationB"] > 0 ? (dict_product["StationB"] / dict_order["StationB"]).ConvertDigits(1) : string.Empty;
                    model.ProductAvgNum3 = dict_order["StationC"] > 0 ? (dict_product["StationC"] / dict_order["StationC"]).ConvertDigits(1) : string.Empty;
                    model.ProductAvgNum4 = dict_order["StationD"] > 0 ? (dict_product["StationD"] / dict_order["StationD"]).ConvertDigits(1) : string.Empty;
                }

                //@7.生产时间
                Dictionary<string, decimal> dict_produce_time = AkDaoHelper.Instance_Product.GetProduceTime(dict_order, startTime, endTime);
                if (dict_produce_time != null && dict_produce_time.Count > 0)
                {
                    model.ProduceTimeTotal = dict_produce_time["All"].ConvertDigits(0);
                    model.ProduceTime1 = dict_produce_time["StationA"].ConvertDigits(0);
                    model.ProduceTime2 = dict_produce_time["StationB"].ConvertDigits(0);
                    model.ProduceTime3 = dict_produce_time["StationC"].ConvertDigits(0);
                    model.ProduceTime4 = dict_produce_time["StationD"].ConvertDigits(0);
                }
            }
        }
        #endregion

        #endregion

        #region --------厂区合并单元格区域------------
        /// <summary>
        /// 厂区合并单元格区域
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