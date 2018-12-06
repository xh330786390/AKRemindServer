using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRemindServer.Common
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
    }
}
