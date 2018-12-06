using AKRemindReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AKRemindReport.Common
{
    public static class AkExtend
    {
        /// <summary>
        ///  长日期时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToLongTime(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        ///  短时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToShortTime(this DateTime dateTime)
        {
            return dateTime.ToString("HH:mm:ss");
        }

        /// <summary>
        /// 短日期
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToShortDate(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 保留小数位
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ConvertDigits(this double db)
        {
            return db.ToString("0.00");
        }

        /// <summary>
        /// 保留小数位
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ConvertDigits(this decimal db, int digit, bool isFormate = true)
        {
            string result = string.Empty;
            if (db == 0) return result;
            if (!isFormate)
            {
                result = db.ToString("0." + "".PadLeft(0, '0'));
            }
            else
            {
                result = db.ToString("0." + "".PadLeft(digit, '0'));
            }
            return result;
        }
        #region[反射设置属性值]

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string GetValue(this AkProduceReportModel entity, string fieldName)
        {
            try
            {
                Type entityType = entity.GetType();
                PropertyInfo propertyInfo = entityType.GetProperty(fieldName);
                object obj = propertyInfo.GetValue(entity, null);
                if (obj != null) return obj.ToString();
            }
            catch
            {
            }
            return null;
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public static bool SetValue(this AkProduceReportModel entity, string fieldName, string fieldValue)
        {
            try
            {
                Type entityType = entity.GetType();
                PropertyInfo propertyInfo = entityType.GetProperty(fieldName);
                propertyInfo.SetValue(entity, fieldValue, null);
                return true;
            }
            catch { }
            return false;
        }
        #endregion
    }
}
