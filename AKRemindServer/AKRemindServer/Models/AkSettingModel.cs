using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRemindServer.Models
{
    [Serializable]
    public class AkSettingModel
    {
        /// <summary>
        /// 周期值
        /// </summary>
        public int PeriodNum { get; set; }
        /// <summary>
        /// 周期更新频率值
        /// </summary>
        public int UpdateNum { get; set; }
    }
}
