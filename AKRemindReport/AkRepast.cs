using AKRemindReport.DB;
using AKRemindReport.Models;
using System;
using System.Collections.Generic;

namespace AKRemindReport
{
    /// <summary>
    /// 用餐
    /// </summary>
    public class AkRepast
    {
        ///// <summary>
        ///// 分线提醒记录表名
        ///// </summary>
        //private static string tableName = "tb_repast";

        ///// <summary>
        ///// 初始化
        ///// </summary>
        //static AkRepast()
        //{
        //    CreateTable(tableName);
        //}

        ///// <summary>
        ///// 测试
        ///// </summary>
        //private void Test()
        //{
        //    AkRepastModel model1 = new AkRepastModel()
        //    {
        //        RepastType = 1,
        //        RepastName = "早餐",
        //        StartTime = "06:00",
        //        EndTime = "08:00",
        //        CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
        //        UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        //    };
        //    Save(model1);
        //    AkRepastModel model2 = new AkRepastModel()
        //    {
        //        RepastType = 4,
        //        RepastName = "晚餐",
        //        StartTime = "18:00",
        //        EndTime = "19:30",
        //        CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
        //        UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        //    };

        //    Save(model2);

        //    AkRepastModel model3 = new AkRepastModel()
        //    {
        //        RepastType = 3,
        //        RepastName = "下午茶",
        //        StartTime = "15:30",
        //        EndTime = "16:00",
        //        CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
        //        UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        //    };
        //    Save(model3);
        //    AkRepastModel model4 = new AkRepastModel()
        //    {
        //        RepastType = 2,
        //        RepastName = "中餐",
        //        StartTime = "11:00",
        //        EndTime = "13:00",
        //        CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
        //        UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        //    };
        //    Save(model4);
        //}

        ///// <summary>
        ///// 创建用餐时间表
        ///// </summary>
        ///// <param name="tableName"></param>
        //private static void CreateTable(string tableName)
        //{
        //    if (!SQLiteHelper.Instance.ExistTable(tableName))
        //    {
        //        string sql = string.Format("CREATE TABLE {0} ( " +
        //                                    @"Id            INTEGER   PRIMARY KEY   AUTOINCREMENT,     " +
        //                                    @"RepastType    INT            NOT NULL,	     " +
        //                                    @"RepastName    CHAR( 50 )     NOT NULL,	     " +
        //                                    @"StartTime     CHAR( 8 )      NOT NULL,	     " +
        //                                    @"EndTime       CHAR( 8 )      NOT NULL,         " +
        //                                    @"CreateTime    CHAR( 23 )     NOT NULL,	     " +
        //                                    @"UpdateTime    CHAR( 23 )     NOT NULL )", tableName);
        //        SQLiteHelper.Instance.ExecuteNonQuery(sql);
        //    }
        //}

        //#region ---------------读取用餐时间------------
        ///// <summary>
        ///// 获取所有用餐时间
        ///// </summary>
        ///// <returns></returns>
        //public List<AkRepastModel> GetAllRepast(List<int> tepastTypes = null)
        //{
        //    //测试数据
        //    Test();

        //    List<AkRepastModel> ltResult = new List<AkRepastModel>();

        //    AkRemindDao reminDao = new AkRemindDao();
        //    var breakFast = reminDao.GetTimeInterval("5");
        //    if (breakFast != null) ltResult.Add(breakFast);

        //    var lunch = reminDao.GetTimeInterval("6");
        //    if (lunch != null) ltResult.Add(lunch);

        //    var afternoonTea = reminDao.GetTimeInterval("7");
        //    if (afternoonTea != null) ltResult.Add(afternoonTea);

        //    var supper = reminDao.GetTimeInterval("8");
        //    if (supper != null) ltResult.Add(supper);
        //    return ltResult;
        //}
        //#endregion

        //#region ---------------新增或修改阀值------------
        ///// <summary>
        ///// 新增、修改阀值
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public bool Save(AkRepastModel model)
        //{
        //    string sql = string.Format("SELECT count(1) FROM {0} where RepastType ={1}", tableName, model.RepastType);

        //    //记录存在则修改、否则新增
        //    bool exist = SQLiteHelper.Instance.ExistRecode(sql);

        //    object[] param = null;
        //    if (exist)
        //    {
        //        param = new object[] { tableName, model.StartTime, model.EndTime, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), model.RepastType };
        //        sql = string.Format("Update {0} set  " +
        //                            "StartTime          ='{1}'," +
        //                            "EndTime            ='{2}'," +
        //                            "UpdateTime         ='{3}'" +
        //                            "where RepastType   ={4}", param);
        //    }
        //    else
        //    {
        //        param = new object[] { tableName, model.RepastType, model.RepastName, model.StartTime, model.EndTime, model.CreateTime, model.UpdateTime };
        //        sql = string.Format("insert into {0}(RepastType, RepastName, StartTime, EndTime, CreateTime, UpdateTime) " +
        //                              @"VALUES({1},'{2}','{3}','{4}','{5}','{6}')", param);
        //    }
        //    return SQLiteHelper.Instance.ExecuteNonQuery(sql) > 0 ? true : false;
        //}
        //#endregion
    }
}
