using AKRemindServer.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRemindServer.Dao
{
    public class AkDaoHelper
    {
        private static readonly object Locker = new object();

        /// <summary>
        /// 获取系统实例
        /// </summary>
        /// <returns></returns>
        private static AkSystemParamDao _instance_systemparam;
        public static AkSystemParamDao Instance_SystemParam
        {
            get
            {
                if (_instance_systemparam == null)
                {
                    lock (Locker)
                    {
                        if (_instance_systemparam == null)
                        {
                            _instance_systemparam = new AkSystemParamDao();
                        }
                    }
                }
                return _instance_systemparam;
            }
        }

        /// <summary>
        /// 获取阀值实例
        /// </summary>
        /// <returns></returns>
        private static AkThresholdDao _instance_threshold;
        public static AkThresholdDao Instance_Threshold
        {
            get
            {
                if (_instance_threshold == null)
                {
                    lock (Locker)
                    {
                        if (_instance_threshold == null)
                        {
                            _instance_threshold = new AkThresholdDao();
                        }
                    }
                }
                return _instance_threshold;
            }
        }

        /// <summary>
        /// 获取分线提醒实例
        /// </summary>
        /// <returns></returns>
        private static AkRemindDao _instance_remind;
        public static AkRemindDao Instance_Remind
        {
            get
            {
                if (_instance_remind == null)
                {
                    lock (Locker)
                    {
                        if (_instance_remind == null)
                        {
                            _instance_remind = new AkRemindDao();
                        }
                    }
                }
                return _instance_remind;
            }
        }
    }
}
