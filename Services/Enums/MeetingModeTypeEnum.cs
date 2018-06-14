using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Enums
{
    /// <summary>
    /// 会议模式枚举
    /// </summary>
    public enum MeetingModeTypeEnum
    {
        /// <summary>
        /// 线上
        /// </summary>
        OnLine=0,

        /// <summary>
        /// 后台线下
        /// </summary>
        BackGroundUnderLine = 1,

        /// <summary>
        /// IWIN线下
        /// </summary>
        IWINUnderLine=2,

        /// <summary>
        /// 大型线下
        /// </summary>
        LargeUnderLine=3
    }
}
