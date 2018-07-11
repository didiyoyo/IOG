using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB.Model
{
    public class DoctorScheduleInfo
    {
        /// <summary>
        /// 是否显示医生行程按钮
        /// </summary>
        public bool IsDisplayDoctorSchedule { get; set; }

        /// <summary>
        /// 会议信息
        /// </summary>
        public md_seminar_meeting_main Meeting { get; set; }

        /// <summary>
        /// 是否显示未接受邀请图标
        /// </summary>
        public bool IsDisplayInvitationIcon { get; set; }
    }
}