using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using AKRemindServer.DB;
using System.Threading;

namespace AKRemindServer
{
    public class AkTask
    {
        /// <summary>
        /// 获取等待时长,单位ms
        /// 首次运行时间，为设置间隔时间的倍数，之后则按设置时间间隔轮询进行
        /// </summary>
        /// <returns></returns>
        public static int WatiTime(string lastTaskTime)
        {
            int waitValue = 0;
            if (AkConfig.SysParam == null)
            {
                AkConfig.SysParam = new Models.AkSystemParamModel();
                AkConfig.SysParam.TaskTime = 900;
            }

            if (AkConfig.SysParam.TaskTime > 0)
            {
                int minite = AkConfig.SysParam.TaskTime / 60;

                DateTime startTime = DateTime.Now;
                DateTime endTime = DateTime.Now;
                for (int i = 0; i <= minite; i++)
                {
                    endTime = DateTime.Parse(startTime.AddMinutes(i).ToString("yyyy-MM-dd HH:mm:01"));
                    if (!string.IsNullOrEmpty(lastTaskTime))
                    {
                        if (DateTime.Parse(lastTaskTime).ToString("yyyy-MM-dd HH:mm").CompareTo(endTime.ToString("yyyy-MM-dd HH:mm")) < 0
                            && endTime.Minute % minite == 0)
                        {
                            TimeSpan timeSpane = (endTime - DateTime.Now);
                            waitValue = (int)(timeSpane.TotalMilliseconds + 1);
                            break;
                        }
                    }
                    else if (endTime.Minute % minite == 0)
                    {
                        TimeSpan timeSpane = (endTime - DateTime.Now);
                        waitValue = (int)timeSpane.TotalMilliseconds;
                        if (waitValue < 0)
                        {
                            waitValue = 0;
                        }
                        break;
                    }
                }
            }

            return waitValue;
        }
    }
}
