using AKRemindReport.Models;
using Common.NLog;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AKRemindReport
{
    /// <summary>
    /// 产区报表
    /// </summary>
    public class AkProductReport : AkReport
    {
        /// <summary>
        /// 报表数据
        /// </summary>
        private List<AkProduceReportModel> _ltData = null;

        /// <summary>
        /// 表单名称
        /// </summary>
        private string sheetName = "生产速度报表";

        /// <summary>
        /// 默认起始行
        /// </summary>
        private int startRow = 5;

        /// <summary>
        /// 默认起始列
        /// </summary>
        private int startColumn = 7;

        /// <summary>
        /// 行位置
        /// </summary>
        private int rowIndex = 1;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AkProductReport()
            : base()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ltData"></param>
        public AkProductReport(List<AkProduceReportModel> ltData)
        {
            this._ltData = ltData;
        }

        /// <summary>
        /// 把数据写入至Excel
        /// </summary>
        /// <param name="ltData"></param>
        public void Export(List<AkProduceReportModel> ltData, List<AkMergedRange> ltMergeds = null, List<int> ltStyle = null)
        {
            ISheet sheet = AkReport.WorkBook.CreateSheet();

            AkReport.WorkBook.SetSheetName(AkReport.SheetIndex++, sheetName);

            //添加表头
            SetHeader(sheet);

            //设置列宽
            SetColumnWidth(sheet);

            List<int> ltStyleNew = new List<int>();
            if (ltStyle != null) ltStyle.ForEach(item =>
            {
                ltStyleNew.Add(item + startRow);
            });

            foreach (var item in ltData)
            {
                try
                {
                    AddRow(sheet, item, ltStyleNew);
                }
                catch (Exception er)
                {
                    LogHelper.Error(typeof(AkProductReport) + ".AddRow Exception error=" + rowIndex.ToString(), er.ToString());
                }

            }

            //合并单元格
            try
            {
                if (ltMergeds != null)
                {
                    MergedRegion(sheet, ltMergeds);
                }
            }
            catch (Exception er)
            {
                LogHelper.Error(typeof(AkProductReport) + ".MergedRegion Exception error=" + rowIndex.ToString(), er.ToString());
            }
        }

        #region --------------设置表头
        /// <summary>
        /// 添加表头
        /// </summary>
        /// <param name="sheet"></param>
        private void SetHeader(ISheet sheet)
        {
            List<AkProduceReportModel> lt_data = getHeaderData();
            foreach (var item in lt_data)
            {
                try
                {
                    //添加数据
                    AddRow(sheet, item, null);
                }
                catch (Exception er)
                {
                    LogHelper.Error(typeof(AkProductReport) + ".SetHeader Exception error=" + rowIndex.ToString(), er.ToString());
                }
            }

            //合并表头
            MergedHeader(sheet);
        }

        /// <summary>
        /// 获取表头信息
        /// </summary>
        private List<AkProduceReportModel> getHeaderData()
        {
            List<AkProduceReportModel> lt_headers = new List<AkProduceReportModel>();

            AkProduceReportModel model1 = new AkProduceReportModel();
            AkProduceReportModel model2 = new AkProduceReportModel();
            AkProduceReportModel model3 = new AkProduceReportModel();
            AkProduceReportModel model4 = new AkProduceReportModel();

            model1.TimeInterval = AkConfig.StartDate.Replace("-", "") + "—" + AkConfig.EndDate.Replace("-", "") + " BK-SOS-Report-产区";
            model1.OrderNumTotal = "生产速度报表";
            lt_headers.Add(model1);

            model2.TimeInterval = "时段";
            model2.HourTime = "小时";
            model2.SalesTurnover = "营业额";
            model2.RemindHealth = "分线健康率";
            model2.TimeOut = "超时订单率";

            model2.OrderNumTotal = "Total生产时间";
            model2.ProductNumTotal = "Total生产时间";
            model2.ProductAvgNumTotal = "Total生产时间";
            model2.ProduceTimeTotal = "Total生产时间";

            model2.OrderNum1 = "A屏生产时间";
            model2.ProductNum1 = "A屏生产时间";
            model2.ProductAvgNum1 = "A屏生产时间";
            model2.ProduceTime1 = "A屏生产时间";

            model2.OrderNum2 = "B屏生产时间";
            model2.ProductNum2 = "B屏生产时间";
            model2.ProductAvgNum2 = "B屏生产时间";
            model2.ProduceTime2 = "B屏生产时间";

            model2.OrderNum3 = "C屏生产时间";
            model2.ProductNum3 = "C屏生产时间";
            model2.ProductAvgNum3 = "C屏生产时间";
            model2.ProduceTime3 = "C屏生产时间";

            model2.OrderNum4 = "D屏生产时间";
            model2.ProductNum4 = "D屏生产时间";
            model2.ProductAvgNum4 = "D屏生产时间";
            model2.ProduceTime4 = "D屏生产时间";
            lt_headers.Add(model2);

            model3.TimeInterval = "时段";
            model3.HourTime = "小时";
            model3.SalesTurnover = "营业额";
            model3.RemindHealth = "分线健康率";
            lt_headers.Add(model3);

            model4.TimeInterval = "时段";
            model4.HourTime = "小时";
            model4.SalesTurnover = "营业额";
            model4.RemindHealth = "分线健康率";

            model4.OrderNumTotal = "订单数量";
            model4.ProductNumTotal = "产品数量";
            model4.ProductAvgNumTotal = "产品平均数";
            model4.ProduceTimeTotal = "生产时间";

            model4.OrderNum1 = "订单数量";
            model4.ProductNum1 = "产品数量";
            model4.ProductAvgNum1 = "产品平均数";
            model4.ProduceTime1 = "A屏生产时间";

            model4.OrderNum2 = "订单数量";
            model4.ProductNum2 = "产品数量";
            model4.ProductAvgNum2 = "产品平均数";
            model4.ProduceTime2 = "B屏生产时间";

            model4.OrderNum3 = "订单数量";
            model4.ProductNum3 = "产品数量";
            model4.ProductAvgNum3 = "产品平均数";
            model4.ProduceTime3 = "C屏生产时间";

            model4.OrderNum4 = "订单数量";
            model4.ProductNum4 = "产品数量";
            model4.ProductAvgNum4 = "产品平均数";
            model4.ProduceTime4 = "D屏生产时间";
            lt_headers.Add(model4);
            return lt_headers;
        }

        /// <summary>
        /// 合并表头
        /// </summary>
        /// <param name="sheet"></param>
        private void MergedHeader(ISheet sheet)
        {
            int startRowIndex = this.startRow - 4;
            int endRowIndex = startRowIndex;
            CellRangeAddress mergedRange = new CellRangeAddress(startRowIndex, endRowIndex, 0, 4);
            sheet.AddMergedRegion(mergedRange);

            mergedRange = new CellRangeAddress(startRowIndex, endRowIndex, 5, 28);
            sheet.AddMergedRegion(mergedRange);

            startRowIndex++;

            mergedRange = new CellRangeAddress(startRowIndex, startRowIndex + 2, 0, 0);
            sheet.AddMergedRegion(mergedRange);

            mergedRange = new CellRangeAddress(startRowIndex, startRowIndex + 2, 1, 1);
            sheet.AddMergedRegion(mergedRange);

            mergedRange = new CellRangeAddress(startRowIndex, startRowIndex + 2, 2, 2);
            sheet.AddMergedRegion(mergedRange);

            mergedRange = new CellRangeAddress(startRowIndex, startRowIndex + 2, 3, 3);
            sheet.AddMergedRegion(mergedRange);

            mergedRange = new CellRangeAddress(startRowIndex, startRowIndex + 2, 4, 4);
            sheet.AddMergedRegion(mergedRange);

            mergedRange = new CellRangeAddress(startRowIndex, startRowIndex + 1, 5, 8);
            sheet.AddMergedRegion(mergedRange);

            mergedRange = new CellRangeAddress(startRowIndex, startRowIndex + 1, 9, 12);
            sheet.AddMergedRegion(mergedRange);

            mergedRange = new CellRangeAddress(startRowIndex, startRowIndex + 1, 13, 16);
            sheet.AddMergedRegion(mergedRange);

            mergedRange = new CellRangeAddress(startRowIndex, startRowIndex + 1, 17, 20);
            sheet.AddMergedRegion(mergedRange);

            mergedRange = new CellRangeAddress(startRowIndex, startRowIndex + 1, 21, 24);
            sheet.AddMergedRegion(mergedRange);

            sheet.GetRow(this.startRow - 1).Height = 25 * 30;

            SetForegroundColor(sheet);
        }

        private void SetForegroundColor(ISheet sheet)
        {
            int startRowIndex = this.startRow - 3;
            for (int row = startRowIndex; row < startRowIndex + 2; row++)
            {
                for (int column = 0; column <= 24; column++)
                {
                    ICell cell = sheet.GetRow(startRowIndex).GetCell(column);
                    cell.CellStyle = this.StyleForegroundThin;
                }
            }

            for (int column = 0; column < 5; column++)
            {
                ICell cell = sheet.GetRow(startRowIndex + 2).GetCell(column);
                cell.CellStyle = this.StyleForegroundThin;
            }
        }
        #endregion

        /// <summary>
        /// Excel添加行数据
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="model"></param>
        private void AddRow(ISheet sheet, AkProduceReportModel model, List<int> ltStyle)
        {
            IRow dataRow = sheet.CreateRow(rowIndex);
            dataRow.Height = 18 * 20;
            int column = 0;

            ICell newCel0 = dataRow.CreateCell(column++);
            ICell newCel1 = dataRow.CreateCell(column++);
            ICell newCel2 = dataRow.CreateCell(column++);
            ICell newCel3 = dataRow.CreateCell(column++);
            ICell newCel4 = dataRow.CreateCell(column++);

            ICell newCel21 = dataRow.CreateCell(column++);
            ICell newCel22 = dataRow.CreateCell(column++);
            ICell newCel23 = dataRow.CreateCell(column++);
            ICell newCel24 = dataRow.CreateCell(column++);

            ICell newCel5 = dataRow.CreateCell(column++);
            ICell newCel6 = dataRow.CreateCell(column++);
            ICell newCel7 = dataRow.CreateCell(column++);
            ICell newCel8 = dataRow.CreateCell(column++);

            ICell newCel9 = dataRow.CreateCell(column++);
            ICell newCel10 = dataRow.CreateCell(column++);
            ICell newCel11 = dataRow.CreateCell(column++);
            ICell newCel12 = dataRow.CreateCell(column++);

            ICell newCel13 = dataRow.CreateCell(column++);
            ICell newCel14 = dataRow.CreateCell(column++);
            ICell newCel15 = dataRow.CreateCell(column++);
            ICell newCel16 = dataRow.CreateCell(column++);

            ICell newCel17 = dataRow.CreateCell(column++);
            ICell newCel18 = dataRow.CreateCell(column++);
            ICell newCel19 = dataRow.CreateCell(column++);
            ICell newCel20 = dataRow.CreateCell(column++);

            newCel0.SetCellValue(model.TimeInterval);
            newCel1.SetCellValue(model.HourTime);
            newCel2.SetCellValue(model.SalesTurnover);
            newCel3.SetCellValue(model.RemindHealth);
            newCel4.SetCellValue(model.TimeOut);

            newCel5.SetCellValue(model.OrderNum1);
            newCel6.SetCellValue(model.ProductNum1);
            newCel7.SetCellValue(model.ProductAvgNum1);
            newCel8.SetCellValue(model.ProduceTime1);

            newCel9.SetCellValue(model.OrderNum2);
            newCel10.SetCellValue(model.ProductNum2);
            newCel11.SetCellValue(model.ProductAvgNum2);
            newCel12.SetCellValue(model.ProduceTime2);

            newCel13.SetCellValue(model.OrderNum3);
            newCel14.SetCellValue(model.ProductNum3);
            newCel15.SetCellValue(model.ProductAvgNum3);
            newCel16.SetCellValue(model.ProduceTime3);

            newCel17.SetCellValue(model.OrderNum4);
            newCel18.SetCellValue(model.ProductNum4);
            newCel19.SetCellValue(model.ProductAvgNum4);
            newCel20.SetCellValue(model.ProduceTime4);

            newCel21.SetCellValue(model.OrderNumTotal);
            newCel22.SetCellValue(model.ProductNumTotal);
            newCel23.SetCellValue(model.ProductAvgNumTotal);
            newCel24.SetCellValue(model.ProduceTimeTotal);

            if (ltStyle != null && ltStyle.Contains(rowIndex))
            {
                newCel0.CellStyle = this.StyleForegroundThin;
                newCel1.CellStyle = this.StyleForegroundThin;
                newCel2.CellStyle = this.StyleForegroundThin;
                newCel3.CellStyle = this.StyleForegroundThin;
                newCel4.CellStyle = this.StyleForegroundThin;
                newCel5.CellStyle = this.StyleForegroundThin;
                newCel6.CellStyle = this.StyleForegroundThin;
                newCel7.CellStyle = this.StyleForegroundThin;
                newCel8.CellStyle = this.StyleForegroundThin;
                newCel9.CellStyle = this.StyleForegroundThin;

                newCel10.CellStyle = this.StyleForegroundThin;
                newCel11.CellStyle = this.StyleForegroundThin;
                newCel12.CellStyle = this.StyleForegroundThin;
                newCel13.CellStyle = this.StyleForegroundThin;
                newCel14.CellStyle = this.StyleForegroundThin;
                newCel15.CellStyle = this.StyleForegroundThin;
                newCel16.CellStyle = this.StyleForegroundThin;
                newCel17.CellStyle = this.StyleForegroundThin;
                newCel18.CellStyle = this.StyleForegroundThin;
                newCel19.CellStyle = this.StyleForegroundThin;
                newCel20.CellStyle = this.StyleForegroundThin;
                newCel21.CellStyle = this.StyleForegroundThin;
                newCel22.CellStyle = this.StyleForegroundThin;
                newCel23.CellStyle = this.StyleForegroundThin;
                newCel24.CellStyle = this.StyleForegroundThin;
            }
            else
            {
                newCel0.CellStyle = this.StyleThin;
                newCel1.CellStyle = this.StyleThin;
                newCel2.CellStyle = this.StyleThin;
                newCel3.CellStyle = this.StyleThin;
                newCel4.CellStyle = this.StyleThin;
                newCel5.CellStyle = this.StyleThin;
                newCel6.CellStyle = this.StyleThin;
                newCel7.CellStyle = this.StyleThin;
                newCel8.CellStyle = this.StyleThin;
                newCel9.CellStyle = this.StyleThin;

                newCel10.CellStyle = this.StyleThin;
                newCel11.CellStyle = this.StyleThin;
                newCel12.CellStyle = this.StyleThin;
                newCel13.CellStyle = this.StyleThin;
                newCel14.CellStyle = this.StyleThin;
                newCel15.CellStyle = this.StyleThin;
                newCel16.CellStyle = this.StyleThin;
                newCel17.CellStyle = this.StyleThin;
                newCel18.CellStyle = this.StyleThin;
                newCel19.CellStyle = this.StyleThin;
                newCel20.CellStyle = this.StyleThin;
                newCel21.CellStyle = this.StyleThin;
                newCel22.CellStyle = this.StyleThin;
                newCel23.CellStyle = this.StyleThin;
                newCel24.CellStyle = this.StyleThin;
            }
            rowIndex++;
        }

        /// <summary>
        /// 设置列宽
        /// </summary>
        /// <param name="sheet"></param>
        private void SetColumnWidth(ISheet sheet)
        {
            sheet.SetColumnWidth(1, 15 * 256);
            sheet.SetColumnWidth(2, 10 * 256);
            sheet.SetColumnWidth(3, 12 * 256);
            sheet.SetColumnWidth(4, 14 * 256);
            sheet.SetColumnWidth(5, 10 * 256);
            sheet.SetColumnWidth(6, 10 * 256);
            sheet.SetColumnWidth(7, 10 * 256);
            sheet.SetColumnWidth(8, 10 * 256);
            sheet.SetColumnWidth(9, 10 * 256);
            sheet.SetColumnWidth(10, 10 * 256);
            sheet.SetColumnWidth(11, 10 * 256);
            sheet.SetColumnWidth(12, 12 * 256);
            sheet.SetColumnWidth(13, 10 * 256);
            sheet.SetColumnWidth(14, 10 * 256);
            sheet.SetColumnWidth(15, 10 * 256);
            sheet.SetColumnWidth(16, 12 * 256);
            sheet.SetColumnWidth(17, 10 * 256);
            sheet.SetColumnWidth(18, 10 * 256);
            sheet.SetColumnWidth(19, 10 * 256);
            sheet.SetColumnWidth(20, 12 * 256);
            sheet.SetColumnWidth(21, 10 * 256);
            sheet.SetColumnWidth(22, 10 * 256);
            sheet.SetColumnWidth(23, 10 * 256);
            sheet.SetColumnWidth(24, 12 * 256);
        }

        private void MergedRegion(ISheet sheet, List<AkMergedRange> ltMergeds)
        {
            CellRangeAddress mergedRange;
            foreach (var item in ltMergeds)
            {
                mergedRange = new CellRangeAddress(
                        item.StartRow + this.startRow,
                     item.EndRow + this.startRow,
                     item.StartColumn,
                     item.EndColumn);
                sheet.AddMergedRegion(mergedRange);
            }
        }
    }
}
