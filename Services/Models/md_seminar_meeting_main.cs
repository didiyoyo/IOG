using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public partial class md_seminar_meeting_main
    {
        [Key]
        public int mid { get; set; }
        [Display(Name = "BU")]
        public string brand { get; set; }
        [Display(Name = "会议编号")]
        public string mcode { get; set; }
        [Display(Name = "会议类型")]
        public string mtype { get; set; }
        [Display(Name = "会议名称")]
        public string mtitle { get; set; }
        [Display(Name = "会议简介")]
        public string mcontent { get; set; }
        [Display(Name = "城市")]
        public string mregion { get; set; }
        [Display(Name = "省份")]
        public string province { get; set; }
        public string mfontdata { get; set; }
        public string mlaterdata { get; set; }
        [Display(Name = "召开时间")]
        public DateTime? mbegintime { get; set; }
        [Display(Name = "结束时间")]
        public DateTime? mendtime { get; set; }
        public string mdatapassword { get; set; }
        [Display(Name = "等级")]
        public string mgrade { get; set; }
        public string mhyrc_img { get; set; }
        public string mhyrc_url { get; set; }
        public string mhyrc_type { get; set; }
        public string mmap_img { get; set; }
        public string mmeet_banner { get; set; }
        public string mspot_banner { get; set; }
        public string mhd_img { get; set; }
        [Display(Name = "详细地址")] 
        public string msite { get; set; }
        public string mremark { get; set; }
        public string yqhewm_img { get; set; }
        public string qdewm_img { get; set; }
        [Display(Name = "投票")]
        public bool vote { get; set; }
        [Display(Name = "调研")] 
        public bool survey { get; set; }
        [Display(Name = "提问")]
        public bool question { get; set; }
        [Display(Name = "直播")]
        public bool live { get; set; }
        [Display(Name = "直播地址")]
        public string liveURL { get; set; }
        public string createtime { get; set; }
        public int? meetingcount { get; set; }
        [Display(Name = "创建人")] 
        public string createby { get; set; }
        public string InvitationCode { get; set; }
        /// <summary>
        /// 1图片2url
        /// </summary>
        [Display(Name = "讲者简介类型")]
        public int? doctortype { get; set; }
        [Display(Name = "讲者简介图片")]
        public string doctorimg { get; set; }
        [Display(Name = "讲者简介URL")]
        public string doctorurl { get; set; }

        public int? meetingmode { get; set; }

        public string introduction { get; set; }

        public string speakers { get; set; }

        public string chairman { get; set; }

        public int? pushCount { get; set; }

        /// <summary>
        /// 会议类型0正常1取消
        /// </summary>
        [Display(Name = "会议状态")]
        public int? Type { get; set; }

        public string survaytitle { get; set; }
    }
}
