using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class MeetingSign
    {
        [Key]
        public int id { get; set; }
        public int meetingid { get; set; }
        public string openid { get; set; }
        /// <summary>
        /// 随机签到邀请码
        /// </summary>
        public string randominvitationcode { get; set; }
        public DateTime? createTime { get; set; }
    }
}
