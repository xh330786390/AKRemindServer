using AKRemindReport.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;

namespace AKRemindReport
{
    //*****************************************
    /// <summary>
    /// NPOI操作Excel 接口
    /// @author:tengxiaohui
    /// time:2017-08-03
    /// </summary>
    //*****************************************
    public class AkReport
    {
        public static int SheetIndex = 0;
        /// <summary>
        /// 工作表
        /// </summary>

        private static XSSFWorkbook workBook = null;
        public static XSSFWorkbook WorkBook
        {
            get
            {
                if (workBook == null)
                {
                    workBook = new XSSFWorkbook();
                }
                return workBook;
            }
            set
            {
                workBook = value;
            }
        }

        public AkReport()
        {

        }

        #region --------------单元格样式---------------
        /// <summary>
        /// 细线
        /// </summary>
        private ICellStyle _StyleForegroundThin;
        public ICellStyle StyleForegroundThin
        {
            get
            {
                if (_StyleForegroundThin == null)
                {
                    _StyleForegroundThin = WorkBook.CreateCellStyle();
                    _StyleForegroundThin.Alignment = HorizontalAlignment.Center;
                    _StyleForegroundThin.VerticalAlignment = VerticalAlignment.Center;
                    _StyleForegroundThin.BorderLeft = BorderStyle.Thin;
                    _StyleForegroundThin.BorderTop = BorderStyle.Thin;
                    _StyleForegroundThin.BorderRight = BorderStyle.Thin;
                    _StyleForegroundThin.BorderBottom = BorderStyle.Thin;
                    _StyleForegroundThin.FillPattern = FillPattern.SolidForeground;
                    _StyleForegroundThin.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey50Percent.Index;
                    _StyleForegroundThin.WrapText = true;

                    IFont font = WorkBook.CreateFont();
                    font.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
                    _StyleForegroundThin.SetFont(font);

                }
                return _StyleForegroundThin;
            }
        }

        /// <summary>
        /// 细线
        /// </summary>
        private ICellStyle _StyleThin;
        public ICellStyle StyleThin
        {
            get
            {
                if (_StyleThin == null)
                {
                    _StyleThin = WorkBook.CreateCellStyle();
                    _StyleThin.Alignment = HorizontalAlignment.Center;
                    _StyleThin.VerticalAlignment = VerticalAlignment.Center;
                    _StyleThin.BorderLeft = BorderStyle.Thin;
                    _StyleThin.BorderTop = BorderStyle.Thin;
                    _StyleThin.BorderRight = BorderStyle.Thin;
                    _StyleThin.BorderBottom = BorderStyle.Thin;
                    _StyleThin.WrapText = true;
                }
                return _StyleThin;
            }
        }


        /// <summary>
        /// Left加粗
        /// </summary>
        private ICellStyle _StyleLeftMedium;
        public ICellStyle StyleLeftMedium
        {
            get
            {
                if (_StyleLeftMedium == null)
                {
                    _StyleLeftMedium = WorkBook.CreateCellStyle();
                    _StyleLeftMedium.Alignment = HorizontalAlignment.Center;
                    _StyleLeftMedium.VerticalAlignment = VerticalAlignment.Center;
                    _StyleLeftMedium.BorderLeft = BorderStyle.Medium;
                }
                return _StyleLeftMedium;
            }
        }

        /// <summary>
        /// Top加粗
        /// </summary>
        private ICellStyle _StyleTopMedium;
        public ICellStyle StyleTopMedium
        {
            get
            {
                if (_StyleTopMedium == null)
                {
                    _StyleTopMedium = WorkBook.CreateCellStyle();
                    _StyleTopMedium.Alignment = HorizontalAlignment.Center;
                    _StyleTopMedium.VerticalAlignment = VerticalAlignment.Center;
                    _StyleTopMedium.BorderTop = BorderStyle.Medium;
                }
                return _StyleTopMedium;
            }
        }

        /// <summary>
        /// Right加粗
        /// </summary>
        private ICellStyle _StyleRightMedium;
        public ICellStyle StyleRightMedium
        {
            get
            {
                if (_StyleRightMedium == null)
                {
                    _StyleRightMedium = WorkBook.CreateCellStyle();
                    _StyleRightMedium.Alignment = HorizontalAlignment.Center;
                    _StyleRightMedium.VerticalAlignment = VerticalAlignment.Center;
                    _StyleRightMedium.BorderRight = BorderStyle.Medium;
                }
                return _StyleRightMedium;
            }
        }

        /// <summary>
        /// Bottom加粗
        /// </summary>
        private ICellStyle _StyleBottomMedium;
        public ICellStyle StyleBottomMedium
        {
            get
            {
                if (_StyleBottomMedium == null)
                {
                    _StyleBottomMedium = WorkBook.CreateCellStyle();
                    _StyleBottomMedium.Alignment = HorizontalAlignment.Center;
                    _StyleBottomMedium.VerticalAlignment = VerticalAlignment.Center;
                    _StyleBottomMedium.BorderLeft = BorderStyle.Thin;
                    _StyleBottomMedium.BorderTop = BorderStyle.Thin;
                    _StyleBottomMedium.BorderRight = BorderStyle.Thin;
                    _StyleBottomMedium.BorderBottom = BorderStyle.Medium;
                }
                return _StyleBottomMedium;
            }
        }

        /// <summary>
        /// RightBottom加粗
        /// </summary>
        private ICellStyle _StyleRightBottomMedium;
        public ICellStyle StyleRightBottomMedium
        {
            get
            {
                if (_StyleRightBottomMedium == null)
                {
                    _StyleRightBottomMedium = WorkBook.CreateCellStyle();
                    _StyleRightBottomMedium.Alignment = HorizontalAlignment.Center;
                    _StyleRightBottomMedium.VerticalAlignment = VerticalAlignment.Center;
                    _StyleRightMedium.BorderRight = BorderStyle.Medium;
                    _StyleRightBottomMedium.BorderBottom = BorderStyle.Medium;
                }
                return _StyleRightBottomMedium;
            }
        }

        /// <summary>
        /// LeftBottom加粗
        /// </summary>
        private ICellStyle _StyleLeftBottomMedium;
        public ICellStyle StyleLeftBottomMedium
        {
            get
            {
                if (_StyleLeftBottomMedium == null)
                {
                    _StyleLeftBottomMedium = WorkBook.CreateCellStyle();
                    _StyleLeftBottomMedium.Alignment = HorizontalAlignment.Center;
                    _StyleLeftBottomMedium.VerticalAlignment = VerticalAlignment.Center;
                    _StyleLeftBottomMedium.BorderLeft = BorderStyle.Medium;
                    _StyleLeftBottomMedium.BorderBottom = BorderStyle.Medium;
                }
                return _StyleLeftBottomMedium;
            }
        }


        /// <summary>
        /// LeftBottom加粗
        /// </summary>
        private ICellStyle _StyleLeftRightMedium;
        public ICellStyle StyleLeftRightMedium
        {
            get
            {
                if (_StyleLeftRightMedium == null)
                {
                    _StyleLeftRightMedium = WorkBook.CreateCellStyle();
                    _StyleLeftRightMedium.Alignment = HorizontalAlignment.Center;
                    _StyleLeftRightMedium.VerticalAlignment = VerticalAlignment.Center;
                    _StyleLeftRightMedium.BorderLeft = BorderStyle.Medium;
                    _StyleLeftRightMedium.BorderRight = BorderStyle.Medium;
                }
                return _StyleLeftRightMedium;
            }
        }

        /// <summary>
        /// LeftBottom加粗
        /// </summary>
        private ICellStyle _StyleLeftRightBottomMedium;
        public ICellStyle StyleLeftRightBottomMedium
        {
            get
            {
                if (_StyleLeftRightBottomMedium == null)
                {
                    _StyleLeftRightBottomMedium = WorkBook.CreateCellStyle();
                    _StyleLeftRightBottomMedium.Alignment = HorizontalAlignment.Center;
                    _StyleLeftRightBottomMedium.VerticalAlignment = VerticalAlignment.Center;
                    _StyleLeftRightBottomMedium.BorderLeft = BorderStyle.Medium;
                    _StyleLeftRightBottomMedium.BorderRight = BorderStyle.Medium;
                    _StyleLeftRightBottomMedium.BorderBottom = BorderStyle.Medium;
                }
                return _StyleLeftRightBottomMedium;
            }
        }

        /// <summary>
        /// TopBottom加粗
        /// </summary>
        private ICellStyle _StyleTopBottomMedium;
        public ICellStyle StyleTopBottomMedium
        {
            get
            {
                if (_StyleTopBottomMedium == null)
                {
                    _StyleTopBottomMedium = WorkBook.CreateCellStyle();
                    _StyleTopBottomMedium.Alignment = HorizontalAlignment.Center;
                    _StyleTopBottomMedium.VerticalAlignment = VerticalAlignment.Center;
                    _StyleTopBottomMedium.BorderTop = BorderStyle.Medium;
                    _StyleTopBottomMedium.BorderBottom = BorderStyle.Medium;
                }
                return _StyleLeftRightMedium;
            }
        }

        #endregion

        ///// <summary>
        ///// 细线
        ///// </summary>
        //private ICellStyle _styleThin = null;
        //private ICellStyle StyleThin
        //{
        //    //https://www.cnblogs.com/kingangWang/archive/2012/02/06/2339502.html
        //    ///
        //    get
        //    {
        //        if (_styleThin == null)
        //        {
        //            _styleThin = WorkBook.CreateCellStyle();
        //            _styleThin.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
        //            //_titleStyle.BorderBottom = BorderStyle.Dashed;
        //            //_titleStyle.BorderLeft = BorderStyle.Dotted;
        //            //_titleStyle.BorderRight = BorderStyle.Medium;
        //            _styleThin.BorderTop = BorderStyle.Thin;

        //            //_titleStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Red.Index;
        //            //_titleStyle.FillPattern = FillPattern.SolidForeground;

        //            //NPOI.SS.UserModel.IFont _fontTitle = _workBook.CreateFont();
        //            //_fontTitle.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
        //            //_fontTitle.Color = NPOI.HSSF.Util.HSSFColor.Red.Index;
        //            //_fontTitle.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
        //            //short s = (short)new NpoiStyle().Ss();
        //            //_titleStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey80Percent.Index;// NPOI.HSSF.Util.HSSFColor.Red.Index;
        //            //_titleStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Grey80Percent.Index;// NPOI.HSSF.Util.HSSFColor.Red.Index;
        //            //_titleStyle.WrapText = true;

        //            //_titleStyle.SetFont(_fontTitle);
        //        }
        //        return _styleThin;
        //    }
        //}

        /// <summary>
        /// 获取单元线条样式
        /// </summary>
        public ICellStyle GetCellStyle(BorderStyle leftStyle, BorderStyle topStyle, BorderStyle rightStyle, BorderStyle bottomStyle)
        {
            ICellStyle cellStyle = WorkBook.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            //cellStyle.BorderLeft = leftStyle;
            //cellStyle.BorderTop = topStyle;
            //cellStyle.BorderRight = rightStyle;
            //cellStyle.BorderBottom = bottomStyle;
            return cellStyle;
        }
    }
}

