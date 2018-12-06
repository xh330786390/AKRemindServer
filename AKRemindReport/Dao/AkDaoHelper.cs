using AKRemindReport.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRemindReport.Dao
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

        /// <summary>
        /// 获取产区实例
        /// </summary>
        /// <returns></returns>
        private static AkProductDao _instance_product;
        public static AkProductDao Instance_Product
        {
            get
            {
                if (_instance_product == null)
                {
                    lock (Locker)
                    {
                        if (_instance_product == null)
                        {
                            _instance_product = new AkProductDao();
                        }
                    }
                }
                return _instance_product;
            }
        }

        /// <summary>
        /// 获取服务区实例
        /// </summary>
        /// <returns></returns>
        private static AkServerDao _instance_server;
        public static AkServerDao Instance_Server
        {
            get
            {
                if (_instance_server == null)
                {
                    lock (Locker)
                    {
                        if (_instance_server == null)
                        {
                            _instance_server = new AkServerDao();
                        }
                    }
                }
                return _instance_server;
            }
        }
    }
}
