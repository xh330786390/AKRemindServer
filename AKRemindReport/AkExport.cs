using AKRemindReport.Dao;
using AKRemindReport.Models;
using Common.NLog;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AKRemindReport
{
    /// <summary>
    /// 报表导出操作
    /// </summary>
    public class AkExport
    {
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="lt_repeast"></param>
        public static void Export(List<int> lt_repast, string file)
        {
            //读取系统配置参数
            AkConfig.ReadParam();

            AkDbfDao akDbfDao = new AkDbfDao();
            //读取dbf数据
            AkDbfDao.DbfTable = akDbfDao.GetDbfFile();

            //获取服务区报表数据
            AkServer akServer = new AkServer();
            var serverDatas = akServer.GetDatas(lt_repast);

            if (serverDatas != null && serverDatas.Count > 0)
            {
                List<int> ltStyle = new List<int>();
                List<AkMergedRange> ltServerMergeds = akServer.GetMergedRanges(lt_repast, ref ltStyle);
                AkServerReport akServerReport = new AkServerReport();
                akServerReport.Export(serverDatas, ltServerMergeds, ltStyle);
            }

            //获取产区报表数据
            AkProduct akProduct = new AkProduct();
            List<AkProduceReportModel> produceDatas = akProduct.GetDatas(lt_repast);

            if (produceDatas != null && produceDatas.Count > 0)
            {
                List<int> ltStyle = new List<int>();
                List<AkMergedRange> ltProductMergeds = akProduct.GetMergedRanges(lt_repast, ref ltStyle);
                AkProductReport akProductReport = new AkProductReport();
                akProductReport.Export(produceDatas, ltProductMergeds, ltStyle);
            }
            CreateFile(file);

            //清空Dbf数据
            AkDbfDao.Clear();
        }

        /// <summary>
        /// 导出报表
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="lt_data"></param>
        private static void CreateFile(string file)
        {
            try
            {
                byte[] bytes = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    AkReport.WorkBook.Write(ms);
                    ms.Flush();
                    AkReport.WorkBook.Clear();
                    AkReport.WorkBook = null;
                    AkReport.SheetIndex = 0;
                    bytes = ms.ToArray();
                }

                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Flush();
                }
            }
            catch (Exception er)
            {
                LogHelper.Error(typeof(AkProductReport) + ".Export Exception error=", er.ToString());
            }
        }
    }
}

