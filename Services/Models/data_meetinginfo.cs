using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class data_meetinginfo
    {
        public string mid { get; set; }
        public string mtitle { get; set; }
        public string msite { get; set; }
        public DateTime? mbegintime { get; set; }
        public DateTime? mendtime { get; set; }
        /// <summary>
        /// 邀请医生总数
        /// </summary>
        public int? InvitedDocCount { get; set; }
        /// <summary>
        /// 医生预约参会总数
        /// </summary>
        public int? JoinDocCount { get; set; }
        /// <summary>
        /// 会议详情点击数
        /// </summary>
        public int? MeetingInfoCount { get; set; }
        /// <summary>
        /// 在线观看点击数
        /// </summary>
        public int? LiveCount { get; set; }
        /// <summary>
        /// 在线观看用户数
        /// </summary>
        public int? LiveByUserCount { get; set; }
        /// <summary>
        /// 投票用户数
        /// </summary>
        public int? VoteCount { get; set; }
        /// <summary>
        /// 调研用户数
        /// </summary>
        public int? SurveyCount { get; set; }
        /// <summary>
        /// 提问用户数
        /// </summary>
        public int? QuestionCount { get; set; }
        /// <summary>
        /// 发送邮件用户数
        /// </summary>
        public int? SendEailCount { get; set; }

    }
}
