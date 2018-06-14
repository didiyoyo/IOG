using Services.Enums;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class DoctorScheduleService
    {
        /// <summary>
        /// 获取医生行程列表
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public travel_information GetDoctorSchedule(int mid,string openid)
        {
            using(var context=new DBContext())
            {
                return context.travel_information.OrderByDescending(t=>t.id).FirstOrDefault(t => t.mid == mid && t.openid == openid);
            }
        }

        /// <summary>
        /// 医生是否存在行程
        /// 会议类型为大型线下会议且存在行程信息
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public bool IsExistDoctorSchedule(md_seminar_meeting_main meeting, string openid)
        {
            using(var context=new DBContext())
            {
                return meeting.meetingmode == (int)MeetingModeTypeEnum.LargeUnderLine && context.travel_information.FirstOrDefault(t => t.mid == meeting.mid && t.openid == openid) != null;
            }
        }
    }
}
