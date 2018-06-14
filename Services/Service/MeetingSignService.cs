using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class MeetingSignService
    {
        /// <summary>
        /// 获得签到信息
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public MeetingSign GetMeetingSignInfo(int mid,string openId)
        {
            using (DBContext db = new DBContext())
            {
                return db.MeetingSign.FirstOrDefault(m => m.meetingid == mid && m.openid == openId);
            }
        }
    }
}
