
using AKRemindReport.Dao;
using AKRemindReport.Models;
using Common.NLog;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AKRemindReport
{
    /// <summary>
    /// 服务区报表
    /// </summary>
    public class AkServerReport : AkReport
    {
        /// <summary>
        /// 报表数据
        /// </summary>
        private List<AkServerReportModel> _ltData = null;

        /// <summary>
        /// 表单名称
        /// </summary>
        private string sheetName = "服务速度报表";

        /// <summary>
        /// 默认起始行
        /// </summary>
        private int startRow = 4;

        /// <summary>
        /// 行位置
        /// </summary>
        private int rowIndex = 1;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AkServerReport()
            : base()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ltData"></param>
        public AkServerReport(List<AkServerReportModel> ltData)
        {
            this._ltData = ltData;
        }

        /// <summary>
        /// 把数据写入至Excel
        /// </summary>
        /// <param name="ltData"></param>
        /// <returns></returns>
        public void Export(List<AkServerReportModel> ltData, List<AkMergedRange> ltMergeds = null, List<int> ltStyle = null)
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
                    //添加数据
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
                //else
                //{
                //    MergedRegion(sheet, ltData);
                //}
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
            List<AkServerReportModel> lt_data = getHeaderData();
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
        private List<AkServerReportModel> getHeaderData()
        {
            List<AkServerReportModel> lt_headers = new List<AkServerReportModel>();

            AkServerReportModel model1 = new AkServerReportModel();
            AkServerReportModel model2 = new AkServerReportModel();
            AkServerReportModel model3 = new AkServerReportModel();


            model1.TimeInterval = AkConfig.StartDate.Replace("-", "") + "—" + AkConfig.EndDate.Replace("-", "") + "  BK-SOS-Report 服务区";
            //model1.HourTime = "BK-SOS-Report";
            //model1.OrderModeName = "BK-SOS-Report";
            //model1.TerminalName = "BK-SOS-Report";
            //model1.SalesTurnover = "BK-SOS-Report";
            //model1.OrderNum = "BK-SOS-Report";
            model1.Item1 = "服务速度报表";
            //model1.AvgProduct = AkDaoHelper.Instance_SystemParam.GetStoreNO();
            //model1.Item2 = "服务区样表";
            //model1.Item3 = "服务区样表";
            //model1.Item4 = "服务区样表";
            //model1.Item5 = "服务区样表";
            //model1.Item6 = "服务区样表";
            //model1.Item7 = "服务区样表";
            lt_headers.Add(model1);

            model2.TimeInterval = "时段";
            model2.HourTime = "小时";
            model2.SalesTurnover = "营业额";
            model2.OrderNum = "订单数量";
            model2.AvgProduct = "产品平均数";
            model2.Item1 = "系统点餐时间";
            model2.Item2 = "收银时间";
            model2.Item3 = "备餐时间";
            model2.Item4 = "汇餐时间";
            model2.Item5 = "整体时间";
            model2.Item6 = "超时订单率";

            model2.Item11 = "柜台";
            model2.Item21 = "自助点餐（含手机点餐&大屏点餐）";
            model2.Item31 = "外送";
            lt_headers.Add(model2);

            model3.Item11 = "柜台订单数量";
            model3.Item12 = "系统点餐时间";
            model3.Item13 = "收银时间";
            model3.Item14 = "备餐时间";
            model3.Item15 = "汇餐时间";
            model3.Item16 = "整体时间";

            model3.Item21 = "自助点餐订单数量";
            model3.Item22 = "备餐时间";
            model3.Item23 = "汇餐时间";

            model3.Item31 = "外送订单数量";
            model3.Item32 = "备餐时间";
            lt_headers.Add(model3);
            return lt_headers;
        }

        private void MergedHeader(ISheet sheet)
        {
            int startRowIndex = 1;
            int endRowIndex = 1;

            //合并第一行
            CellRangeAddress mergedRange = new CellRangeAddress(startRowIndex, endRowIndex, 0, 3);
            sheet.AddMergedRegion(mergedRange);

            mergedRange = new CellRangeAddress(startRowIndex, endRowIndex, 5, 21);
            sheet.AddMergedRegion(mergedRange);

            startRowIndex++;
            endRowIndex = startRowIndex;

            mergedRange = new CellRangeAddress(startRowIndex, endRowIndex, 11, 16);
            sheet.AddMergedRegion(mergedRange);

            mergedRange = new CellRangeAddress(startRowIndex, endRowIndex, 17, 19);
            sheet.AddMergedRegion(mergedRange);

            mergedRange = new CellRangeAddress(startRowIndex, endRowIndex, 20, 21);
            sheet.AddMergedRegion(mergedRange);

            mergedRange = new CellRangeAddress(startRowIndex, startRowIndex + 1, 0, 0);
            sheet.AddMergedRegion(mergedRange);

            mergedRange = new CellRangeAddress(startRowIndex, startRowIndex + 1, 1, 1);
            sheet.AddMergedRegion(mergedRange);

            mergedRange = new CellRangeAddress(startRowIndex, startRowIndex + 1, 2, 2);
            sheet.AddMergedRegion(mergedRange);

            mergedRange = new CellRangeAddress(startRowIndex, startRowIndex + 1, 3, 3);
            sheet.AddMergedRegion(mergedRange);

            mergedRange = new CellRangeAddress(startRowIndex, startRowIndex + 1, 4, 4);
            sheet.AddMergedRegion(mergedRange);

            mergedRange = new CellRangeAddress(startRowIndex, startRowIndex + 1, 5, 5);
            sheet.AddMergedRegion(mergedRange);

            mergedRange = new CellRangeAddress(startRowIndex, startRowIndex + 1, 6, 6);
            sheet.AddMergedRegion(mergedRange);

            mergedRange = new CellRangeAddress(startRowIndex, startRowIndex + 1, 7, 7);
            sheet.AddMergedRegion(mergedRange);

            mergedRange = new CellRangeAddress(startRowIndex, startRowIndex + 1, 8, 8);
            sheet.AddMergedRegion(mergedRange);

            mergedRange = new CellRangeAddress(startRowIndex, startRowIndex + 1, 9, 9);
            sheet.AddMergedRegion(mergedRange);

            mergedRange = new CellRangeAddress(startRowIndex, startRowIndex + 1, 10, 10);
            sheet.AddMergedRegion(mergedRange);

            sheet.GetRow(startRowIndex).Height = 15 * 30;

            SetForegroundColor(sheet);
            sheet.GetRow(startRowIndex + 1).Height = 25 * 30;
        }


        private void SetForegroundColor(ISheet sheet)
        {
            for (int column = 0; column <= 21; column++)
            {
                //ICell cell = sheet.GetRow(3).GetCell(column);
                //cell.CellStyle = this.StyleForegroundThin;

                ICell cel2 = sheet.GetRow(2).GetCell(column);
                cel2.CellStyle = this.StyleForegroundThin;
            }
        }

        #endregion



        /// <summary>
        /// Excel添加行数据
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="model"></param>
        private void AddRow(ISheet sheet, AkServerReportModel model, List<int> ltStyle)
        {
            IRow dataRow = sheet.CreateRow(rowIndex);
            int column = 0;
            dataRow.Height = 18 * 20;

            ICell newCel0 = dataRow.CreateCell(column++);
            ICell newCel1 = dataRow.CreateCell(column++);
            ICell newCel2 = dataRow.CreateCell(column++);
            ICell newCel3 = dataRow.CreateCell(column++);
            ICell newCel4 = dataRow.CreateCell(column++);
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
            ICell newCel21 = dataRow.CreateCell(column++);

            newCel0.SetCellValue(model.TimeInterval);
            newCel1.SetCellValue(model.HourTime);
            newCel2.SetCellValue(model.SalesTurnover);
            newCel3.SetCellValue(model.OrderNum);
            newCel4.SetCellValue(model.AvgProduct);
            newCel5.SetCellValue(model.Item1);
            newCel6.SetCellValue(model.Item2);
            newCel7.SetCellValue(model.Item3);
            newCel8.SetCellValue(model.Item4);
            newCel9.SetCellValue(model.Item5);
            newCel10.SetCellValue(model.Item6);

            newCel11.SetCellValue(model.Item11);
            newCel12.SetCellValue(model.Item12);
            newCel13.SetCellValue(model.Item13);
            newCel14.SetCellValue(model.Item14);
            newCel15.SetCellValue(model.Item15);
            newCel16.SetCellValue(model.Item16);

            newCel17.SetCellValue(model.Item21);
            newCel18.SetCellValue(model.Item22);
            newCel19.SetCellValue(model.Item23);

            newCel20.SetCellValue(model.Item31);
            newCel21.SetCellValue(model.Item32);

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
            sheet.SetColumnWidth(3, 10 * 256);
            sheet.SetColumnWidth(4, 13 * 256);
            sheet.SetColumnWidth(5, 10 * 256);
            sheet.SetColumnWidth(6, 10 * 256);
            sheet.SetColumnWidth(7, 10 * 256);
            sheet.SetColumnWidth(8, 10 * 256);
            sheet.SetColumnWidth(9, 10 * 256);
            sheet.SetColumnWidth(10, 10 * 256);
            sheet.SetColumnWidth(11, 12 * 256);
            sheet.SetColumnWidth(12, 10 * 256);
            sheet.SetColumnWidth(13, 10 * 256);
            sheet.SetColumnWidth(14, 10 * 256);
            sheet.SetColumnWidth(15, 10 * 256);
            sheet.SetColumnWidth(16, 10 * 256);
            sheet.SetColumnWidth(17, 16 * 256);
            sheet.SetColumnWidth(18, 10 * 256);
            sheet.SetColumnWidth(19, 10 * 256);
            sheet.SetColumnWidth(20, 12 * 256);
            sheet.SetColumnWidth(21, 10 * 256);
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
